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
    public class ShapeGeneratorBaseViewModel : SynthModuleBase
    {
        protected bool ConstructionValidated;
        private ObservableCollection<VertexModel> _vertices;


        public ShapeGeneratorBaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            Center = new Vector3(0);
            Vertices = new ObservableCollection<VertexModel>();
            
            

            ConstructionValidated = false;
        }

        public override void Initialize()
        {
            base.Initialize();

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


        public Vector3 Center { get; set; }

        public override SynthModuleType ModuleType
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

        protected override void SetupPins()
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


        public override void PreRender()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void PostRender()
        {
            throw new NotImplementedException();
        }

        public override bool ConnectSynthModule(PinBase pin, ISynthModule module)
        {
            if (!base.ConnectSynthModule(pin, module))
            {
                return false;
            }
            //if(!IsAbleToAttach())
            //{
            //    return false;
            //}

            

            return true;
        }

        public override void DisconnectSynthModule(PinBase pin, ISynthModule module)
        {
            base.DisconnectSynthModule(pin, module);

            var moduleToDisconnect = ConnectedModules.First(x => x.Module == module && x.Pin == pin);
            if (moduleToDisconnect != null)
            {
                ConnectedModules.Remove(moduleToDisconnect);
            }
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

        

        protected override void ToggleConnectedModule(ConnectedModule connectedModule, bool adding)
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