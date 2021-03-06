﻿using System;

using Windows.ApplicationModel.Activation;

namespace BulkRenaming.Infrastructure
{
    /// <summary>
    ///     The base bootstrapper for loading application dependencies.
    /// </summary>
    public abstract class BootstrapperBase : IDisposable
    {
        #region Interface Implementations

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region All other members

        protected abstract void Dispose(bool disposing);

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