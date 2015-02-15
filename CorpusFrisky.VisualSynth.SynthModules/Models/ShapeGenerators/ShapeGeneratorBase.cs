using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators
{
    public class ShapeGeneratorBase : ISynthModule
    {
        protected bool ConstructionValidated;

        public ShapeGeneratorBase()
        {
            Center = new Vector3(0);
            VertexPositions = new List<Vector3>();
            VertexColors = new List<Color4>();

            ConstructionValidated = false;
        }

        public Vector3 Center { get; set; }
        public List<Vector3> VertexPositions { get; set; }
        public List<Color4> VertexColors { get; set; }

        protected void ValidateConstruction(int numVertices)
        {
            while (VertexPositions.Count < numVertices)
            {
                VertexPositions.Add(new Vector3());
            }

            while (VertexColors.Count < numVertices)
            {
                VertexColors.Add(new Color4());
            }
        }

        public virtual void PreRender()
        {
            throw new NotImplementedException();
        }

        public virtual void Render()
        {
            throw new NotImplementedException();
        }

        public virtual void PostRender()
        {
            throw new NotImplementedException();
        }
    }
}
