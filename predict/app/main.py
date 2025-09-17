from typing import Union
from fastapi import FastAPI
from datetime import datetime, timedelta
from joblib import dump, load
from lightgbm import LGBMClassifier
import pyodbc
from pydantic import BaseModel
from scipy.signal import savgol_filter
from sklearn.linear_model import BayesianRidge
from sklearn.model_selection import train_test_split
from sklearn.pipeline import make_pipeline
from sklearn.preprocessing import MinMaxScaler
import numpy as np
import tensorflow as tf
from data import data
from sklearn.metrics import accuracy_score 

host = 'postgres'
database = 'binance_1'

class SavgolRequestValues(BaseModel):
    field: str
    values: list[float]

class SavgolRequest(BaseModel):
    data: list[SavgolRequestValues]
    windows: list[int]
    order: int

class SavgolResponseValues:
    field: str
    window: int
    order: int
    smooth: list[float]
    derivatives: list[float]

class CandleRequest(BaseModel):
    open: list[float]
    high: list[float]
    low: list[float]
    close: list[float]

class CandleResponse(BaseModel):
    pattern: str
    result: list[int]

connection_string = (
  'DRIVER=PostgreSQL Unicode;'
  'SERVER=postgres;'
  'PORT=5432;'
  'DATABASE=binance_1;'
  'UID=postgres;'
  'PWD=;'
)
app = FastAPI()

@app.get("/")
def read_root():
    return {"Hello": "World"}


@app.get("/items/{item_id}")
def read_item(item_id: int, q: Union[str, None] = None):
    return {"item_id": item_id, "q": q}

@app.get("/drivers")
def get_drivers():
    return pyodbc.drivers()

@app.get("/test_db")
def test_db():
    now = datetime.utcnow()
    next_hour = now.replace(hour = now.hour + 1, minute = 0, second = 0, microsecond = 0)

    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()

    cursor.execute(f"""
    INSERT INTO prediction_hour(crypto_id,time_open,open,high,low,close,created)
    VALUES(5,'{next_hour.strftime("%Y-%m-%d %H:%M:%S")}', 1, 2, 3, 4, '{now.strftime("%Y-%m-%d %H:%M:%S")}') RETURNING id;""")
    id = cursor.fetchone()[0]
    conn.commit()

    return id

@app.post("/savgol")
def get_savgol(input: SavgolRequest):
    response = []
    for window in input.windows:
        for data in input.data:
            s = savgol_filter(data.values, window, input.order, deriv=0)
            d = savgol_filter(data.values, window, input.order, deriv=1)
            #result = SavgolResponseValues(field=data.field, window=window, order=input.order, smooth=s.tolist(), derivatives=d.tolist())
            #response.append(result)
            
            result = SavgolResponseValues()
            result.field = data.field
            result.window = window
            result.order = input.order
            result.smooth = s
            result.derivatives = d
            response.append(result) 
        #s = savgol_filter(input.values, input.window, input.order, deriv=0)
        #d = savgol_filter(input.values, input.window, input.order, deriv=1)
        #results.push({ "smooth": s.tolist(), "derivatives": d.tolist() })
    #s = savgol_filter(input.values, input.window, input.order, deriv=0)
    #d = savgol_filter(input.values, input.window, input.order, deriv=1)
    #print(d)
    return response
    
@app.post("/savgol/v2")
def get_savgol_v2(input: SavgolRequest):
    response = []
    for window in input.windows:
        for data in input.data:
            smooth = []
            derivatives = []
            
            for _ in range(1, window ):
                smooth.append(None)
                derivatives.append(None)

            for end_index in range(window, len(data.values) + 1):
                start_index = max(0, end_index - 3 * window)
                try:
                    values = data.values[start_index:end_index]
                    s = savgol_filter(values, window, input.order, deriv=0)
                    d = savgol_filter(values, window, input.order, deriv=1)

                    smooth.append(s[-1])
                    derivatives.append(d[-1])
                except Exception as e:
                    print(data.values[start_index:end_index])
                    continue
                    
            result = SavgolResponseValues()
            result.field = data.field
            result.window = window
            result.order = input.order
            result.smooth = smooth
            result.derivatives = derivatives
            response.append(result) 

    return response

@app.post("/predict")
def predict(input: data.PredictionRequest):
    db = data.data(host, database, True)
    result = db.predict(input.cryptoId, input.time, input.models)
    """
    df = db.get_data_columns_prepared(5, input.time.isoformat(), end_time.isoformat())

    result = PredictionResponse(cryptoId=input.cryptoId, time=input.time, predictions={})

    for model in input.models:
        X = db.prepare_data_predict(df)
        pipeline = load(f'/models/{model.modelName}')
        y_pred = pipeline.predict(X) 
        result.predictions[model.output] = y_pred[-1]
    """
    return result

@app.post("/train-classifier")
def train_model(input: data.TrainRequest):
    db = data.data(host, database, True)
    result = db.train_classifier(input.cryptoId, input.startTime, input.endTime, '/models')
    """
    df = db.get_data_columns_prepared(input.cryptoId, input.startTime.isoformat(), input.endTime.isoformat())

    result = []
    for col in classifier_columns:
        X, y = db.prepare_data_train(df, col)

        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42, shuffle=False)

        pipeline = make_pipeline(MinMaxScaler(), LGBMClassifier())
        pipeline.fit(X_train, y_train) 

        model_name = f'LGBM_{input.cryptoId}_{col}_{input.startTime.strftime("%Y%m%d%H%M%S")}_{input.endTime.strftime("%Y%m%d%H%M%S")}.joblib'
        dump(pipeline, f'/models/{model_name}')
        y_pred = pipeline.predict(X_test) 

        accuracy = accuracy_score(y_test, y_pred) 
        result.append(TrainResponse(output = col, modelName = model_name, accuracy = accuracy))
    """
    return result

@app.post("/train-regression")
def train_model(input: data.TrainRequest):
    db = data.data(host, database, True)
    result = db.train_regression(input.cryptoId, input.startTime, input.endTime, '/models')
    """
    df = db.get_data_columns_prepared(input.cryptoId, input.startTime.isoformat(), input.endTime.isoformat())

    result = []
    for col in regression_columns:
        X, y = db.prepare_data_train(df, col)

        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42, shuffle=False)

        pipeline = make_pipeline(MinMaxScaler(), BayesianRidge())
        pipeline.fit(X_train, y_train) 

        model_name = f'BayesianRidge_{input.cryptoId}_{col}_{input.startTime.strftime("%Y%m%d%H%M%S")}_{input.endTime.strftime("%Y%m%d%H%M%S")}.joblib'
        dump(pipeline, f'/models/{model_name}')
        y_pred = pipeline.predict(X_test) 

        accuracy = accuracy_score(y_test, y_pred) 
        result.append(TrainResponse(output = col, modelName = model_name, accuracy = accuracy))
    """
    return result