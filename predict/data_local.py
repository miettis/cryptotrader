import pyodbc
import pandas as pd
from sqlalchemy import create_engine
engine = create_engine('postgresql://postgres:@localhost:5432/binance_1')

connection_string = (
  'DRIVER=PostgreSQL Unicode(x64);'
  'SERVER=localhost;'
  'PORT=5432;'
  'DATABASE=binance_1;'
  'UID=postgres;'
  'PWD=;'
)

def query_to_df(sql):
  cnxn = pyodbc.connect(connection_string)
  return pd.read_sql(sql, cnxn)

def df_to_db(df: pd.DataFrame, table_name):
  df.to_sql(table_name, engine, if_exists='replace', index=False)

def get_columns():
  query = """SELECT table_name, column_name, data_type
FROM information_schema.columns
WHERE table_name LIKE 'price%' AND table_name != 'price_prediction' AND column_name NOT LIKE 'prs_%'
ORDER BY table_name, ordinal_position"""
  columns = query_to_df(query)
  columns = columns[3:]
  columns = columns[columns.column_name != 'id']
  return columns

def get_columns_by_type():
  columns = get_columns()
  # get columns where data_type is 'integer' or 'boolean'
  class_columns = []
  numeric_columns = []
  for index, row in columns[(columns['data_type'] == 'integer') | (columns['data_type'] == 'boolean')].iterrows():
      class_columns.append(f"{row['table_name']}_{row['column_name']}")
  for index, row in columns[(columns['data_type'] == 'numeric')].iterrows():
      numeric_columns.append(f"{row['table_name']}_{row['column_name']}")

  return class_columns, numeric_columns

def get_data(crypto_id: int, start_time: str, end_time: str):
  columns = get_columns()
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

  return query_to_df(query)