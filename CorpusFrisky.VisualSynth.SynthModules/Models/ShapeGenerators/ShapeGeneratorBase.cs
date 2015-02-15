﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;

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
