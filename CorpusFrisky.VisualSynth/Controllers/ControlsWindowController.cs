using Autofac;
using Autofac.Core;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators;
using CorpusFrisky.VisualSynth.Views.ControlViews;
using CorpusFrisky.VisualSynth.Views.Windows;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System.Linq;
using System.Windows.Controls;

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

            if (designView == null)
            {
                designView = parameters == null ? ComponentContext.Resolve<DesignView>() : ComponentContext.Resolve<DesignView>(parameters);
                RegionManager.RegisterViewWithRegion(RegionNames.LowerControlRegion, () => designView);
            }

            ComponentContext.Resolve<ControlsWindow>().Show();
        }

        private void ModuleAdded(ModuleAddedEventArgs args)
        {
            var currentView = RegionManager.Regions[RegionNames.LeftControlRegion].Views.FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(UserControl)));

            if (currentView != null)
            {
                RegionManager.Regions[RegionNames.LeftControlRegion].Remove(currentView);
            }

            RegionManager.RegisterViewWithRegion(RegionNames.LeftControlRegion, () => GetViewFromModule(args.Module));
        }

        private object GetViewFromModule(ISynthModule module)
        {
            switch (module.ModuleType)
            {
                case SynthModuleType.TRIANGLE_GENERATOR:
                    return new TriangleGeneratorView(module);
                case SynthModuleType.RECTANGLE_GENERATOR:
                    return new RectangleGeneratorView(module);
                default:
                    return null;
            }
        }
    }
}
