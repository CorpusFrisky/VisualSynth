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
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using CorpusFrisky.VisualSynth.SynthModules.Models.ShapeGenerators;

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
            var currentView = RegionManager.Regions[RegionNames.LeftControlRegion].Views.FirstOrDefault(x => x.GetType() == typeof(UserControl));

            if (currentView != null)
            {
                RegionManager.Regions[RegionNames.LeftControlRegion].Remove(currentView);
            }

            //TODO: Clean up this mess.

            var viewType = GetViewTypeFromModule(args.Module);

            var compContextType = ComponentContext.GetType().GetInterfaces().Single(e => e.Name == "ILifetimeScope");
            var resExtensionsType = typeof(Autofac.ResolutionExtensions);
            var test = resExtensionsType.GetMethods();
            Console.Out.Write(test);
            MethodInfo method = resExtensionsType.GetMethod("Resolve", new[] { typeof(IComponentContext), typeof(Parameter[])})
                                                  .MakeGenericMethod(viewType);
            var moduleView = method.Invoke(this, new object[] {  ComponentContext,
                                                                 new Parameter[] {new TypedParameter(typeof(ISynthModule), args.Module) }
                                                              });

            //var moduleView = ComponentContext.Resolve<viewType.>(new TypedParameter(typeof(ISynthModule), args.Module));
            RegionManager.RegisterViewWithRegion(RegionNames.LeftControlRegion, () => moduleView);
        }

        private Type GetViewTypeFromModule(ISynthModule module)
        {
            switch (module.ModuleType)
            {
                case SynthModuleType.TRIANGLE_GENERATOR:
                    return typeof(TriangleGeneratorView);
                case SynthModuleType.RECTANGLE_GENERATOR:
                    //return typeof (RectangleGeneratorView);
                    return null;

            }

            return null;
        }
    }
}
