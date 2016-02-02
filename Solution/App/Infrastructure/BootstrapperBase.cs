using System;
using Windows.ApplicationModel.Activation;

namespace App.Infrastructure
{
    public abstract class BootstrapperBase : IDisposable
    {
        public abstract void Dispose();

        public virtual void Run(LaunchActivatedEventArgs e)
        {
            this.Configure();
        }

        protected virtual void Configure()
        {
        }
    }
}