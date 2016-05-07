namespace CorpusFrisky.VisualSynth.Common
{
    public static class RegionNames
    {
        public const string LeftControlRegion = "LeftControlRegion";
        public const string RightControlRegion = "RightControlRegion";
        public const string UpperControlRegion = "UpperControlRegion";
        public const string LowerControlRegion = "LowerControlRegion";
    }

    public enum SynthModuleType
    {
        Output,

        TriangleGenerator,
        RectangleGenerator,

        Summer,

        ColorInverter,

        Oscillator,

        Unknown
    }

    public static class Constants
    {
        public const float FrameRate = 30.0f;
    }
}
