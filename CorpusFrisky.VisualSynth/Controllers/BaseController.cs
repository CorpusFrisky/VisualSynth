using Autofac;
using Autofac.Core;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace CorpusFrisky.VisualSynth.Controllers
{
    public abstract class BaseController
    {
        protected readonly ILifetimeScope ComponentContext;
        protected readonly IRegionManager RegionManager;
        protected readonly IEventAggregator EventAggregator;

        protected BaseController(ILifetimeScope componentContext, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            ComponentContext = componentContext;
            RegionManager = regionManager;
            EventAggregator = eventAggregator;
        }

        public abstract void Show();
        public abstract void Show(params Parameter[] parameters);
    }
}
