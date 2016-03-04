using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CorpusFrisky.VisualSynth.Events
{
    public class ModuleAddedOrClickedEvent : PubSubEvent<ModuleAddedOrClickedEventArgs>
    {
         
    }

    public class ModuleAddedOrClickedEventArgs
    {
        public ISynthModule Module { get; set; }
    }
}