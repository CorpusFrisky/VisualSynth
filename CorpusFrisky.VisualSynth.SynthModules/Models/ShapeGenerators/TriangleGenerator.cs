using CorpusFrisky.VisualSynth.Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators
{
    public class TriangleGenerator : ShapeGeneratorBase
    {
        public TriangleGenerator()
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

        public override int NumVertices
        {
            get { return 3; }
        }

        public override SynthModuleType ModuleType
        {
            get { return SynthModuleType.TRIANGLE_GENERATOR; }
        }

        public override void PreRender()
        {
            if (!ConstructionValidated)
            {
                ValidateConstruction(NumVertices);
                ConstructionValidated = true;
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
                GL.Color4(vertex.Color);
                GL.Vertex3(vertex.Position);
            }

            GL.End();
        }
    }
}