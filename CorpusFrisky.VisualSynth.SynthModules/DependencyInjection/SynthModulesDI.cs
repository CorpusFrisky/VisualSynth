using Autofac;
using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators;

namespace CorpusFrisky.VisualSynth.SynthModules.DependencyInjection
{
    public class SynthModulesDI : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TriangleGenerator>().AsSelf();
            builder.RegisterType<TriangleGeneratorView>().AsSelf().SingleInstance();
            builder.RegisterType<TriangleGeneratorViewModel>().AsSelf();
        }
    }
}
