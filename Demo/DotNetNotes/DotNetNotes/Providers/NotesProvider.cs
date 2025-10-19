namespace DotNetNotes.Providers;

/// <summary>
/// Notes Provider
/// </summary>
/// <param name="config">Config</param>
internal class NotesProvider(INotesConfig config) : INotesProvider
{
    private const string field_id = "@Id";
    private const string field_title = "@Title";
    private const string field_content = "@Content";
    private const string field_background = "@Background";
    private const string table_create = @"CREATE TABLE IF NOT EXISTS 
    Notes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title NVARCHAR(50) NULL, 
    Content NVARCHAR(255) NULL, Background NVARCHAR(10) NULL);";
    private const string table_destroy = "DROP TABLE IF EXISTS Notes;";
    private const string table_insert = "INSERT INTO Notes VALUES (NULL, @Title, @Content, @Background); SELECT last_insert_rowid();";
    private const string table_update = "UPDATE Notes SET Title = @Title, Content = @Content, Background = @Background WHERE Id = @Id;";
    private const string table_delete = "DELETE FROM Notes WHERE Id = @Id;";
    private const string table_list = "SELECT Id, Title, Content, Background FROM Notes;";
    private const string table_get = "SELECT Id, Title, Content, Background FROM Notes WHERE Id = @Id;";

    /// <summary>
    /// Create Storage
    /// </summary>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> CreateAsync()
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand create = new()
            {
                Connection = connection,
                CommandText = table_create
            };
            await create.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="note">Note</param>
    /// <returns>Id</returns>
    public async Task<int?> AddAsync(NoteModel note)
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand insert = new()
            {
                Connection = connection,
                CommandText = table_insert
            };
            insert.Parameters.AddWithValue(field_title, note.Title);
            insert.Parameters.AddWithValue(field_content, note.Content);
            insert.Parameters.AddWithValue(field_background, note.Background);
            return Convert.ToInt32(await insert.ExecuteScalarAsync());
        }
        catch(Exception)
        {
            return null;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Note</returns>
    public async Task<NoteModel?> GetAsync(int id)
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand get = new()
            {
                Connection = connection,
                CommandText = table_get
            };
            get.Parameters.AddWithValue(field_id, id);
            var query = await get.ExecuteReaderAsync();
            NoteModel? note = null;
            while(await query.ReadAsync())
            {
                note = new()
                {
                    Id = query.GetInt32(0),
                    Title = query.GetString(1),
                    Content = query.GetString(2),
                    Background = query.GetString(3)
                };
            }
            return note;
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// List
    /// </summary>
    /// <returns>IEnumerable of Note</returns>
    public async Task<IEnumerable<NoteModel>> ListAsync()
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand list = new()
            {
                Connection = connection,
                CommandText = table_list
            };
            var query = await list.ExecuteReaderAsync();
            List<NoteModel> results = [];
            while (await query.ReadAsync())
            {
                results.Add(new()
                {
                    Id = query.GetInt32(0),
                    Title = query.GetString(1),
                    Content = query.GetString(2),
                    Background = query.GetString(3)
                });
            }
            return results;
        }
        catch (Exception)
        {
            return [];
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="note">Note</param>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> EditAsync(int id, NoteModel note)
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand update = new()
            {
                Connection = connection,
                CommandText = table_update
            };
            update.Parameters.AddWithValue(field_id, id);
            update.Parameters.AddWithValue(field_title, note.Title);
            update.Parameters.AddWithValue(field_content, note.Content);
            update.Parameters.AddWithValue(field_background, note.Background);
            await update.ExecuteScalarAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand delete = new()
            {
                Connection = connection,
                CommandText = table_delete
            };
            delete.Parameters.AddWithValue(field_id, id);
            await delete.ExecuteScalarAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Destroy Storage
    /// </summary>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> DestroyAsync()
    {
        using SqliteConnection connection = new(config.ConnectionString);
        try
        {
            await connection.OpenAsync();
            SqliteCommand create = new()
            {
                Connection = connection,
                CommandText = table_destroy
            };
            await create.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            connection.Close();
        }
    }
}