using Windows.ApplicationModel.Activation;

namespace App.Infrastructure.Contracts
{
    public interface IBootstrapper
    {
        void Run(LaunchActivatedEventArgs e);
    }
}