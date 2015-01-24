using Autofac;
using CorpusFrisky.VisualSynth.Controllers;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;

namespace CorpusFrisky.VisualSynth.Modules
{
    class DisplayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DisplayWindowController>().As<IDisplayWindowController>();
        }
    }
}
