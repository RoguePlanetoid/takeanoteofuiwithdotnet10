namespace DotNetNotes;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    private const string note_settings = "appsettings.notes.json";

    // Extension Block with Extension Methods
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Add Services
        /// </summary>
        /// <param name="root">Configuration Root</param>
        /// <param name="config">Config</param>
        /// <param name="noteProviderType">Note Provider Type</param>
        /// <returns>Service Collection</returns>
        [RequiresUnreferencedCode("Calls Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<TService, TImplementation>()")]
        [RequiresDynamicCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>()")]
        private IServiceCollection AddServices(IConfigurationRoot root, NotesConfig? config = null, Type? noteProviderType = null)
        {
            services.AddSingleton<INotesConfig>(config ?? root.GetSection(nameof(NotesConfig)).Get<NotesConfig>() ?? new())
            .AddTransient<IApplicationProvider, ApplicationProvider>();
            return noteProviderType is not null ? services.AddTransient(typeof(INotesProvider), noteProviderType) : 
            services.AddTransient<INotesProvider, NotesProvider>();
        }

        /// <summary>
        /// Add Services
        /// </summary>
        /// <param name="config">Config</param>
        /// <returns>Service Collection</returns>
        [RequiresUnreferencedCode("Calls Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<TService, TImplementation>()")]
        [RequiresDynamicCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>()")]
        public IServiceCollection AddServices(NotesConfig? config = null) =>
            services.AddServices(
            new ConfigurationBuilder()
            .AddJsonFile(note_settings, true, true)
            .Build(),
            config);

        /// <summary>
        /// Add Services
        /// </summary>
        /// <param name="config">Config</param>
        /// <returns>Service Collection</returns>
        [RequiresUnreferencedCode("Calls Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<TService, TImplementation>()")]
        [RequiresDynamicCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>()")]
        public IServiceCollection AddServices<TNoteProvider>(NotesConfig? config = null) where TNoteProvider : INotesProvider =>
            services.AddServices(
            new ConfigurationBuilder()
            .AddJsonFile(note_settings, true, true)
            .Build(),
            config,
            typeof(TNoteProvider));
    }
}