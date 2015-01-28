namespace CorpusFrisky.VisualSynth.Modules
{
    public interface ISynthModule
    {
        void PreRender();
        void Render();
        void PostRender();
    }
}