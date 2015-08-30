using System;
using System.Collections.ObjectModel;
using System.Drawing;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;

namespace CorpusFrisky.VisualSynth.ViewModels
{
    public class DesignViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand _addTriangleCommand;
        private DelegateCommand _addRectangleCommand;
        private DelegateCommand<SynthComponentModel> _handleModuleLeftClick;

        #endregion


        public DesignViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SynthComponents = new ObservableCollection<SynthComponentModel>();

            AddTestOscillator();
        }


        #region Properties

        public ObservableCollection<SynthComponentModel> SynthComponents { get; set; }

        public Point CurrentDesignPos { get; set; }

        #endregion


        #region Commands

        public DelegateCommand AddTriangleCommand
        {
            get
            {
                if (_addTriangleCommand == null)
                {
                    _addTriangleCommand = new DelegateCommand(AddTriangle);
                }

                return _addTriangleCommand;
            }
        }

        public DelegateCommand AddRectangleCommand
        {
            get
            {
                if (_addRectangleCommand == null)
                {
                    _addRectangleCommand = new DelegateCommand(AddRectangle);
                }

                return _addRectangleCommand;
            }
        }

        public DelegateCommand<SynthComponentModel> HandleModuleLeftClickCommand
        {
            get
            {
                if (_handleModuleLeftClick == null)
                {
                    _handleModuleLeftClick = new DelegateCommand<SynthComponentModel>(HandleModuleLeftClick);
                }

                return _handleModuleLeftClick;
            }
        }

        #endregion


        #region Command Handlers

        private void AddTriangle()
        {
            var rand = new Random();
            var triangle = new TriangleGeneratorViewModel()
                           {
                               Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                           };

            SynthComponents.Add(new SynthComponentModel
                                {
                                    DesignPos = CurrentDesignPos,
                                    Module = triangle
                                });

            _eventAggregator.GetEvent<ModuleAddedEvent>().Publish(new ModuleAddedEventArgs
                                                                  {
                                                                      Module = triangle
                                                                  });
        }

        private void AddRectangle()
        {
            var rand = new Random();
            var rectangle = new RectangleGeneratorViewModel
                            {
                                //DesignPos = new Point((int)mousePoint.X, (int)mousePoint.Y),
                                Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                            };

            SynthComponents.Add(new SynthComponentModel
                                {
                                    DesignPos = CurrentDesignPos,
                                    Module = rectangle
                                });

            _eventAggregator.GetEvent<ModuleAddedEvent>().Publish(new ModuleAddedEventArgs
                                                                  {
                                                                      Module = rectangle
                                                                  });
        }

        private void HandleModuleLeftClick(SynthComponentModel componentModel)
        {
            _eventAggregator.GetEvent<ModuleAddedEvent>().Publish(new ModuleAddedEventArgs
                                                                  {
                                                                      Module = componentModel.Module
                                                                  });
        }

        #endregion

        #region Helper Methods

        private void AddTestOscillator()
        {
            var osc = new Oscillator();

            SynthComponents.Add(new SynthComponentModel()
            {
                DesignPos = new Point(-100, -100),
                Module = osc
            });
        }

        #endregion
    }
}