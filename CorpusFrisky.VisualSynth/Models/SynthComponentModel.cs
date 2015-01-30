﻿using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules;

namespace CorpusFrisky.VisualSynth.Models
{
    public class SynthComponentModel
    {
        public ISynthModule Module { get; set; }
        public Point DesignPos { get; set; }
    }
}