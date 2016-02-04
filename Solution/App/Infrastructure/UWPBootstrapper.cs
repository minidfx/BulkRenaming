using System;
using App.Services;
using App.Services.Contracts;
using App.ViewModels;
using App.ViewModels.Contracts;
using Microsoft.Practices.Unity;

namespace App.Infrastructure
{
    // ReSharper disable once InconsistentNaming
    public sealed class UWPBootstrapper : CaliburnBootstrapper
    {
        public UWPBootstrapper(Action createWindow) : base(createWindow)
        {
        }

        protected override void Configure()
        {
            base.Configure();

            this.UnityContainer.RegisterType<IOpenFolderService, OpenFolderService>();
            this.UnityContainer.RegisterType<IShellViewModel, ShellViewModel>();
        }

        public override void Dispose()
        {
            base.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}