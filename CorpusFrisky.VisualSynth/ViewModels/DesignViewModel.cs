using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using CorpusFrisky.VisualSynth.Modules;
using CorpusFrisky.VisualSynth.Modules.ShapeGenerators;
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

            Shapes = new List<ISynthModule>();
        }

        #region Properties

        public List<ISynthModule> Shapes { get; set; }

        #endregion

        #region Commands

        public ICommand AddTriangleCommand
        {
            get
            {
                if (_addTriangleCommand == null)
                {
                    _addTriangleCommand = new DelegateCommand<Canvas>(AddTriangle);
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
                    _addRectangleCommand = new DelegateCommand<Canvas>(AddRectangle);
                }

                return _addRectangleCommand;
            }
        }

        #endregion

        #region Command Handlers

        private void AddTriangle(Canvas canvas)
        {
            var rand = new Random();
            var mousePoint = Mouse.GetPosition(canvas);
            var triangle = new TriangleGenerator()
            {
                //DesignPos = new Point((int)mousePoint.X, (int)mousePoint.Y),
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

            Shapes.Add(triangle);
        }

        private void AddRectangle(Canvas canvas)
        {
            var rand = new Random();
            var mousePoint = Mouse.GetPosition(canvas);
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

            Shapes.Add(rectangle);
        }

        #endregion
    }
}