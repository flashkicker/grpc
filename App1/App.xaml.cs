using App1.Services;
using Microsoft.UI.Xaml;
using GRPC1;
using GrpcDotNetNamedPipes;
using System;
using System.Reflection;
using COS.SDK;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            StartGrpcServer();
            m_window = new MainWindow();
            m_window.Activate();
        }

        private void StartGrpcServer()
        {
            var server = new NamedPipeServer(ServiceHelper.GetServiceName(typeof(App1Service)));
            GRPC1Service.BindService(server.ServiceBinder, new App1Service());
            server.Start();
        }

        private Window m_window;
    }
}
