using System;
using System.Collections.ObjectModel;
using System.Configuration;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers
{
    public class OscillatorViewModel : SynthModuleBase, IPropertyModifierModule
    {
        private const int TableLength = 1000;
        private static double[] _sinTable;

        private double _rate;

        private double _index;
        private double _cachedIndex;
        private double _cachedValue;

        public enum OscillatorType
        {
            Sine,
            Triangle,
            Square,
            RampUp,
            RampDown,
            SampleAndHold
        }

        public OscillatorViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            _index = 0;
            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
        }

        public override void Initialize()
        {
            base.Initialize();

            SetupPins();

        }

        public static void InitOscillatorTables()
        {
            _sinTable = new double[TableLength];
            for (var i = 0; i < _sinTable.Length; i++)
            {
                _sinTable[i] = Math.Sin(i/((double)_sinTable.Length)*2d*Math.PI);
            }
        }

        #region Properties

        public IEventAggregator EventAggregator { get; private set; }


        public double Rate
        {
            get { return _rate; }
            set { SetProperty(ref _rate, value); }
        }


        public override SynthModuleType ModuleType
        {
            get { return SynthModuleType.Oscillator; }
        }

        #endregion

        #region ISynthModule Implementations

        protected override void SetupPins()
        {
            var pinIndex = 0;

            OutputPins.Add(new OutputValuePin
            {
                Module = this,
                PinIndex = pinIndex++,
                Label = "Output",
                PinType = PinTypeEnum.Value
            });

            EventAggregator.GetEvent<PinSetupCompleteEvent>().Publish(new PinSetupCompleteEventArgs
            {
                SynthModule = this
            });
        }

        public override void PreRender()
        {
            _index += Rate * TableLength / Constants.FrameRate;
            _index %= TableLength;
        }

        public override void Render()
        {
        }

        public override void PostRender()
        {
            
        }

        public override bool  ConnectSynthModule(PinBase pin, ISynthModule module)
        {
            if (!base.ConnectSynthModule(pin, module))
            {
                return false;
            }

            return true;
        }

        public override void DisconnectSynthModule(PinBase pin, ISynthModule module)
        {
            base.DisconnectSynthModule(pin, module);
        }

        #endregion

        #region  IPropertyModifierModule

        public double GetValue()
        {
            //Only recalculate if we've changed index since the last check.
            if (_cachedIndex != _index)
            {
                _cachedIndex = _index;

                var index = (int)_index;
                var dec = _index - index;

                var lowVal = _sinTable[index];
                var highVal = _sinTable[(index + 1) % _sinTable.Length];

                _cachedValue = lowVal + ((highVal - lowVal) * dec);
            }

            return _cachedValue;
        }

        #endregion
    }
}
