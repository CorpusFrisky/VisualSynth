﻿using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.ObjectModel;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers
{
    public class OscillatorViewModel : SynthModuleBaseViewModel, IModifierModule
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

        public OscillatorViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _index = 0;
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
                _sinTable[i] = Math.Sin(i / ((double)_sinTable.Length) * 2d * Math.PI);
            }
        }

        #region Properties

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
                PinType = PinTypeEnum.Value,
                GetValue_Function = GetValue
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

        public override void Render(bool fromFinalRenderCall = false)
        {
        }

        public override void PostRender()
        {

        }

        public override bool ConnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            if (!base.ConnectSynthModule(inputPin, outputPin))
            {
                return false;
            }

            return true;
        }

        public override void DisconnectSynthModule(InputPin inputPin, OutputPin outputPin)
        {
            base.DisconnectSynthModule(inputPin, outputPin);
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
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
