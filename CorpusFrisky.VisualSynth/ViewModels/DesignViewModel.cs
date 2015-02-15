using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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

        private ICommand _addTriangleCommand;
        private ICommand _addRectangleCommand;

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

        public ICommand AddTriangleCommand
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

        public ICommand AddRectangleCommand
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
                VertexColors = new List<Color4>
                {
                    new Color4(1.0f,0.0f,0.0f,0.0f),
                    new Color4(1.0f,1.0f,0.0f,0.0f),
                    new Color4(1.0f,0.0f,1.0f,0.0f),
                }
            };

            SynthComponents.Add(new SynthComponentModel
            {
                DesignPos = CurrentDesignPos,
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
                VertexColors = new List<Color4>
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
        #endregion
    }
}