using System;
using Autofac;
using Autofac.Core;
using CorpusFrisky.VisualSynth.Controllers.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace CorpusFrisky.VisualSynth.Controllers
{
    public class GlobalController : BaseController
    {
        private readonly IControlsWindowController _controlsWindowController;
        private readonly IDisplayWindowController _displayWindowController;

        public GlobalController(ILifetimeScope componentContext, IRegionManager regionManager, IEventAggregator eventAggregator,
            IControlsWindowController controlsWindowController, IDisplayWindowController displayWindowController)
            : base(componentContext, regionManager, eventAggregator)
        {
            _controlsWindowController = controlsWindowController;
            _displayWindowController = displayWindowController;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
        }

        public override void Show()
        {
            _controlsWindowController.Show();
            _displayWindowController.Show();
        }

        public override void Show(params Parameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
