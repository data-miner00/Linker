namespace Linker.Wpf
{
    using System.Windows;
    using Autofac;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; } = ContainerConfig.Configure();

        public static ILifetimeScope Scope { get; private set; } = Container.BeginLifetimeScope();

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = Scope.Resolve<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Container.Dispose();

            base.OnExit(e);
        }
    }
}
