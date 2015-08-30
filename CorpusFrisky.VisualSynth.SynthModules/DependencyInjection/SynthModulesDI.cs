using Autofac;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators;
using TriangleGeneratorViewModel = CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators.TriangleGeneratorViewModel;

namespace CorpusFrisky.VisualSynth.SynthModules.DependencyInjection
{
    public class SynthModulesDI : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TriangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<TriangleGeneratorView>().AsSelf();
            builder.RegisterType<ViewModels.ShapeGenerators.TriangleGeneratorViewModel>().AsSelf();

            builder.RegisterType<RectangleGeneratorViewModel>().AsSelf();
            builder.RegisterType<RectangleGeneratorView>().AsSelf();
            //builder.RegisterType<RectangleGeneratorViewModel>().AsSelf();
        }
    }
}
