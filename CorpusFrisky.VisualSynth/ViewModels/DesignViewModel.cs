using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CorpusFrisky.VisualSynth.ViewModels
{
    public interface IShape
    {
        Vector3 DisplayCenter { get; set; }
        Point DesignPos { get; set; }

        void Draw();
    }

    public class Triangle : IShape 
    {
        public Triangle()
        {
            Vertices = new Vector3[3];
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vertices = new[] { v1, v2, v3 };
        }

        public Vector3[] Vertices { get; set; }
        public Vector3 DisplayCenter { get; set; }
        public Point DesignPos { get; set; }

        public void Draw()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1000.0, 0.0, 1000.0, 0.0, 4.0);
            GL.Translate(DisplayCenter);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex3(Vertices[0]);
            GL.Color3(Color.SpringGreen);
            GL.Vertex3(Vertices[1]);
            GL.Color3(Color.Ivory);
            GL.Vertex3(Vertices[2]);

            GL.End();
        }
    }

    public class Rectangle : IShape
    {
        public Rectangle()
        {
            Vertices = new Vector3[4];
        }

        public Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            Vertices = new[] { v1, v2, v3, v4 };
        }

        public Vector3[] Vertices { get; set; }
        public Vector3 DisplayCenter { get; set; }
        public Point DesignPos { get; set; }

        public void Draw()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1000.0, 0.0, 1000.0, 0.0, 4.0);
            GL.Translate(DisplayCenter);

            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex3(Vertices[0]);
            GL.Color3(Color.SpringGreen);
            GL.Vertex3(Vertices[1]);
            GL.Color3(Color.Ivory);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);

            GL.End();
        }
    }


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

            Shapes = new List<IShape>();
        }

        #region Properties

        public List<IShape> Shapes { get; set; }

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
            var triangle = new Triangle(new Vector3(-100.0f, -100.0f, 0.0f),
                new Vector3(100.0f, -100.0f, 0.0f),
                new Vector3(0.0f, 100.0f, 0.0f)
                )
            {
                DesignPos = new Point((int)mousePoint.X, (int)mousePoint.Y),
                DisplayCenter = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f)
            };

            Shapes.Add(triangle);
        }

        private void AddRectangle(Canvas canvas)
        {
            var rand = new Random();
            var mousePoint = Mouse.GetPosition(canvas);
            var rectangle = new Rectangle(new Vector3(100.0f, 100.0f, 0.0f),
                new Vector3(100.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 100.0f, 0.0f)
                )
            {
                DesignPos = new Point((int)mousePoint.X, (int)mousePoint.Y),
                DisplayCenter = new Vector3(rand.Next(1000), rand.Next(1000), 0.0f)
            };

            Shapes.Add(rectangle);
        }

        #endregion
    }
}