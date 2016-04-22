using Autofac;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Generators;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Utility;
using CorpusFrisky.VisualSynth.SynthModules.Views.Generators;
using CorpusFrisky.VisualSynth.SynthModules.Views.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.Views.Utility;
using TriangleGeneratorViewModel = CorpusFrisky.VisualSynth.SynthModules.ViewModels.Generators.TriangleGeneratorViewModel;

namespace CorpusFrisky.VisualSynth.SynthModules.Bootstrapper
{
    public class SynthModulesBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<OutputViewModel>().AsSelf();
            builder.RegisterType<OutputView>().AsSelf();

            builder.RegisterType<TriangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<RectangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<ShapeGeneratorView>().AsSelf();

            builder.RegisterType<SummerViewModel>().AsSelf();
            builder.RegisterType<SummerView>().AsSelf();

            builder.RegisterType<OscillatorViewModel>().AsSelf();
            builder.RegisterType<OscillatorView>().AsSelf();
        }
    }
}
