using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class RectangleGeneratorViewModel : BindableBase
    {
        public RectangleGeneratorViewModel(ISynthModule module)
        {
            Module = module;
        }

        public ISynthModule Module { get; set; }
    }
}