using System;

using BulkRenaming.Services;
using BulkRenaming.Services.Contracts;
using BulkRenaming.ViewModels;
using BulkRenaming.ViewModels.Contracts;

using Microsoft.Practices.Unity;

namespace BulkRenaming.Infrastructure
{
    /// <summary>
    ///     Bootstrapper for configuring the application boot.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class UWPBootstrapper : CaliburnBootstrapper
    {
        #region Constructors

        public UWPBootstrapper(Action createWindow) : base(createWindow)
        {
        }

        #endregion

        #region All other members

        protected override void Configure()
        {
            base.Configure();

            this.UnityContainer
                .RegisterType<IOpenFolderService, OpenFolderService>()
                .RegisterType<IShellViewModel, ShellViewModel>();
        }

        ~UWPBootstrapper()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // For resources managed
            if (disposing)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}