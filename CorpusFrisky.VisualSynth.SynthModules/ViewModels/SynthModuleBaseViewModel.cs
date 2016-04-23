using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public abstract class SynthModuleBaseViewModel : BindableBase, ISynthModule
    {
        private ObservableCollection<PinConnection> _connectedModules;

        public SynthModuleBaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
            ConnectedModules = new ObservableCollection<PinConnection>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;
        }

        public virtual void Initialize()
        {
        }

        #region Properties 

        protected IEventAggregator EventAggregator { get; private set; }

        public virtual SynthModuleType ModuleType { get { return SynthModuleType.Unknown; } }
        public ObservableCollection<PinBase> InputPins { get; set; }
        public ObservableCollection<PinBase> OutputPins { get; set; }

        public ObservableCollection<PinConnection> ConnectedModules
        {
            get { return _connectedModules; }
            private set { SetProperty(ref _connectedModules, value); }
        }

        #endregion

        protected abstract void SetupPins();

        public abstract void PreRender();

        public abstract void Render(bool fromFinalRenderCall = false);

        public abstract void PostRender();

        public void RenderInputs()
        {
            foreach (var connectedPin in InputPins.SelectMany(x => x.ConnectedPins))
            {
                connectedPin.Module.Render();
            }
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