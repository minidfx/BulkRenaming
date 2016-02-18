using System;

using Windows.ApplicationModel.Activation;

namespace BulkRenaming.Infrastructure
{
    public abstract class BootstrapperBase : IDisposable
    {
        #region Interface Implementations

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion

        #region All other members

        public abstract void Dispose(bool disposing);

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