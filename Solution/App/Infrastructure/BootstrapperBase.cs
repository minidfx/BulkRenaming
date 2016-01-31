using Windows.ApplicationModel.Activation;
using App.Infrastructure.Contracts;

namespace App.Infrastructure
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        protected virtual void Configure()
        {
        }

        public virtual void Run(LaunchActivatedEventArgs e)
        {
            this.Configure();
        }
    }
}