using System;

using BulkRenaming.Services;
using BulkRenaming.Services.Contracts;
using BulkRenaming.ViewModels;
using BulkRenaming.ViewModels.Contracts;

using Microsoft.Practices.Unity;

namespace BulkRenaming.Infrastructure
{
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

        public override void Dispose()
        {
            base.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}