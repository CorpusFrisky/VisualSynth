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
            Pins = new ObservableCollection<Pin>();
            ConnectedModules = new ObservableCollection<ConnectedModule>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;

            ConstructionValidated = false;
        }

        
        #region Properties

        public IEventAggregator EventAggregator { get; private set; }

        public ObservableCollection<VertexModel> Vertices
        {
            get { return _vertices; }
            set { SetProperty(ref _vertices, value); }
        }

        public ObservableCollection<Pin> Pins { get; set; }

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
            var pinIndex = 0;
            var vertexNumber = 1;
            foreach (var vertex in Vertices)
            {
                Pins.Add(new Pin {PinIndex = pinIndex++, Label = "V" + vertexNumber + " Color", TargetObject = vertex, TargetType = PinTargetTypeEnum.Vertex, TargetProperty = PinTagetPropertyEnum.Color });
                Pins.Add(new Pin { PinIndex = pinIndex++, Label = "V" + vertexNumber + " Position", TargetObject = vertex, TargetType = PinTargetTypeEnum.Vertex, TargetProperty = PinTagetPropertyEnum.Position });
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

        public bool ConnectSynthModule(Pin pin, ISynthModule module)
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

        public bool DisconnectSynthModule(Pin pin, ISynthModule module)
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
                    AddConnectedModule(newItem as ConnectedModule);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItems in e.OldItems)
                {
                    RemoveConnectedModule(oldItems as ConnectedModule);
                }
            }
        }

        protected virtual void AddConnectedModule(ConnectedModule connectedModule)
        {
            var modifierModule = connectedModule.Module as IModifierModule;
            if (modifierModule == null)
            {
                // TODO: log a message
                return;
            }

            var pin = connectedModule.Pin;

            switch (pin.TargetType)
            {
                case PinTargetTypeEnum.Vertex:
                    var vertex = pin.TargetObject as VertexModel;
                    if (vertex == null)
                    {
                        //TODO: Log
                        return;
                    }
                    vertex.AddPropertyModifier(pin.TargetProperty, modifierModule);
                    break;
            }
        }

        protected virtual void RemoveConnectedModule(ConnectedModule connectedModule)
        {
            var modifierModule = connectedModule.Module as IModifierModule;
            if (modifierModule == null)
            {
                // TODO: log a message
                return;
            }

            var pin = connectedModule.Pin;

            switch (pin.TargetType)
            {
                case PinTargetTypeEnum.Vertex:
                    var vertex = pin.TargetObject as VertexModel;
                    if (vertex == null)
                    {
                        //TODO: Log
                        return;
                    }
                    vertex.RemovePropertyModifier(pin.TargetProperty, modifierModule);
                    break;
            }
        }

        #endregion

    }
}