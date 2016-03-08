﻿using System.Collections.Specialized;
using System.Linq;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class TriangleGeneratorViewModel : ShapeGeneratorBaseViewModel
    {
        public TriangleGeneratorViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            SetupDefaultVertices();
        }

        private void SetupDefaultVertices()
        {
            Vertices.Add(new VertexModel()
                         {
                             Color = new Color4(1.0f, 0.0f, 0.0f, 0.0f),
                             Position = new Vector3(-100.0f, 0.0f, 0.0f)
                         });

            Vertices.Add(new VertexModel()
                         {
                             Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                             Position = new Vector3(100.0f, 0.0f, 0.0f)
                         });

            Vertices.Add(new VertexModel()
                         {
                             Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                             Position = new Vector3(0.0f, 100.0f, 0.0f)
                         });
        }

        #region Properties

        public override int NumVertices
        {
            get { return 3; }
        }

        public override SynthModuleType ModuleType
        {
            get { return SynthModuleType.TriangleGenerator; }
        }

        #endregion

        #region Methods

        public override void PreRender()
        {
            foreach (var vertex in Vertices)
            {
                vertex.ApplyModifiers();
            }
        }

        public override void Render()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1000.0, 0.0, 1000.0, 0.0, 4.0);
            GL.Translate(Center);

            GL.Begin(BeginMode.Triangles);//(PrimitiveType.Triangles);

            foreach (var vertex in Vertices)
            {
                GL.Color4(vertex.ModifiedColor);
                GL.Vertex3(vertex.Position);
            }

            GL.End();
        }

        public override void PostRender()
        { }

        #endregion
    }
}