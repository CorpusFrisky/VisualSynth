﻿using System.Linq;
using Autofac;
using Autofac.Core;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;
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




            var moduleView = RegionManager.Regions[RegionNames.LeftControlRegion].Views.FirstOrDefault(x => x.GetType() == typeof(TriangleGeneratorView)) as TriangleGeneratorView;

            if (moduleView == null)
            {
                moduleView = parameters == null ? ComponentContext.Resolve<TriangleGeneratorView>() : ComponentContext.Resolve<TriangleGeneratorView>(parameters);
                RegionManager.RegisterViewWithRegion(RegionNames.LeftControlRegion, () => moduleView);
            }

            ComponentContext.Resolve<ControlsWindow>().Show();
        }
    }
}
