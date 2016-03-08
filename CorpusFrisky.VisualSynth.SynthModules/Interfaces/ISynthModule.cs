﻿using System.Collections.ObjectModel;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;

namespace CorpusFrisky.VisualSynth.SynthModules.Interfaces
{
    public interface ISynthModule
    {
        void Initialize();
        void SetupPins();
        void PreRender();
        void Render();
        void PostRender();

        bool ConnectSynthModule(PinBase pin, ISynthModule module);
        bool DisconnectSynthModule(PinBase pin, ISynthModule module);


        SynthModuleType ModuleType { get; }

        ObservableCollection<PinBase> InputPins { get; set; } 
        ObservableCollection<PinBase> OutputPins { get; set; }
    }
}