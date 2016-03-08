using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class ShapeGeneratorBaseViewModel : BindableBase, ISynthModule
    {
        protected bool ConstructionValidated;
        private ObservableCollection<VertexModel> _vertices;

        private ObservableCollection<ConnectedModule> _connectedModules;


        public ShapeGeneratorBaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            Center = new Vector3(0);
            Vertices = new ObservableCollection<VertexModel>();
            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
            ConnectedModules = new ObservableCollection<ConnectedModule>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;

            ConstructionValidated = false;
        }

        public void Initialize()
        {
            if (!ConstructionValidated)
            {
                ValidateConstruction(NumVertices);
                ConstructionValidated = true;
            }
        }
        
        #region Properties

        public IEventAggregator EventAggregator { get; private set; }

        public ObservableCollection<VertexModel> Vertices
        {
            get { return _vertices; }
            set { SetProperty(ref _vertices, value); }
        }

        public ObservableCollection<PinBase> InputPins { get; set; }
        public ObservableCollection<PinBase> OutputPins { get; set; }

        public ObservableCollection<ConnectedModule> ConnectedModules
        {
            get { return _connectedModules; } 
            private set { SetProperty(ref _connectedModules, value); }
        }

        public Vector3 Center { get; set; }

        public virtual SynthModuleType ModuleType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int NumVertices
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Event Handlers

        #endregion



        #region ISynthModule Implementation

        public virtual void SetupPins()
        {
            var inputPinIndex = 0;
            var vertexNumber = 1;
            foreach (var vertex in Vertices)
            {
                InputPins.Add(new InputValuePin
                {
                    Module = this,
                    PinIndex = inputPinIndex,
                    Label = "V" + vertexNumber + " Color",
                    PinType = PinTypeEnum.Value,
                    TargetObject = vertex,
                    TargetType = PinTargetTypeEnum.Vertex,
                    TargetProperty = PinTagetPropertyEnum.Color
                });

                inputPinIndex++;

                InputPins.Add(new InputValuePin
                {
                    Module = this,
                    PinIndex = inputPinIndex,
                    Label = "V" + vertexNumber + " Position",
                    PinType = PinTypeEnum.Value,
                    TargetObject = vertex,
                    TargetType = PinTargetTypeEnum.Vertex,
                    TargetProperty = PinTagetPropertyEnum.Position
                });

                inputPinIndex++;
                vertexNumber++;
            }

            EventAggregator.GetEvent<PinSetupCompleteEvent>().Publish(new PinSetupCompleteEventArgs
            {
                SynthModule = this
            });
        }


        public virtual void PreRender()
        {
            throw new NotImplementedException();
        }

        public virtual void Render()
        {
            throw new NotImplementedException();
        }

        public virtual void PostRender()
        {
            throw new NotImplementedException();
        }

        public bool ConnectSynthModule(PinBase pin, ISynthModule module)
        {
            //if(!IsAbleToAttach())
            //{
            //    return false;
            //}

            ConnectedModules.Add(new ConnectedModule
            {
                Pin = pin,
                Module = module
            });

            return true;
        }

        public bool DisconnectSynthModule(PinBase pin, ISynthModule module)
        {
            var moduleToDisconnect = ConnectedModules.First(x => x.Module == module && x.Pin == pin);
            if (moduleToDisconnect != null)
            {
                ConnectedModules.Remove(moduleToDisconnect);
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        protected void ValidateConstruction(int numVertices)
        {
            while (Vertices.Count < numVertices)
            {
                Vertices.Add(new VertexModel()
                {
                    Color = new Color4(),
                    Position = new Vector3()
                });
            }

            SetupPins();
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
            var pin = connectedModule.Pin;

            if (pin.IsInput)
            {
                if (pin.PinType == PinTypeEnum.Value)
                {
                    ToggleInputValueModule(connectedModule, adding);
                   
                }
                else if (pin.PinType == PinTypeEnum.Frame)
                {
                    
                }
            }
        }

        private void ToggleInputValueModule(ConnectedModule connectedModule, bool adding)
        {
            var module = connectedModule.Module as IModifierModule;
            var pin = connectedModule.Pin as InputValuePin;

            if (module == null || pin == null)
            {
                // TODO: log a message
                return;
            }

            switch (pin.TargetType)
            {
                case PinTargetTypeEnum.Vertex:
                    var vertex = pin.TargetObject as VertexModel;
                    if (vertex == null)
                    {
                        //TODO: Log
                        return;
                    }

                    if (adding)
                    {
                        vertex.AddPropertyModifier(pin.TargetProperty, module);
                    }
                    else
                    {
                        vertex.RemovePropertyModifier(pin.TargetProperty, module);

                    }
                    break;
            }
        }

        #endregion

    }
}