from datetime import datetime, timedelta
from joblib import dump, load
from lightgbm import LGBMClassifier
import numpy as np
from pydantic import BaseModel
import pyodbc
import pandas as pd
from sklearn import preprocessing
from sklearn.metrics import accuracy_score 
from sklearn.linear_model import BayesianRidge
from sklearn.model_selection import train_test_split
from sklearn.pipeline import make_pipeline
from sqlalchemy import create_engine

class TrainRequest(BaseModel):
    cryptoId: int
    startTime: datetime
    endTime: datetime

class TrainResponse(BaseModel):
    output: str
    modelName: str
    accuracy: float
    samples: int

class PredictionModel(BaseModel):
    output: str
    modelName: str

class PredictionRequest(BaseModel):
    cryptoId: int
    time: datetime
    models: list[PredictionModel]

class PredictionResponse(BaseModel):
    cryptoId: int
    time: datetime
    predictions: dict
    
class data:
  def __init__(self, host: str, database: str, unix=False):
    driver = 'PostgreSQL Unicode(x64)'
    if unix:
      driver='PostgreSQL Unicode'
    self.connection_string = f'DRIVER={driver};SERVER={host};PORT=5432;DATABASE={database};UID=postgres;PWD=;'
    self.engine = create_engine(f'postgresql://postgres:@{host}:5432/{database}')

    boolean_columns, integer_columns, numeric_columns, ignore_columns, future_columns = self.get_columns_by_type()
    integer_columns.append('price_trends_st_upper')
    integer_columns.append('price_trends_st_lower')
    integer_columns.append('price_volatilities_vstop_upper')
    integer_columns.append('price_volatilities_vstop_lower')
    integer_columns.append('price_trends_atr_sell')
    integer_columns.append('price_trends_atr_buy')

    self.boolean_columns = boolean_columns
    self.integer_columns = integer_columns
    self.numeric_columns = numeric_columns
    self.future_columns = future_columns
    self.ignored_columns = ignore_columns

    self.classifier_columns = [
      'price_peak_highest_high',
      'price_peak_lowest_low',
      'price_return_day_rank2',
      'price_return_day_rank3',
      'price_return_day_rank4',
      'price_return_day_rank6',
      'price_return_day_rank8',
      'price_return_day_rank12',
      'price_return_twoday_rank2',
      'price_return_twoday_rank3',
      'price_return_twoday_rank4',
      'price_return_twoday_rank6',
      'price_return_twoday_rank8',
      'price_return_twoday_rank12',
      'price_return_week_rank2',
      'price_return_week_rank3',
      'price_return_week_rank4',
      'price_return_week_rank6',
      'price_return_week_rank8',
      'price_return_week_rank12',
    ]

    self.regression_columns = [
      'price_high',
      'price_low',
    ]
  
  def query_to_df(self, sql: str):
    cnxn = pyodbc.connect(self.connection_string)
    return pd.read_sql(sql, cnxn)

  def df_to_db(self, df: pd.DataFrame, table_name: str):
    df.to_sql(table_name, self.engine, if_exists='replace', index=False)

  def get_columns(self):
    query = """SELECT table_name, column_name, data_type, col_description((table_schema||'.'||table_name)::regclass::oid, ordinal_position) as column_comment
  FROM information_schema.columns
  WHERE table_name LIKE 'price%' AND table_name != 'price_prediction'
  ORDER BY table_name, ordinal_position"""
    columns = self.query_to_df(query)
    columns = columns[3:]
    columns = columns[columns.column_name != 'id']
    return columns

  def get_columns_by_type(self):
    columns = self.get_columns()
    # get columns where data_type is 'integer' or 'boolean'
    boolean_columns = []
    integer_columns = []
    numeric_columns = []
    ignore_columns = []
    future_columns = []
    for index, row in columns.iterrows():
        if row['data_type'] == 'boolean':
            boolean_columns.append(f"{row['table_name']}_{row['column_name']}")
        if row['data_type'] == 'integer':
            integer_columns.append(f"{row['table_name']}_{row['column_name']}")
        if row['data_type'] == 'numeric':
            numeric_columns.append(f"{row['table_name']}_{row['column_name']}")
        if row['column_comment'] == 'ignore_prediction':
            ignore_columns.append(f"{row['table_name']}_{row['column_name']}")
        if row['column_comment'] == 'future':
            future_columns.append(f"{row['table_name']}_{row['column_name']}")

    return boolean_columns, integer_columns, numeric_columns, ignore_columns, future_columns

  def get_data_raw(self, crypto_id: int, start_time: str, end_time: str):
    columns = self.get_columns()
    select_str = ''
    for index, row in columns.iterrows():
      select_str += f", {row['table_name']}.{row['column_name']} as {row['table_name']}_{row['column_name']}"
    select_str = select_str[2:]

    tables = columns.table_name.unique()
    tables = [table for table in tables if table != 'price']
    join_str = ''
    for table in tables:
      join_str += f"LEFT JOIN {table} ON {table}.id = price.id\n"
      
    query = f"""
  SELECT 
  {select_str} 
  FROM price
  {join_str}
  WHERE price.crypto_id = {crypto_id} AND price.time_open >= '{start_time}' AND price.time_open < '{end_time}'
  ORDER BY price.time_open"""
    #print(query)

    return self.query_to_df(query)
  
  def get_data_columns_prepared(self, crypto_id: int, start_time: str, end_time: str):
    #boolean_columns, integer_columns, numeric_columns, ignore_columns, future_columns = self.get_columns_by_type()
    

    #columns_to_scale = list(set(numeric_columns))
    #columns_to_scale.append('price_trades')
    #columns_to_scale.append('price_other_indicators_pivot_hightrend')
    #columns_to_scale.append('price_trends_hilb_dcp')

    data = self.get_data_raw(crypto_id, start_time, end_time)
    data.drop(columns=self.ignored_columns, inplace=True)
    data.drop(columns=['price_time_open', 'price_open', 'price_close'], inplace=True)
    
    data['price_momentums_go_upper_exp'] = data['price_momentums_go_upper_exp'].astype(int)
    data['price_momentums_go_lower_exp'] = data['price_momentums_go_lower_exp'].astype(int)
    data['price_trends_atr_sell'] = data['price_trends_atr_sell'].notnull().astype(int)
    data['price_trends_atr_buy'] = data['price_trends_atr_buy'].notnull().astype(int)
    data['price_trends_par_rev'] = data['price_trends_par_rev'].astype(int)
    data['price_trends_st_upper'] = data['price_trends_st_upper'].notnull().astype(int)
    data['price_trends_st_lower'] = data['price_trends_st_lower'].notnull().astype(int)
    data['price_volatilities_sdc_bp'] = data['price_volatilities_sdc_bp'].astype(int)
    data['price_volatilities_vstop_upper'] = data['price_volatilities_vstop_upper'].notnull().astype(int)
    data['price_volatilities_vstop_lower'] = data['price_volatilities_vstop_lower'].notnull().astype(int)
    data['price_volatilities_vstop_stop'] = data['price_volatilities_vstop_stop'].astype(int)

    for column in self.boolean_columns:
      # if boolean convert to int
      data[column] = data[column].replace({True: 1, False: 0})

    #scaler = preprocessing.StandardScaler()
    #normalizer = preprocessing.Normalizer()
    #for column in columns_to_scale:
    #    # if data contains column
    #    if column in data.columns:
    #        #data[column] = scaler.fit_transform(data[column].values.reshape(-1, 1))
    #        data[column] = normalizer.fit_transform(data[column].values.reshape(-1, 1))
    #        data[column].astype(np.float64)   

    return data
  


  def prepare_data_train(self, df: pd.DataFrame, output_column: str):
    X = df.drop(columns=self.future_columns)
    y = df[[output_column]]
    y = y.shift(-1)

    # remove last row (no output for it)
    X = X[:-1]
    y = y[:-1]
    return X, y
  
  def get_data_train(self, crypto_id: int, start_time: str, end_time: str, output_column: str):
    data = self.get_data_columns_prepared(crypto_id, start_time, end_time)
    X, y = self.prepare_data_train(data, output_column)
    return X, y
  
  def prepare_data_predict(self, df: pd.DataFrame):
    X = df.drop(columns=self.future_columns)
    return X
  
  def get_data_predict(self, crypto_id: int, start_time: str, end_time: str, output_column: str):
    data = self.get_data_columns_prepared(crypto_id, start_time, end_time)
    X = self.prepare_data_predict(data, output_column)
    return X
  
  def prepare_data_train_window(self, df: pd.DataFrame, output_column: str, window_size: int):
    #X = df.drop(columns=[output_column])
    X = df.drop(columns=self.future_columns)
    # create sliding window
    for i in range(1, window_size):
        X = pd.concat([X, df.drop(columns=self.future_columns).shift(i).add_suffix(f'_{i}')], axis=1)

    y = df[[output_column]]
    y = y.shift(-1)

    # remove first n rows (no past data for them)
    X = X[window_size:]
    y = y[window_size:]

    # remove last row (no output for it)
    X = X[:-1]
    y = y[:-1]
    return X, y

  def get_data_train_window(self, crypto_id: int, start_time: str, end_time: str, output_column, window_size: int):
    data = self.get_data_columns_prepared(crypto_id, start_time, end_time)
    X, y = self.prepare_data_train_window(data, output_column, window_size)
    return X, y
  

  def train_classifier(self, crypto_id: int, start_time: datetime, end_time: datetime, output_dir: str):
    df = self.get_data_columns_prepared(crypto_id, start_time.isoformat(), end_time.isoformat())

    result = []
    for col in self.classifier_columns:
        X, y = self.prepare_data_train(df, col)

        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42, shuffle=False)

        pipeline = make_pipeline(preprocessing.MinMaxScaler(), LGBMClassifier())
        pipeline.fit(X_train, y_train) 

        model_name = f'LGBM_{crypto_id}_{col}_{start_time.strftime("%Y%m%d%H%M%S")}_{end_time.strftime("%Y%m%d%H%M%S")}.joblib'
        dump(pipeline, f'{output_dir}/{model_name}')
        y_pred = pipeline.predict(X_test) 

        accuracy = accuracy_score(y_test, y_pred) 
        result.append(TrainResponse(output = col, modelName = model_name, accuracy = accuracy, samples=len(X)))

    return result
  
  def train_regression(self, crypto_id: int, start_time: datetime, end_time: datetime, output_dir: str):
    df = self.get_data_columns_prepared(crypto_id, start_time.isoformat(), end_time.isoformat())
    df.dropna(inplace=True)
    result = []
    for col in self.regression_columns:
      X, y = self.prepare_data_train(df, col)

      X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42, shuffle=False)

      pipeline = make_pipeline(preprocessing.MinMaxScaler(), BayesianRidge())
      try:
        pipeline.fit(X_train, y_train) 

        model_name = f'BayesianRidge_{crypto_id}_{col}_{start_time.strftime("%Y%m%d%H%M%S")}_{end_time.strftime("%Y%m%d%H%M%S")}.joblib'
        dump(pipeline, f'{output_dir}/{model_name}')
        #y_pred = pipeline.predict(X_test) 

        result.append(TrainResponse(output = col, modelName = model_name, accuracy = 0.0, samples=len(X)))
      except Exception as e:
        nan_count = X.isna().sum().sort_values(ascending = False).head(50)
        for index, value in nan_count.iteritems():
          print(index, value)

    return result
    
  def predict(self, crypto_id: int, time: datetime, models: list[PredictionModel]):
    end_time = time + timedelta(hours=1)
    df = self.get_data_columns_prepared(crypto_id, time.isoformat(), end_time.isoformat())

    result = PredictionResponse(cryptoId=crypto_id, time=time, predictions={})

    for model in models:
        X = self.prepare_data_predict(df)
        pipeline = load(f'/models/{model.modelName}')
        y_pred = pipeline.predict(X) 
        result.predictions[model.output] = y_pred[-1]

    return result