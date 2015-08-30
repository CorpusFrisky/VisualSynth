using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class TriangleGeneratorViewModel : BindableBase
    {
        public TriangleGeneratorViewModel(ISynthModule module)
        {
            Module = module;
        }

        public ISynthModule Module { get; set; }


    }
}