using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public abstract class SynthModuleBase : BindableBase, ISynthModule
    {
        private ObservableCollection<PinConnection> _connectedModules;

        public SynthModuleBase()
        {
            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
            ConnectedModules = new ObservableCollection<PinConnection>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;
        }

        public virtual void Initialize()
        {
        }

        #region Properties 

        public virtual SynthModuleType ModuleType { get { return SynthModuleType.Unknown; } }
        public ObservableCollection<PinBase> InputPins { get; set; }
        public ObservableCollection<PinBase> OutputPins { get; set; }

        public ObservableCollection<PinConnection> ConnectedModules
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

        public virtual bool ConnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            ConnectedModules.Add(new PinConnection
            {
                InputPin = inputPin,
                OutputPin = outputPin
            });

            return true;
        }

        public virtual void DisconnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            var moduleToRemove = ConnectedModules.First(x => x.InputPin == inputPin && x.OutputPin == outputPin);
            ConnectedModules.Remove(moduleToRemove);
        }

        protected virtual void OnConnectedModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    ToggleConnectedModule(newItem as PinConnection, true);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItems in e.OldItems)
                {
                    ToggleConnectedModule(oldItems as PinConnection, false);
                }
            }
        }

        protected abstract void ToggleConnectedModule(PinConnection pinConnection, bool adding);
    }
}