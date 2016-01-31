using App.Infrastructure.Contracts;
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

            this.UnityContainer.RegisterType<IBootstrapper, UWPBootstrapper>();
        }
    }
}