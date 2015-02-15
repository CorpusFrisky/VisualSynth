using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class TriangleGeneratorViewModel : BindableBase
    {
        static private int test = 0;

        private string _testString = "";

        public TriangleGeneratorViewModel(TriangleGenerator module)
        {
            Module = module;
            _testString = "Triangle " + test++;
        }

        public TriangleGenerator Module { get; set; }

        public string TestString { get { return _testString; } }
    }
}