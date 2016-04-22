using Autofac;
using CorpusFrisky.VisualSynth.Controllers;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;
using CorpusFrisky.VisualSynth.ViewModels;
using CorpusFrisky.VisualSynth.Views.ControlViews;

namespace CorpusFrisky.VisualSynth.Bootstrapper
{
    class ControlsBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ControlsWindowController>().As<IControlsWindowController>();
            builder.RegisterType<DesignView>().AsSelf().SingleInstance();
            builder.RegisterType<DesignViewModel>().AsSelf().SingleInstance();
        }
    }
}
