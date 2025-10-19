var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddOpenApi(options => options.AddSchemaTransformer<AllowedBackgroundSchemaTransformer>())
    .AddValidation()
    .RegisterServices();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapEndpoints();
app.MapOpenApi();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/openapi/v1.json", string.Empty);
    options.RoutePrefix = string.Empty;
});
app.Run();