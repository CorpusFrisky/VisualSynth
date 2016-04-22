using System.Windows;
using Autofac;
using CorpusFrisky.VisualSynth.Controllers;
using CorpusFrisky.VisualSynth.Bootstrapper;
using CorpusFrisky.VisualSynth.SynthModules.Bootstrapper;
using CorpusFrisky.VisualSynth.Views.Windows;
using Prism.AutofacExtension;

namespace CorpusFrisky.VisualSynth.Bootstrap
{
    public class GlobalBootstrapper : AutofacBootstrapper
    {
        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterModule<ControlsBootstrapper>();
            builder.RegisterModule<DisplayBootstrapper>();
            builder.RegisterModule<SynthModulesBootstrapper>();

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
