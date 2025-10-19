namespace AspNetCoreWebApiNotes.Transformers;

/// <summary>
/// Allowed Background Schema Transformer
/// </summary>
public class AllowedBackgroundSchemaTransformer : IOpenApiSchemaTransformer
{
    /// <summary>
    /// Transform
    /// </summary>
    /// <param name="schema">OpenAPI Schema</param>
    /// <param name="context">OpenAPI Schema Transformer Context</param>
    /// <param name="cancellationToken">Cancellation Token</param>    
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var isBackground = context.JsonPropertyInfo?.Name.Equals(nameof(NoteRequest.Background), 
            StringComparison.InvariantCultureIgnoreCase);
        if (isBackground == true)
        {
            schema.Type = JsonSchemaType.String;                       
            schema.Enum = [.. BackgroundModel.Names.Select(name => JsonValue.Create(name))];
            schema.Description = $"Allowed values: {BackgroundModel.NamesOutput}.";
            schema.Example = JsonValue.Create("Yellow");
        }
        return Task.CompletedTask;
    }
}
