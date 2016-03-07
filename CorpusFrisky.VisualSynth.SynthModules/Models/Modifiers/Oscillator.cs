using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Modifiers
{
    public class Oscillator : IPropertyModifierModule
    {
        private const int TableLength = 1000;
        private static double[] _sinTable;

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

        public Oscillator()
        {
            _index = 0;
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

        public double Rate { get; set; }

       
        public SynthModuleType ModuleType { get; private set; }
        public ObservableCollection<Pin> Pins { get; set; }

        #endregion

        #region ISynthModule Implementations

        public void SetupPins()
        {
            throw new NotImplementedException();
        }

        public void PreRender()
        {
            _index += Rate * TableLength / Constants.FrameRate;
            _index %= TableLength;
        }

        public void Render()
        {
        }

        public void PostRender()
        {
            
        }

        public bool ConnectSynthModule(Pin pin, ISynthModule module)
        {
            throw new NotImplementedException();
        }

        public bool DisconnectSynthModule(Pin pin, ISynthModule module)
        {
            throw new NotImplementedException();
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
