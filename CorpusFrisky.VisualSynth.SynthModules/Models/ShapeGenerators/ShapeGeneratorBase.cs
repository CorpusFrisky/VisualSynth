using CorpusFrisky.VisualSynth.Common;
using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators
{
    public class ShapeGeneratorBase : BindableBase, ISynthModule
    {
        protected bool ConstructionValidated;

        public ShapeGeneratorBase()
        {
            Center = new Vector3(0);
            VertexPositions = new List<Vector3>();
            VertexColors = new ObservableCollection<Color4>();

            ConstructionValidated = false;
        }

        public Vector3 Center { get; set; }
        public List<Vector3> VertexPositions { get; set; }
        public ObservableCollection<Color4> VertexColors { get; set; }

        public Color4 ColorV0
        {
            get { return VertexColors[0]; }
            set { VertexColors[0] = value; }
        }

        public Color4 ColorV1
        {
            get { return VertexColors[1]; }
            set { VertexColors[1] = value; }
        }

        public Color4 ColorV2
        {
            get { return VertexColors[2]; }
            set { VertexColors[2] = value; }
        }

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

        public virtual SynthModuleType ModuleType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int NumVertices
        {
            get { throw new NotImplementedException(); }
        }
    }
}
