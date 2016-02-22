using System.Diagnostics.CodeAnalysis;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace BulkRenaming.Infrastructure
{
    /// <summary>
    ///     Bootstrapper for configuring Unity.
    /// </summary>
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

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<UnityContainer>k__BackingField", Justification = "Dispose method is called.")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UnityContainer.Dispose();
            }
        }

        #endregion
    }
}