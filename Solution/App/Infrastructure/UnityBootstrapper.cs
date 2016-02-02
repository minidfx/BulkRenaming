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

        public override void Dispose()
        {
            this.UnityContainer.Dispose();
        }
    }
}