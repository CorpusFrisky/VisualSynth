using System.Collections.ObjectModel;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public class SynthModuleBase : BindableBase, ISynthModule
    {
        public virtual void Initialize()
        {
        }

        protected virtual void SetupPins()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PreRender()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Render()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PostRender()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool ConnectSynthModule(PinBase pin, ISynthModule module)
        {
            return true;
        }

        public virtual bool DisconnectSynthModule(PinBase pin, ISynthModule module)
        {
            throw new System.NotImplementedException();
        }

        public virtual SynthModuleType ModuleType { get { return SynthModuleType.Unknown; } }
        public ObservableCollection<PinBase> InputPins { get; set; }
        public ObservableCollection<PinBase> OutputPins { get; set; }
    }
}