using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private ObservableCollection<ISynthModule> _connectedModules; 

        public ShapeGeneratorBase()
        {
            Center = new Vector3(0);
            Vertices = new ObservableCollection<VertexModel>();
            ConnectedModules = new ObservableCollection<ISynthModule>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;

            ConstructionValidated = false;
        }

        
        #region Properties

        public ObservableCollection<VertexModel> Vertices
        {
            get { return _vertices; }
            set { SetProperty(ref _vertices, value); }
        }

        public ObservableCollection<ISynthModule> ConnectedModules
        {
            get { return _connectedModules; } 
            private set { SetProperty(ref _connectedModules, value); }
        }

        public Vector3 Center { get; set; }

        public virtual SynthModuleType ModuleType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int NumVertices
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Event Handlers

        private void OnConnectedModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        #endregion


        #region Methods

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

        public virtual bool ConnectSynthModule(/*int pin,*/ ISynthModule module)
        {
            //if(!IsAbleToAttach())
            //{
            //    return false;
            //}

            ConnectedModules.Add(module);

            return true;
        }

        public virtual bool DisconnectSynthModule(/*int pin,*/ ISynthModule module)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}