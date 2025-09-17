using CryptoTrader.Data;
using Microsoft.CSharp;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.CodeDom.Compiler;
using System.Text;

namespace CryptoTrader.ML.Console
{
    internal class ModelBuilder
    {
        public static void CreateModel(int numberOfPrevious)
        {
            
            var codes = Enumerable.Range(1, numberOfPrevious).ToDictionary(x => "Previous" + x, x => new StringBuilder());
            codes.Add("Current", new StringBuilder());
            codes.Add("Next", new StringBuilder());
            var currentPropertiesCode = new StringBuilder();
            var nextPropertiesCode = new StringBuilder();

            void AddProperties(Type type, string prefix = "")
            {
                if (type.IsClass)
                {
                    foreach (var property in type.GetProperties())
                    {
                        if (property.PropertyType == typeof(Crypto) || property.PropertyType == typeof(Price) || property.Name == "Id" || property.Name == "CryptoId")
                        {
                            continue;
                        }
                        else if (property.PropertyType.IsClass || property.PropertyType.IsEnum)
                        {
                            AddProperties(property.PropertyType, prefix + property.Name);
                        }
                        else
                        {
                            var propertyType = property.PropertyType switch
                            {

                                var t when t == typeof(decimal) || t == typeof(decimal?) => "float",
                               //var t when t == typeof(long) || t == typeof(long?) => "long",
                                var t when t == typeof(int) || t == typeof(int?) => "float",
                                var t when t == typeof(bool) || t == typeof(bool?) => "bool",
                                _ => null
                            };
                            if(propertyType == null)
                            {
                                continue;
                            }
                            foreach(var key in codes.Keys)
                            {
                                codes[key].AppendLine($"        public {propertyType} {key}{prefix}{property.Name} {{ get; set; }}");
                            }
                            //previousPropertiesCode.AppendLine($"        public {propertyType} Previous{prefix}{property.Name} {{ get; set; }}");
                            //codes["Current"].AppendLine($"        public {propertyType} Current{prefix}{property.Name} {{ get; set; }}");
                            //codes["Next"].AppendLine($"        public {propertyType} Next{prefix}{property.Name} {{ get; set; }}");

                        }
                    }
                }
                else if (type.IsEnum)
                {
                    foreach (var value in Enum.GetValues(type))
                    {
                        var name = Enum.GetName(type, value);
                        foreach (var key in codes.Keys.Where(x => x.StartsWith("Previous")))
                        {
                            codes[key].AppendLine($"        public bool {key}{prefix}{name} {{ get; set; }}");
                        }
                        //previousPropertiesCode.AppendLine($"        public bool Previous{prefix}{name} {{ get; set; }}");
                        //codes["Current"].AppendLine($"        public bool Current{prefix}{name} {{ get; set; }}");
                        //codes["Next"].AppendLine($"        public bool Next{prefix}{name} {{ get; set; }}");
                    }
                }
            }

            AddProperties(typeof(Price));

            var propertiesCode = new StringBuilder();
            foreach (var key in codes.Keys.Where(x => x.StartsWith("Previous")).OrderByDescending(x => x))
            {
                propertiesCode.AppendLine(codes[key].ToString());
            }
            propertiesCode.AppendLine(codes["Current"].ToString());
            propertiesCode.AppendLine(codes["Next"].ToString());
            var sourceCode = @$"using System;

    public class PriceModel{numberOfPrevious}
    {{      
{propertiesCode}
    }}";


            File.WriteAllText(@$"..\..\..\PriceModel{numberOfPrevious}.cs", sourceCode);

        }
        public static T[] MapData<T>(Price[] data, int numberOfPrevious) where T : new()
        {
            var missing = new Dictionary<string, int>();
            var result = new List<T>();
            var properties = typeof(T).GetProperties().ToDictionary(x => x.Name);
            //var priceProperties = typeof(Price).GetProperties().ToDictionary(x => x.Name);
            void AddMissing(string property)
            {
                if(!missing.ContainsKey(property))
                {
                    missing[property] = 0;
                }
                missing[property]++;
            }
            void Assign(T model, Type valueType, object value, string prefix)
            {
                if (valueType.IsClass)
                {
                    foreach (var property in valueType.GetProperties())
                    {
                        if (property.PropertyType == typeof(Crypto) || property.PropertyType == typeof(Price) || property.Name == "Id" || property.Name == "CryptoId")
                        {
                            continue;
                        }

                        var propertyValue = value == null ? null : property.GetValue(value);
                        if (property.PropertyType.IsClass || property.PropertyType.IsEnum)
                        {
                            Assign(model, property.PropertyType, propertyValue, prefix + property.Name);
                        }
                        else
                        {
                            var modelPropertyName = prefix + property.Name;
                            if (properties.ContainsKey(modelPropertyName))
                            {
                                
                                if (propertyValue == null && (property.PropertyType == typeof(decimal?) || property.PropertyType == typeof(int?) ))
                                {
                                    AddMissing(modelPropertyName);
                                    propertyValue = float.NaN;
                                }
                                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                                {
                                    propertyValue = (float)(decimal)propertyValue;
                                }
                                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                                {
                                    propertyValue = (float)(int)propertyValue;
                                }

                                properties[modelPropertyName].SetValue(model, propertyValue);
                            }
                        }
                    }
                }
                if(valueType.IsEnum && value != null)
                {
                    var name = Enum.GetName(valueType, value);
                    var modelPropertyName = prefix + name;
                    if (properties.ContainsKey(modelPropertyName))
                    {
                        properties[modelPropertyName].SetValue(model, true);
                    }
                }
            }

            for (var i = numberOfPrevious; i < data.Length - 1; i++)
            {

                //var previous = data[i - 1];
                //var previous = data[(i - numberOfPrevious) .. i];
                var current = data[i];
                var next = data[i + 1];
                var model = new T();

                for (var prev = 1; prev <= numberOfPrevious; prev++)
                {
                    var previous = data[i - prev];
                    Assign(model, typeof(Price), previous, $"Previous{prev}");
                }

                //Assign(model, typeof(Price), previous, "Previous");
                Assign(model, typeof(Price), current, "Current");
                Assign(model, typeof(Price), next, "Next");

                result.Add(model);
            }

            foreach(var item in missing.OrderByDescending(x => x.Value))
            {
                //System.Console.WriteLine($"{item.Key}: {item.Value}");
            }

            return result.ToArray();
        }
        public static ModelStats BuildModel<T>(T[] data, string labelColumn) where T : class
        {
            var split1 = (int)(data.Length * 0.8);
            var split2 = (int)(data.Length * 0.9);
            var mlContext = new MLContext(seed: 0);
            var trainingData = mlContext.Data.LoadFromEnumerable(data.Take(split1));
            var testData = mlContext.Data.LoadFromEnumerable(data.Skip(split1).Take(split2 - split1));

            var ignored = new string[] 
            {
                "OtherIndicatorsPivotHighPoint",
                "OtherIndicatorsPivotLowPoint",
                "OtherIndicatorsPriceRelativeStrengthPercent",
                "OtherIndicatorsPriceRelativeStrengthPrs",
                "OtherIndicatorsPriceRelativeStrengthSma",
                "OtherIndicatorsWilliamsFractalFractalBear",
                "OtherIndicatorsWilliamsFractalFractalBull",
                "TrendAtrTrailingStopBuyStop",
                "TrendAtrTrailingStopSellStop",
                "TrendSuperTrendLower",
                "TrendSuperTrendUpper",
                "VolatilityVolatilityStopLower",
                "VolatilityVolatilityStopUpper"
            };
            var features = typeof(T).GetProperties().Where(x => !ignored.Any(i => x.Name.EndsWith(i)) && (x.Name.StartsWith("Previous") || x.Name.StartsWith("Current"))).Select(x => x.Name).ToArray();


            var estimators = new List<IEstimator<ITransformer>>();
            foreach (var propertyName in features)
            {
                var propertyType = typeof(T).GetProperty(propertyName).PropertyType;
                if(propertyType == typeof(float))
                {
                    estimators.Add(mlContext.Transforms.NormalizeMeanVariance(propertyName));
                }
                else if(propertyType == typeof(bool))
                {
                    estimators.Add(mlContext.Transforms.Categorical.OneHotEncoding(propertyName));
                }
            }

            var pipeline = estimators.First();
            for (var i = 1; i < estimators.Count; i++)
            {
                pipeline = pipeline.Append(estimators[i]);
            }
            pipeline = pipeline.Append(mlContext.Transforms.Concatenate("Features", features));

            //PeekDataViewInConsole(mlContext, trainingData, pipeline, 5);

            //var trainer = mlContext.Regression.Trainers.LightGbm(labelColumnName: labelColumn, featureColumnName: "Features");
            var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: labelColumn, featureColumnName: "Features");
            //var trainer = mlContext.Regression.Trainers.LbfgsPoissonRegression(labelColumnName: labelColumn, featureColumnName: "Features");
            //var trainer = mlContext.Regression.Trainers.OnlineGradientDescent(labelColumnName: labelColumn, featureColumnName: "Features");
            //var trainer = mlContext.Regression.Trainers.FastTree(labelColumnName: labelColumn, featureColumnName: "Features");

