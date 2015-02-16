using System.Linq;
using System.Windows;
using Autofac;
using Autofac.Core;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators;
using CorpusFrisky.VisualSynth.Views.ControlViews;
using CorpusFrisky.VisualSynth.Views.Windows;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace CorpusFrisky.VisualSynth.Controllers
{
    public class ControlsWindowController : BaseController, IControlsWindowController
    {
        public ControlsWindowController(ILifetimeScope componentContext, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(componentContext, regionManager, eventAggregator)
        {
            SubscribeToEvents();
        }
        
        private void SubscribeToEvents()
        {
            EventAggregator.GetEvent<ModuleAddedEvent>().Subscribe(ModuleAdded);
        }

        public override void Show()
        {
            Show(null);
        }

        public override void Show(params Parameter[] parameters)
        {
            var designView = RegionManager.Regions[RegionNames.LowerControlRegion].Views.FirstOrDefault(x => x.GetType() == typeof(DesignView)) as DesignView;

            if(designView == null)
            {
                designView = parameters == null ? ComponentContext.Resolve<DesignView>() : ComponentContext.Resolve<DesignView>(parameters);
                RegionManager.RegisterViewWithRegion(RegionNames.LowerControlRegion, () => designView);
            }

            ComponentContext.Resolve<ControlsWindow>().Show();
        }

        private void ModuleAdded(ModuleAddedEventArgs args)
        {
            var currentView = RegionManager.Regions[RegionNames.LeftControlRegion].Views.FirstOrDefault(x => x.GetType() == typeof(TriangleGeneratorView)) as TriangleGeneratorView;

            if (currentView != null)
            {
                RegionManager.Regions[RegionNames.LeftControlRegion].Remove(currentView);
            }

            var moduleView = ComponentContext.Resolve<TriangleGeneratorView>(new TypedParameter(typeof(ISynthModule), args.Module));
            //moduleView.ViewModel.Module = args.Module;
            RegionManager.RegisterViewWithRegion(RegionNames.LeftControlRegion, () => moduleView);
        }
    }
}
