using System.Windows;

namespace Bezier
{
    public partial class Options : Window
    {
        public Options(DrawingOptions drawingOptions)
        {
            DataContext = drawingOptions;
            InitializeComponent();
        }
    }
}
