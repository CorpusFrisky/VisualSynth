using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
                    _handleModuleLeftClick = new DelegateCommand<SynthComponentModel>(HandleModuleLeftClick);

                return _handleModuleLeftClick;
            }
        }

        #endregion

        #region Command Handlers

        private void AddTriangle()
        {
            var rand = new Random();
            var triangle = new TriangleGenerator()
            {
                Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                VertexPositions = new List<Vector3>
                {
                    new Vector3(-100.0f,0.0f,0.0f),
                    new Vector3(100.0f,0.0f,0.0f),
                    new Vector3(0.0f,100.0f,0.0f)
                },
                VertexColors = new ObservableCollection<Color4>
                {
                    new Color4(1.0f,0.0f,0.0f,0.0f),
                    new Color4(1.0f,1.0f,0.0f,0.0f),
                    new Color4(1.0f,0.0f,1.0f,0.0f),
                }
            };

            var componentModel = new SynthComponentModel
            {
                DesignPos = CurrentDesignPos,
                Module = triangle
            };

            SynthComponents.Add(componentModel);

            _eventAggregator.GetEvent<ModuleAddedEvent>().Publish(new ModuleAddedEventArgs
                                                                  {
                                                                      Module = triangle
                                                                  });
        }

        private void AddRectangle()
        {
            var rand = new Random();
            var rectangle = new RectangleGenerator
            {
                //DesignPos = new Point((int)mousePoint.X, (int)mousePoint.Y),
                Center = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f),
                VertexPositions = new List<Vector3>
                {
                    new Vector3(-100.0f,0.0f,0.0f),
                    new Vector3(100.0f,0.0f,0.0f),
                    new Vector3(100.0f,100.0f,0.0f),
                    new Vector3(-100.0f,100.0f,0.0f)
                },
                VertexColors = new ObservableCollection<Color4>
                {
                    new Color4(1.0f,0.0f,0.0f,0.0f),
                    new Color4(1.0f,1.0f,0.0f,0.0f),
                    new Color4(1.0f,0.0f,1.0f,0.0f),
                    new Color4(1.0f,0.0f,1.0f,0.0f)
                }
            };

            SynthComponents.Add(new SynthComponentModel
            {
                DesignPos = CurrentDesignPos,
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
    }
}