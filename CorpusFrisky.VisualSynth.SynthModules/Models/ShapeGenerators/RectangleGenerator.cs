using OpenTK.Graphics.OpenGL;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators
{
    public class RectangleGenerator : ShapeGeneratorBase
    {
        public static int NumVertices = 4;

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

            GL.Begin(BeginMode.Quads);

            GL.Color4(VertexColors[0]);
            GL.Vertex3(VertexPositions[0]);
            GL.Color4(VertexColors[1]);
            GL.Vertex3(VertexPositions[1]);
            GL.Color4(VertexColors[2]);
            GL.Vertex3(VertexPositions[2]);
            GL.Color4(VertexColors[3]);
            GL.Vertex3(VertexPositions[3]);

            GL.End();
        }
    }
}