            var trainingPipeline = pipeline.Append(trainer);

            var trainedModel = trainingPipeline.Fit(trainingData);

            var predictions = trainedModel.Transform(testData);
            //var preview = predictions.Preview();
            var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: labelColumn, scoreColumnName: "Score");
            PrintRegressionMetrics(trainer.ToString(), metrics);

            
            var predictionEngine = mlContext.Model.CreatePredictionEngine<T, PricePrediction>(trainedModel);
            var labelProperty = typeof(T).GetProperty(labelColumn);
            var max = 0f;
            var sum = 0f;
            var count = 0;
            foreach (var item in data.Skip(split2))
            {
                var prediction = predictionEngine.Predict(item);
                var actual = (float)labelProperty.GetValue(item);
                if(float.IsNaN(prediction.Output) || float.IsNaN(actual))
                {
                    continue;
                }
                var diff = Math.Abs(actual - prediction.Output);
                diff = diff / actual * 100f;
                if(count % 100 == 0)
                {
                    System.Console.WriteLine($"Predicted: {prediction.Output}, actual: {labelProperty.GetValue(item)}, diff: {diff}%");
                }

                if (diff > max)
                {
                    max = diff;
                }

                sum += diff;
                count++;
            }

            System.Console.WriteLine($"Max diff:      {max} %");
            System.Console.WriteLine($"Average diff:  {sum / count} %");

