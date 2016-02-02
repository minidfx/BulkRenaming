using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Caliburn.Micro;
using Microsoft.Practices.Unity;
using Action = System.Action;

namespace App.Infrastructure
{
    public class CaliburnBootstrapper : UnityBootstrapper
    {
        private readonly Action _createWindow;

        public CaliburnBootstrapper(Action createWindow)
        {
            this._createWindow = createWindow;
        }

        protected override void Configure()
        {
            base.Configure();

            this.UnityContainer.RegisterType<IEventAggregator, EventAggregator>()
                .RegisterType<INavigationService, FrameAdapter>();

            IoC.BuildUp += this.BuildUp;
            IoC.GetInstance += this.GetInstance;
            IoC.GetAllInstances += this.GetAllInstances;
        }

        public override void Run(LaunchActivatedEventArgs e)
        {
            base.Run(e);

            if (e.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                return;
            }

            this._createWindow();
        }

        private IEnumerable<object> GetAllInstances(Type type)
        {
            return this.UnityContainer.ResolveAll(type);
        }

        private object GetInstance(Type type, string s)
        {
            return this.UnityContainer.Resolve(type, s);
        }

        private void BuildUp(object o)
        {
            this.UnityContainer.BuildUp(o.GetType(), o);
        }
    }
}