﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace MLModel1_WebApi2
{
    public partial class MLModel1
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new []{new InputOutputColumnPair(@"vendor_id", @"vendor_id"),new InputOutputColumnPair(@"payment_type", @"payment_type")})      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"rate_code", @"rate_code"),new InputOutputColumnPair(@"passenger_count", @"passenger_count"),new InputOutputColumnPair(@"trip_time_in_secs", @"trip_time_in_secs"),new InputOutputColumnPair(@"trip_distance", @"trip_distance")}))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"vendor_id",@"payment_type",@"rate_code",@"passenger_count",@"trip_time_in_secs",@"trip_distance"}))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.Regression.Trainers.FastTreeTweedie(new FastTreeTweedieTrainer.Options(){NumberOfLeaves=80,MinimumExampleCountPerLeaf=12,NumberOfTrees=32768,MaximumBinCountPerFeature=1024,LearningRate=0.0843354547342146F,FeatureFraction=1F,LabelColumnName=@"fare_amount",FeatureColumnName=@"Features"}));

            return pipeline;
        }
    }
}
