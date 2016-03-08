using System.Drawing;

namespace CorpusFrisky.VisualSynth.Models
{
    public class ConnectionWire
    {
        public Point Pin1Pos { get; set; }
        
        public Point Pin2Pos { get; set; }
        
        public bool IsHighlighted { get; set; } 
    }
}