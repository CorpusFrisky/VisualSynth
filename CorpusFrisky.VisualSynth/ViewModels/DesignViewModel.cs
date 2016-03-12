using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using OpenTK.Audio;

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
             
        private PinBase _activelyConnectingPin;
        private Point _currentMousePos;

        #endregion


        public DesignViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SynthComponents = new ObservableCollection<SynthComponentModel>();
            ConnectionWires = new ObservableCollection<ConnectionWire>();
            ActivelyConnectingPin = null;

            SubscribeToEvents();
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

        public DelegateCommand<PinBase> PinLeftClickedCommand
        {
            get { return _pinLeftClickedCommand ?? (_pinLeftClickedCommand = new DelegateCommand<PinBase>(PinLeftClicked)); }
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

            SynthComponents.Add(new SynthComponentModel
                                {
                                    DesignPos = CurrentDesignPos,
                                    Module = triangle
                                });
            triangle.Initialize();

            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
                                                                  {
                                                                      Module = triangle
                                                                  });
        }

        private void AddRectangle()
        {
            var rand = new Random();
            var rectangle = new RectangleGeneratorViewModel(_eventAggregator)
                            {
                                Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                            };

            SynthComponents.Add(new SynthComponentModel
                                {
                                    DesignPos = CurrentDesignPos,
                                    Module = rectangle
                                });
            rectangle.Initialize();

            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
                                                                  {
                                                                      Module = rectangle
                                                                  });
        }

        private void AddOscillator()
        {
            var oscillator = new OscillatorViewModel(_eventAggregator)
            {
                Rate = 1.0,
            };

            SynthComponents.Add(new SynthComponentModel
            {
                DesignPos = CurrentDesignPos,
                Module = oscillator
            });
            oscillator.Initialize();

            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
            {
                Module = oscillator
            });
        }

        private void HandleModuleLeftClick(SynthComponentModel componentModel)
        {
            _eventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Publish(new ModuleAddedOrClickedEventArgs
                                                                  {
                                                                      Module = componentModel.Module
                                                                  });
        }

        private void PinLeftClicked(PinBase pin)
        {
            if (ActivelyConnectingPin == null)
            {
                ActivelyConnectingPin = pin;
            }
            else
            {
                if (ActivelyConnectingPin.IsInput)
                {
                    if (pin.IsInput)
                    {
                        return;
                    }

                    ActivelyConnectingPin.ConnectSynthModule(ActivelyConnectingPin, pin.Module);

                    ConnectionWires.Add(new ConnectionWire
                    {
                        IsHighlighted = false,
                        Pin1Pos = GetPinCenterPos(pin),
                        Pin2Pos = GetPinCenterPos(ActivelyConnectingPin)
                    });
                }
                else
                {
                    if (!pin.IsInput)
                    {
                        return;
                    }

                    pin.Module.ConnectSynthModule(pin, ActivelyConnectingPin.Module);

                    ConnectionWires.Add(new ConnectionWire
                    {
                        IsHighlighted = false,
                        Pin1Pos = GetPinCenterPos(ActivelyConnectingPin),
                        Pin2Pos = GetPinCenterPos(pin)
                    });
                }

                ActivelyConnectingPin = null;
            }
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

        Point GetPinCenterPos(PinBase pin)
        {
            var component = SynthComponents.FirstOrDefault(x => x.Module == pin.Module);
            if (component == null)
            {
                return Point.Empty;
            }

            var pinPos = Point.Add(component.DesignPos, new Size(pin.PinDesignPos));
            //offset to pin center
            return Point.Add(pinPos, new Size(5, 5));
        }

        #endregion
    }
}