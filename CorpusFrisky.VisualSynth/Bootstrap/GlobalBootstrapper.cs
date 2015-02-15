using System.Windows;
using Autofac;
using CorpusFrisky.VisualSynth.Controllers;
using CorpusFrisky.VisualSynth.DependencyInjection;
using CorpusFrisky.VisualSynth.Views.Windows;
using Prism.AutofacExtension;

namespace CorpusFrisky.VisualSynth.Bootstrap
{
    public class GlobalBootstrapper : AutofacBootstrapper
    {
        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterModule<ControlsDI>();
            builder.RegisterModule<DisplayDI>();

            builder.RegisterModule<SynthModulesDI>();
                builder.RegisterType<ControlsWindow>().AsSelf().SingleInstance();
            builder.RegisterType<DisplayWindow>().AsSelf().SingleInstance();
            builder.RegisterType<GlobalController>().AsSelf().SingleInstance();
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ControlsWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;

            var globalController = Container.Resolve<GlobalController>();

            globalController.Show();
        }
    }
}
