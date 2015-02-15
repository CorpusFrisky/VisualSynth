using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CorpusFrisky.VisualSynth.Events
{
    public class ModuleAddedEvent : PubSubEvent<ModuleAddedEventArgs>
    {
         
    }

    public class ModuleAddedEventArgs
    {
        public TriangleGenerator Module { get; set; }
    }
}