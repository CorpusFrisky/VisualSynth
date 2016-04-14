using System;
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
using System.Reflection;
using System.Windows.Controls;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using CorpusFrisky.VisualSynth.SynthModules.Views.Modifiers;

namespace CorpusFrisky.VisualSynth.Controllers
{
    public class ControlsWindowController : BaseController, IControlsWindowController
    {
        private readonly Type AutoFacResExtenionsType = null;

        public ControlsWindowController(ILifetimeScope componentContext, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(componentContext, regionManager, eventAggregator)
        {
            SubscribeToEvents();

            AutoFacResExtenionsType = typeof(Autofac.ResolutionExtensions);
        }

        private void SubscribeToEvents()
        {
            EventAggregator.GetEvent<ModuleAddedOrClickedEvent>().Subscribe(ModuleAddedOrClicked);
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

        private void ModuleAddedOrClicked(ModuleAddedOrClickedEventArgs args)
        {
            var currentView = RegionManager.Regions[RegionNames.LeftControlRegion].Views.FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(UserControl)));

            if (currentView != null)
            {
                RegionManager.Regions[RegionNames.LeftControlRegion].Remove(currentView);
            }

            var view = GetViewForModule(args.Module);
            RegionManager.RegisterViewWithRegion(RegionNames.LeftControlRegion, () => view);
        }

        private UserControl GetViewForModule(ISynthModule module)
        {
            UserControl view = null;
            switch (module.ModuleType)
            {
                case SynthModuleType.Output:
                    view = ComponentContext.Resolve<OutputView>();
                    view.DataContext = module as OutputViewModel;
                    break;
                case SynthModuleType.TriangleGenerator:
                    view = ComponentContext.Resolve<ShapeGeneratorView>();
                    view.DataContext = module as TriangleGeneratorViewModel;
                    break;
                case SynthModuleType.RectangleGenerator:
                    view = ComponentContext.Resolve<ShapeGeneratorView>();
                    view.DataContext = module as RectangleGeneratorViewModel;
                    break;
                case SynthModuleType.Oscillator:
                    view = ComponentContext.Resolve<OscillatorView>();
                    view.DataContext = module as OscillatorViewModel;
                    break;
                default:
                    return null;
            }

            return view;
        }
    }
}
