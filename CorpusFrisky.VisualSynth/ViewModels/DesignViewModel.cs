using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace CorpusFrisky.VisualSynth.ViewModels
{
    public class DesignViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand _addTriangleCommand;
        private DelegateCommand _addRectangleCommand;
        private DelegateCommand _addOscillatorCommand;
        private DelegateCommand<SynthComponentModel> _handleModuleLeftClick;
        private DelegateCommand<PinBase> _pinLeftClickedCommand;
        private DelegateCommand<PinBase> _pinRighttClickedCommand;
        private DelegateCommand _handleLeftClickCommand;
        private DelegateCommand _handleRightClickCommand;

        private PinBase _activelyConnectingPin;
        private Point _currentMousePos;
        private PinBase _activelyDisconnectingPin;
        private ConnectionWire _activelyDisconnectingWire;
        private List<ConnectionWire> _connectionsFromActivelyDisconnectingPin;
        private int _activelyDisconnectingWireIndex;
        private bool _shouldShowContextMenu;

        #endregion


        public DesignViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SynthComponents = new ObservableCollection<SynthComponentModel>();
            ConnectionWires = new ObservableCollection<ConnectionWire>();
            ActivelyConnectingPin = null;

            _connectionsFromActivelyDisconnectingPin = new List<ConnectionWire>();

            SubscribeToEvents();

            SetupOutputModule();
           
        }

        private void SetupOutputModule()
        {
            //TODO: put control window dimensions in design constants
            //TODO: handle window resizing
            var pos = new Point(350, 450);
            var outputViewModel = new OutputViewModel(_eventAggregator);

            AddAndInitializeModule(outputViewModel, pos);
        }

        private void SubscribeToEvents()
        {
            _eventAggregator.GetEvent<PinSetupCompleteEvent>().Subscribe(OnPinCompleteEvent);
        }


        #region Properties

        public ObservableCollection<SynthComponentModel> SynthComponents { get; set; }

        public ObservableCollection<ConnectionWire> ConnectionWires { get; set; }

        public PinBase ActivelyConnectingPin
        {
            get { return _activelyConnectingPin; }
            set
            {
                SetProperty(ref _activelyConnectingPin, value);
                OnPropertyChanged("ShouldShowActivelyConnectingLine");
                OnPropertyChanged("ActivelyConnectingPinPos");
            }
        }

        public PinBase ActivelyDisconnectingPin
        {
            get { return _activelyDisconnectingPin; }
            set
            {
                SetProperty(ref _activelyDisconnectingPin, value);
            }
        }

        public ConnectionWire ActivelyDisconnectingWire
        {
            get { return _activelyDisconnectingWire; }
            set
            {
                if (_activelyDisconnectingWire != null)
                {
                    _activelyDisconnectingWire.IsDeletionTarget = false;
                }

                SetProperty(ref _activelyDisconnectingWire, value);

                if (_activelyDisconnectingWire != null)
                {
                    _activelyDisconnectingWire.IsDeletionTarget = true;
                }
            }
        }

        public Point CurrentDesignPos { get; set; }

        public bool ShouldShowActivelyConnectingLine
        {
            get { return ActivelyConnectingPin != null; }
        }

        public Point ActivelyConnectingPinPos
        {
            get
            {
                if (ActivelyConnectingPin == null)
                {
                    return Point.Empty;
                }

                return GetPinCenterPos(ActivelyConnectingPin);
            }
        }

        public Point CurrentMousePos
        {
            get { return _currentMousePos; }
            set { SetProperty(ref _currentMousePos, value); }
        }

        #endregion


        #region Commands

        public DelegateCommand AddTriangleCommand
        {
            get { return _addTriangleCommand ?? (_addTriangleCommand = new DelegateCommand(AddTriangle)); }
        }

        public DelegateCommand AddRectangleCommand
        {
            get { return _addRectangleCommand ?? (_addRectangleCommand = new DelegateCommand(AddRectangle)); }
        }

        public DelegateCommand AddOscillatorCommand
        {
            get { return _addOscillatorCommand ?? (_addOscillatorCommand = new DelegateCommand(AddOscillator)); }
        }

        public DelegateCommand<SynthComponentModel> HandleModuleLeftClickCommand
        {
            get { return _handleModuleLeftClick ?? (_handleModuleLeftClick = new DelegateCommand<SynthComponentModel>(HandleModuleLeftClick)); }
        }

        public DelegateCommand HandleLeftClickCommand
        {
            get { return _handleLeftClickCommand ?? (_handleLeftClickCommand = new DelegateCommand(HandleLeftClick)); }
        }

        //public DelegateCommand HandleRightClickCommand
        //{
        //    get { return _handleRightClickCommand ?? (_handleRightClickCommand = new DelegateCommand(HandleRightClick)); }
        //}

        public DelegateCommand<PinBase> PinLeftClickedCommand
        {
            get { return _pinLeftClickedCommand ?? (_pinLeftClickedCommand = new DelegateCommand<PinBase>(HandlePinLeftClick)); }
        }

        public DelegateCommand<PinBase> PinRightClickedCommand
        {
            get { return _pinRighttClickedCommand ?? (_pinRighttClickedCommand = new DelegateCommand<PinBase>(HandlePinRightClick)); }
        }

        #endregion


        #region Command Handlers

        private void AddTriangle()
        {
            var rand = new Random();
            var triangle = new TriangleGeneratorViewModel(_eventAggregator)
                           {
                               Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                           };

            AddAndInitializeModule(triangle);
        }

        private void AddRectangle()
        {
            var rand = new Random();
            var rectangle = new RectangleGeneratorViewModel(_eventAggregator)
                            {
                                Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                            };

            AddAndInitializeModule(rectangle);
        }

        private void AddOscillator()
        {
            var oscillator = new OscillatorViewModel(_eventAggregator)
            {
                Rate = 1.0,
            };

            AddAndInitializeModule(oscillator);
        }

        private void HandleModuleLeftClick(SynthComponentModel componentModel)
        {
            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
                                                                  {
                                                                      Module = componentModel.Module
                                                                  });
        }

        private void HandleLeftClick()
        {
            ActivelyConnectingPin = null;

            if (ActivelyDisconnectingWire != null)
            {
                ConnectionWires.Remove(ActivelyDisconnectingWire);
                ActivelyDisconnectingWire.InputConnection.DisconnectSynthModule(ActivelyDisconnectingWire.OutputConnection);
                ActivelyConnectingPin = ActivelyDisconnectingPin;
                DeactivateDisconnection();
            }
        }

        private void HandlePinLeftClick(PinBase pin)
        {
            if (ActivelyDisconnectingPin != null)
            {
                DeactivateDisconnection();
            }

            if (ActivelyConnectingPin == null)
            {
                ActivelyConnectingPin = pin;
            }
            else
            {
                if (ActivelyConnectingPin.IsInput)
                {
                    var inputPin = ActivelyConnectingPin as InputPin;
                    var outputPin = pin as OutputPin;

                    if (inputPin == null ||
                        outputPin == null || 
                        pin.IsInput)
                    {
                        return;
                    }

                    inputPin.ConnectSynthModule(outputPin);
                }
                else
                {
                    var inputPin = pin as InputPin;
                    var outputPin = ActivelyConnectingPin as OutputPin;

                    if (inputPin == null ||
                        outputPin == null ||
                        !pin.IsInput)
                    {
                        return;
                    }

                    inputPin.ConnectSynthModule(outputPin);
                }

                ConnectionWires.Add(new ConnectionWire
                {
                    //TODO: Do we still need IsInput now that I've create InputePint and OutputPin??? (Yes, InputePint)
                    OutputConnection = (ActivelyConnectingPin.IsInput ? pin : ActivelyConnectingPin) as OutputPin,
                    InputConnection = (ActivelyConnectingPin.IsInput ? ActivelyConnectingPin : pin) as InputPin,
                    Pin1Pos = GetPinCenterPos(ActivelyConnectingPin),
                    Pin2Pos = GetPinCenterPos(pin)
                });

                ActivelyConnectingPin = null;
            }
        }

        private void HandlePinRightClick(PinBase pin)
        {
            if (pin.ConnectedPins.Count == 0)
            {
                ActivelyDisconnectingPin = null;
                return;
            }

            if (pin != ActivelyDisconnectingPin)
            {
                ActivelyDisconnectingPin = pin;
                _connectionsFromActivelyDisconnectingPin =
                    ConnectionWires.Where(x => x.InputConnection == pin || x.OutputConnection == pin).ToList();
                _activelyDisconnectingWireIndex = 0;
            }
            else
            {
                _activelyDisconnectingWireIndex = (_activelyDisconnectingWireIndex + 1) %
                                                  _connectionsFromActivelyDisconnectingPin.Count;
            }

            ActivelyDisconnectingWire = _connectionsFromActivelyDisconnectingPin[_activelyDisconnectingWireIndex];
        }

        #endregion

        #region Event Handlers

        private void OnPinCompleteEvent(PinSetupCompleteEventArgs args)
        {
            var component = SynthComponents.FirstOrDefault(x => x.Module == args.SynthModule);
            if (component == null)
            {
                return;
            }

            component.UpdateDimensions();
        }

        #endregion

        #region Helper Methods

        void AddAndInitializeModule(ISynthModule module, Point? position = null)
        {
            SynthComponents.Add(new SynthComponentModel()
            {
                DesignPos = position ?? CurrentDesignPos,
                Module = module
            });

            //TODO: C'mon, man
            //Init has to be called afterwards...can't really remember why.
            module.Initialize();

            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
            {
                Module = module
            });
        }

        Point GetPinCenterPos(PinBase pin)
        {
            var component = SynthComponents.FirstOrDefault(x => x.Module == pin.Module);
            if (component == null)
            {
                return Point.Empty;
            }

            var pinPos = Point.Add(component.DesignPos, new Size(pin.PinDesignPos));
            //offset to pin center
            return Point.Add(pinPos, new Size(DesignConstants.PinWidth / 2, DesignConstants.PinHeight / 2));
        }

        private void DeactivateDisconnection()
        {
            ActivelyDisconnectingPin = null;
            ActivelyDisconnectingWire = null;
            _connectionsFromActivelyDisconnectingPin.Clear();
        }

        #endregion
    }
}