            return new ModelStats
            {
                Trainer = trainer.ToString(),
                Metrics = metrics,
                MaxDiff = max,
                AverageDiff = sum / count
            };

        }


        static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
        {
            System.Console.WriteLine($"LossFn:        {metrics.LossFunction:0.##}");
            System.Console.WriteLine($"R2 Score:      {metrics.RSquared:0.##}");
            System.Console.WriteLine($"Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            System.Console.WriteLine($"Squared loss:  {metrics.MeanSquaredError:#.##}");
            System.Console.WriteLine($"RMS loss:      {metrics.RootMeanSquaredError:#.##}");
        }
        static void PeekDataViewInConsole(MLContext mlContext, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
        {

            //https://github.com/dotnet/machinelearning/blob/main/docs/code/MlNetCookBook.md#how-do-i-look-at-the-intermediate-data
            var transformer = pipeline.Fit(dataView);
            var transformedData = transformer.Transform(dataView);

            // 'transformedData' is a 'promise' of data, lazy-loading. call Preview
            //and iterate through the returned collection from preview.

            var preViewTransformedData = transformedData.Preview(maxRows: numberOfRows);

            foreach (var row in preViewTransformedData.RowView)
            {
                var ColumnCollection = row.Values;
                string lineToPrint = "Row--> ";
                foreach (KeyValuePair<string, object> column in ColumnCollection)
                {
                    lineToPrint += $"| {column.Key}:{column.Value}";
                }
                System.Console.WriteLine(lineToPrint + "\n");
            }
        }
    }
}
