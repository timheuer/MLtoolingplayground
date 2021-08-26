using System.Text.Json;
using Microsoft.Extensions.ML;
using MLModel1_WebApi2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput>().FromFile("MLModel1.zip");
builder.Services.AddControllers();
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

app.MapGet("/predict", async (HttpContext http) =>
{
    // Get PredictionEnginePool service
    var predictionEnginePool = http.RequestServices.GetRequiredService<PredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput>>();

    // Deserialize HTTP request JSON body
    var body = http.Request.Body as Stream;
    var input = await JsonSerializer.DeserializeAsync<MLModel1.ModelInput>(body);

    // Predict
    MLModel1.ModelOutput prediction = predictionEnginePool.Predict(input);

    // Return prediction as response
    return Results.Content(prediction.ToString());
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();