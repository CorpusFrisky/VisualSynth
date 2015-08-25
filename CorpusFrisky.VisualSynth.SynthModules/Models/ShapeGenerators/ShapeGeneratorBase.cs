using System;
using System.Collections.ObjectModel;
using CorpusFrisky.VisualSynth.Common;
using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators
{
    public class ShapeGeneratorBase : BindableBase, ISynthModule
    {
        protected bool ConstructionValidated;
        private ObservableCollection<VertexModel> _vertices;

        public ShapeGeneratorBase()
        {
            Center = new Vector3(0);
            Vertices = new ObservableCollection<VertexModel>();

            ConstructionValidated = false;
        }

        public ObservableCollection<VertexModel> Vertices
        {
            get { return _vertices; }
            set { SetProperty(ref _vertices, value); }
        }

        public Vector3 Center { get; set; }

        protected void ValidateConstruction(int numVertices)
        {
            while (Vertices.Count < numVertices)
            {
                Vertices.Add(new VertexModel()
                             {
                                 Color = new Color4(),
                                 Position = new Vector3()
                             });
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