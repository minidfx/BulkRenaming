using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

using BulkRenaming.Infrastructure;
using BulkRenaming.ViewModels.Contracts;

using Caliburn.Micro;

using Microsoft.Practices.ServiceLocation;

namespace BulkRenaming
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        #region Fields

        private readonly UWPBootstrapper _bootstrapper;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            this._bootstrapper = new UWPBootstrapper(() =>
            {
                this.Initialize();

                var shellViewModel = ServiceLocator.Current.GetInstance<IShellViewModel>();

                var uiElement = ViewLocator.LocateForModel(shellViewModel, null, null);
                ViewModelBinder.Bind(shellViewModel, uiElement, null);
                var activate = shellViewModel as IActivate;

                activate?.Activate();

                Window.Current.Content = uiElement;
                Window.Current.Activate();
            });
        }

        #endregion

        #region All other members

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">
        ///     Details about the launch request and process.
        /// </param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            this._bootstrapper.Run(e);
        }

        #endregion
    }
}