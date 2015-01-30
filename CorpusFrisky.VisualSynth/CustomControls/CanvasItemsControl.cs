using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CorpusFrisky.VisualSynth.CustomControls
{
    public class CanvasItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(
                       DependencyObject element,
                       object item)
        {
            Binding leftBinding = new Binding() { Path = new PropertyPath("DesignPos.X") };
            Binding bottomBinding = new Binding() { Path = new PropertyPath("DesignPos.Y") };

            FrameworkElement contentControl = (FrameworkElement)element;
            contentControl.SetBinding(Canvas.LeftProperty, leftBinding);
            contentControl.SetBinding(Canvas.BottomProperty, bottomBinding);

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}