using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bezier
{
    public partial class MainWindow : Window
    {
        private int steps = 0;
        private readonly CubicBezierCurve cubicBezierCurve;
        private readonly QuadraticBezierCurve quadraticBezierCurve;
        private readonly LinearBezierCurve linearBezierCurve;
        private readonly WeightsController[] weightsControllers;

        private IEnumerable<IBezierCurve> BezierCurves
        {
            get
            {
                yield return cubicBezierCurve;
                yield return quadraticBezierCurve;
                yield return linearBezierCurve;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            cubicBezierCurve = new CubicBezierCurve(
                new Vector2(10.0f, 10.0f),
                new Vector2(20.0f, 80.0f),
                new Vector2(20.0f, 200.0f),
                new Vector2(100.0f, 100.0f));
            quadraticBezierCurve = new QuadraticBezierCurve(
                new Vector2(150.0f, 150.0f),
                new Vector2(200.0f, 250.0f),
                new Vector2(250.0f, 250.0f));
            linearBezierCurve = new LinearBezierCurve(
                new Vector2(500.0f, 200.0f),
                new Vector2(310.0f, 260.0f));
            weightsControllers = BezierCurves
                .Select(c => new WeightsController(c))
                .ToArray();
            RedrawCanvas();
        }

        private void RedrawCanvas()
        {
            BezierDrawing.Clear(MainCanvas);
            DrawBezierCurves();
            DrawIntersections();
        }

        private void DrawBezierCurves()
        {
            foreach (var bezierCurve in BezierCurves)
                BezierDrawing.DrawEverything(MainCanvas, bezierCurve, steps);
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
