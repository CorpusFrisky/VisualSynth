using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public class SynthModuleBase : BindableBase, ISynthModule
    {
        private ObservableCollection<ConnectedModule> _connectedModules;

        public SynthModuleBase()
        {
            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
            ConnectedModules = new ObservableCollection<ConnectedModule>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;
        }

        public virtual void Initialize()
        {
        }

        #region Properties 

        public virtual SynthModuleType ModuleType { get { return SynthModuleType.Unknown; } }
        public ObservableCollection<PinBase> InputPins { get; set; }
        public ObservableCollection<PinBase> OutputPins { get; set; }

        public ObservableCollection<ConnectedModule> ConnectedModules
        {
            get { return _connectedModules; }
            private set { SetProperty(ref _connectedModules, value); }
        }

        #endregion

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
            ConnectedModules.Add(new ConnectedModule
            {
                Pin = pin,
                Module = module
            });

            return true;
        }

        public virtual void DisconnectSynthModule(PinBase pin, ISynthModule module)
        {
            pin.DisconnectSynthModule(pin, module);
        }

        protected virtual void OnConnectedModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    ToggleConnectedModule(newItem as ConnectedModule, true);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItems in e.OldItems)
                {
                    ToggleConnectedModule(oldItems as ConnectedModule, false);
                }
            }
        }

        protected virtual void ToggleConnectedModule(ConnectedModule connectedModule, bool adding)
        {
            throw new NotImplementedException();
        }
    }
}