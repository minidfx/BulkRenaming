using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace App.Infrastructure
{
    public class UnityBootstrapper : BootstrapperBase
    {
        #region Constructors

        public UnityBootstrapper()
        {
            this.UnityContainer = new UnityContainer();
        }

        #endregion

        #region Properties, Indexers

        protected UnityContainer UnityContainer { get; }

        #endregion

        #region All other members

        protected override void Configure()
        {
            base.Configure();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(this.UnityContainer));
        }

        public override void Dispose()
        {
            this.UnityContainer.Dispose();
        }

        #endregion
    }
}