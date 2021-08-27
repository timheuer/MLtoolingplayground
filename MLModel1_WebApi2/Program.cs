using Microsoft.Extensions.ML;
using MLModel1_WebApi2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput>().FromFile("MLModel1.zip");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MLModel1", Version = "v1" });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MLModel1 v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapPost("/predict", async (MLModel1.ModelInput modelInput, PredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput> predictionEnginePool) =>
{
    // Predict
    MLModel1.ModelOutput prediction = predictionEnginePool.Predict(modelInput);

    // Return prediction as response
    return Results.Json(prediction);
});

app.Run();
