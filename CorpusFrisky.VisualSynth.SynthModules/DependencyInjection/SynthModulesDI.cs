using Autofac;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using CorpusFrisky.VisualSynth.SynthModules.Views.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators;
using TriangleGeneratorViewModel = CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators.TriangleGeneratorViewModel;

namespace CorpusFrisky.VisualSynth.SynthModules.DependencyInjection
{
    public class SynthModulesDI : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<OutputViewModel>().AsSelf();
            builder.RegisterType<OutputView>().AsSelf();

            builder.RegisterType<TriangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<RectangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<ShapeGeneratorView>().AsSelf();

            builder.RegisterType<OscillatorViewModel>().AsSelf();
            builder.RegisterType<OscillatorView>().AsSelf();
        }
    }
}
