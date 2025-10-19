namespace AspNetCoreWebApiNotes.Endpoints;

/// <summary>
/// Notes Endpoints
/// </summary>
public static class NotesEndpoints
{
    /// <summary>
    /// Map Notes Endpoints
    /// </summary>
    /// <param name="endpoints">Endpoint Route Builder</param>
    /// <returns>Endpoint Route Builder</returns>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // List Backgrounds
        endpoints.MapGet("/backgrounds", () =>
            Results.Ok(BackgroundModel.Options.Select(background => new { background.Name, background.Background })))
        .WithName("ListBackgrounds");
        // Notes Group
        var group = endpoints.MapGroup("/notes");
        // List Note
        group.MapGet("/", async (IApplicationProvider appProvider) =>
        {
            await appProvider.ListAsync();
            return Results.Ok(appProvider.Content.Notes.Select(note => new NoteResponse(note)));
        })
        .WithName("ListNotes")
        .Produces<IEnumerable<NoteResponse>>(StatusCodes.Status200OK);
        // Get Note by Id
        group.MapGet("/{id:int}", async (int id, IApplicationProvider appProvider) =>
        {
            var note = await appProvider.GetAsync(id);
            return note is null ? Results.NotFound() : Results.Ok(new NoteResponse(note));
        })
        .WithName("GetNote")
        .Produces<NoteResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        // New Note
        group.MapPost("/", async (NoteRequest request, IApplicationProvider appProvider) =>
        {
            if (request == null)
                return Results.BadRequest("Create Note Request is required.");
            var note = request.ToNoteModel();
            var ok = await appProvider.NewAsync(note, true);
            if (!ok || note.Id is null)
                return Results.BadRequest("Unable to create note.");
            return Results.Created($"/notes/{note.Id}", new NoteResponse(note));
        })
        .WithName("CreateNote")
        .Accepts<NoteRequest>("application/json")
        .Produces<NoteResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
        // Edit Note
        group.MapPut("/{id:int}", async (int id, NoteRequest request, IApplicationProvider appProvider) =>
        {
            if (request == null)
                return Results.BadRequest("Update Note Request is required.");
            var existing = await appProvider.GetAsync(id);
            if (existing is null)
                return Results.NotFound();
            var note = request.ToNoteModel();
            note.Id = id;
            var ok = await appProvider.EditAsync(note, true);
            return ok ? Results.NoContent() : Results.BadRequest("Unable to update note.");
        })
        .WithName("UpdateNote")
        .Accepts<NoteRequest>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
        // Delete Note
        group.MapDelete("/{id:int}", async (int id, IApplicationProvider appProvider) =>
        {
            var existing = await appProvider.GetAsync(id);
            if (existing is null)
                return Results.NotFound();
            var ok = await appProvider.DeleteAsync(existing, true);
            return ok ? Results.NoContent() : Results.BadRequest("Unable to delete note.");
        })
        .WithName("DeleteNote")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
        return endpoints;
    }
}