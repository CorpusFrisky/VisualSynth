using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpusFrisky.VisualSynth.Common;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Modifiers
{
    public class Oscillator : IPropertyModifierModule
    {
        private const int TableLength = 1000;
        private static double[] _sinTable;

        private double _index;

        public Oscillator()
        {
            _index = 0;
        }


        public static void InitOscillatorTables()
        {
            _sinTable = new double[TableLength];
            for (var i = 0; i < _sinTable.Length; i++)
            {
                _sinTable[i] = Math.Sin((i/_sinTable.Length)*2*Math.PI);
            }
        }

        public double Rate { get; set; }

        public void PreRender()
        {
            _index += (Rate * TableLength / Constants.FrameRate) % TableLength;
        }

        public void Render()
        {
        }

        public void PostRender()
        {
            
        }

        public SynthModuleType ModuleType { get; private set; }

        public double GetValue()
        {
            var index = (int)_index;
            var dec = _index - index;

            var lowVal = _sinTable[index];
            var highVal = _sinTable[index + 1];

            return lowVal + ((highVal - lowVal)*dec);

        }
    }
}
