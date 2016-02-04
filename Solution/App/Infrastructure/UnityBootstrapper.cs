using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace App.Infrastructure
{
    public class UnityBootstrapper : BootstrapperBase
    {
        public UnityBootstrapper()
        {
            this.UnityContainer = new UnityContainer();
        }

        protected UnityContainer UnityContainer { get; }

        protected override void Configure()
        {
            base.Configure();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(this.UnityContainer));
        }

        public override void Dispose()
        {
            this.UnityContainer.Dispose();
        }
    }
}