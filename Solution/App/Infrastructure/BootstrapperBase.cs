using System;

using Windows.ApplicationModel.Activation;

namespace App.Infrastructure
{
    public abstract class BootstrapperBase : IDisposable
    {
        #region Interface Implementations

        public abstract void Dispose();

        #endregion

        #region All other members

        public virtual void Run(LaunchActivatedEventArgs e)
        {
            this.Configure();
        }

        protected virtual void Configure()
        {
        }

        #endregion
    }
}