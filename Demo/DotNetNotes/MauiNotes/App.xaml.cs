namespace MauiNotes
{
    public partial class App : Application
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public App() => InitializeComponent();

        /// <summary>
        /// Create Window
        /// </summary>
        /// <param name="activationState">Activation State</param>
        /// <returns>Window</returns>
        protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());
    }
}