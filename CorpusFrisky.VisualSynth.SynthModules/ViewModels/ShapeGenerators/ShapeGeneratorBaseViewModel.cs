﻿using System;
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
        private Vector3 _center;


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


        public Vector3 Center
        {
            get { return _center; }
            set { SetProperty(ref _center, value); }
        }
        
        public float X
        {
            get { return Center.X; }
            set
            {
                _center.X = value;
                OnPropertyChanged("Center");
            }
        }

        public float Y
        {
            get { return Center.Y; }
            set
            {
                _center.Y = value;
                OnPropertyChanged("Center");
            }
        }

        public float Z
        {
            get { return Center.Z; }
            set
            {
                _center.Z = value;
                OnPropertyChanged("Center");
            }
        }

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

        public override bool ConnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            if (!base.ConnectSynthModule(inputPin, outputPin))
            {
                return false;
            }
            //if(!IsAbleToAttach())
            //{
            //    return false;
            //}

            

            return true;
        }

        public override void DisconnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            base.DisconnectSynthModule(inputPin, outputPin);
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

        

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            var pin = pinConnection.InputPin;

            if (pin.IsInput)
            {
                if (pin.PinType == PinTypeEnum.Value)
                {
                    ToggleInputValueModule(pinConnection, adding);
                   
                }
                else if (pin.PinType == PinTypeEnum.Frame)
                {
                    
                }
            }
        }

        private void ToggleInputValueModule(PinConnection pinConnection, bool adding)
        {
            var inputPin = pinConnection.InputPin as InputValuePin;
            var outputPin = pinConnection.OutputPin as OutputValuePin;

            if (inputPin == null)
            {
                // TODO: log a message
                return;
            }

            switch (inputPin.TargetType)
            {
                case PinTargetTypeEnum.Vertex:
                    var vertex = inputPin.TargetObject as VertexModel;
                    if (vertex == null)
                    {
                        //TODO: Log
                        return;
                    }

                    if (adding)
                    {
                        vertex.AddPropertyModifier(inputPin.TargetProperty, outputPin);
                    }
                    else
                    {
                        vertex.RemovePropertyModifier(inputPin.TargetProperty, outputPin);
                    }
                    break;
            }
        }

        #endregion

    }
}