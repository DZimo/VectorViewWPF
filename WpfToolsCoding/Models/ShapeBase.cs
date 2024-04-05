using ReactiveUI;
using System.Windows.Media;

namespace WpfToolsCoding.Models
{
    public class ShapeBase : ReactiveObject
    {
        public string? Type { get; set; }

        public bool Filled { get; set; }

        public string Color { get; set; }

        public Color ARGBcolor { get; set; }

        public Brush BrushColor { get; set; }


    }
}
