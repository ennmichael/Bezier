using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bezier
{
    public partial class MainWindow : Window
    {
        private int steps = 0;
        private readonly CubicCurve cubicCurve;
        private readonly QuadraticCurve quadraticCurve;
        private readonly LinearCurve linearCurve;
        private readonly WeightsController[] weightsControllers;

        private IEnumerable<ICurve> Curves
        {
            get
            {
                yield return cubicCurve;
                yield return quadraticCurve;
                yield return linearCurve;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            cubicCurve = new CubicCurve(
                new Vector2(10.0f, 10.0f),
                new Vector2(20.0f, 80.0f),
                new Vector2(20.0f, 200.0f),
                new Vector2(100.0f, 100.0f));
            quadraticCurve = new QuadraticCurve(
                new Vector2(150.0f, 150.0f),
                new Vector2(100.0f, 300.0f),
                new Vector2(350.0f, 150.0f));
            linearCurve = new LinearCurve(
                new Vector2(500.0f, 200.0f),
                new Vector2(310.0f, 260.0f));
            weightsControllers = Curves
                .Select(c => new WeightsController(c))
                .ToArray();
            RedrawCanvas();
        }

        private void RedrawCanvas()
        {
            Drawing.Clear(MainCanvas);
            Drawcurves();
            DrawIntersections();
        }

        private void Drawcurves()
        {
            foreach (var curve in Curves)
                Drawing.DrawEverything(MainCanvas, curve, steps);
        }

        private void DrawIntersections()
        {
        }

        IEnumerable<Vector2> FindIntersections()
        {
            yield return new Vector2();
        }

        private Vector2 CanvasMousePosition => Mouse.GetPosition(MainCanvas).ToVector2();

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (var weightsController in weightsControllers)
                {
                    bool handled = weightsController.MouseDown(CanvasMousePosition);
                    if (handled)
                        break;
                }
                RedrawCanvas();
            }
            else
            {
                foreach (var weightsController in weightsControllers)
                    weightsController.MouseUp();
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            steps = (int)e.NewValue;
            if (IsInitialized)
                RedrawCanvas();
        }
    }
}
