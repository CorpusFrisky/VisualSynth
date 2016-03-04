using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators
{
    public class ShapeGeneratorBaseViewModel : BindableBase, ISynthModule
    {
        protected bool ConstructionValidated;
        private ObservableCollection<VertexModel> _vertices;

        private ObservableCollection<ConnectedModule> _connectedModules; 

        public ShapeGeneratorBaseViewModel()
        {
            Center = new Vector3(0);
            Vertices = new ObservableCollection<VertexModel>();
            ConnectedModules = new ObservableCollection<ConnectedModule>();

            ConnectedModules.CollectionChanged += OnConnectedModulesChanged;

            ConstructionValidated = false;
        }

        
        #region Properties

        public ObservableCollection<VertexModel> Vertices
        {
            get { return _vertices; }
            set { SetProperty(ref _vertices, value); }
        }

        public ObservableCollection<ConnectedModule> ConnectedModules
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



        protected virtual void OnConnectedModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        protected virtual void AddConnectedModule(ConnectedModule connectedModule)
        {
            throw new NotImplementedException();
        }

        protected virtual void RemoveConnectedModule(ConnectedModule connectedModule)
        {
            throw new NotImplementedException();
        }

        public bool ConnectSynthModule(int pin, ISynthModule module)
        {
            //if(!IsAbleToAttach())
            //{
            //    return false;
            //}

            ConnectedModules.Add(new ConnectedModule
            {
                Pin = pin,
                Module = module
            });

            return true;
        }

        public bool DisconnectSynthModule(int pin, ISynthModule module)
        {
            var moduleToDisconnect = ConnectedModules.First(x => x.Module == module && x.Pin == pin);
            if (moduleToDisconnect != null)
            {
                ConnectedModules.Remove(moduleToDisconnect);
                return true;
            }

            return false;
        }

        #endregion
    }
}