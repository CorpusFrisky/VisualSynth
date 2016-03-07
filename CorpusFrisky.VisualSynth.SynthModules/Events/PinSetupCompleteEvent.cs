using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CorpusFrisky.VisualSynth.Events
{
    public class PinSetupCompleteEvent : PubSubEvent<PinSetupCompleteEventArgs>
    {
         
    }

    public class PinSetupCompleteEventArgs
    {
        public ISynthModule SynthModule { get; set; }
    }
}