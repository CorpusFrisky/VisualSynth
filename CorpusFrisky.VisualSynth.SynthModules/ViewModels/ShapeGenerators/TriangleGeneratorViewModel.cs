using System.Collections.Specialized;
using System.Linq;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class TriangleGeneratorViewModel : ShapeGeneratorBaseViewModel
    {
        public TriangleGeneratorViewModel()
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
            get { return SynthModuleType.TRIANGLE_GENERATOR; }
        }

        #endregion

        #region Methods

        public override void PreRender()
        {
            if (!ConstructionValidated)
            {
                ValidateConstruction(NumVertices);
                ConstructionValidated = true;
            }

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


        protected override void OnConnectedModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    AddConnectedModule(newItem as ConnectedModule);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItems in e.OldItems)
                {
                    RemoveConnectedModule(oldItems as ConnectedModule);
                }
            }
        }


        #endregion

        #region Helper Methods

        private void AddConnectedModule(ConnectedModule connectedModule)
        {
            var modifierModule = connectedModule.Module as IModifierModule;
            if (modifierModule == null)
            {
                // TODO: log a message
                return;
            }

            //TODO: Come up with a good way to associated pins with properties
            switch (connectedModule.Pin)
            {
                case 0:
                    Vertices[0].AddPropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
                case 1:
                    Vertices[1].AddPropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
                case 2:
                    Vertices[2].AddPropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
            }
        }

        private void RemoveConnectedModule(ConnectedModule connectedModule)
        {
            var modifierModule = connectedModule.Module as IModifierModule;
            if (modifierModule == null)
            {
                // TODO: log a message
                return;
            }

            switch (connectedModule.Pin)
            {
                case 0:
                    Vertices[0].RemovePropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
                case 1:
                    Vertices[1].RemovePropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
                case 2:
                    Vertices[2].RemovePropertyModifier(VertexModel.VertexProperty.Color, modifierModule);
                    break;
            }
        }

        #endregion
    }
}