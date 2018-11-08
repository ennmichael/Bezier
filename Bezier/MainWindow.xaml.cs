using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bezier
{
    public partial class MainWindow : Window
    {
        private int steps = 0;
        private readonly ICurve[] curves;
        private readonly WeightsController[] weightsControllers;

        public MainWindow()
        {
            InitializeComponent();
            curves = new ICurve[]
            {
                new QuarticCurve(
                    new Vector2(600.0f, 350.0f),
                    new Vector2(700.0f, 450.0f),
                    new Vector2(500.0f, 550.0f),
                    new Vector2(550.0f, 400.0f),
                    new Vector2(420.0f, 450.0f)),
                new CubicCurve(
                    new Vector2(110.0f, 410.0f),
                    new Vector2(120.0f, 480.0f),
                    new Vector2(120.0f, 600.0f),
                    new Vector2(200.0f, 500.0f)),
                new QuadraticCurve(
                    new Vector2(150.0f, 150.0f),
                    new Vector2(100.0f, 300.0f),
                    new Vector2(350.0f, 150.0f)),
                new LinearCurve(
                    new Vector2(700.0f, 100.0f),
                    new Vector2(510.0f, 160.0f))
            };
            weightsControllers = curves.Select(c => new WeightsController(c)).ToArray();
            RedrawCanvas();
        }

        private void RedrawCanvas()
        {
            Drawing.Clear(MainCanvas);
            DrawCurves();
            try
            {
                DrawIntersections();
            }
            catch (NotImplementedException)
            { }
        }

        private void DrawCurves()
        {
            foreach (var curve in curves)
                Drawing.DrawEverything(MainCanvas, curve, steps);
        }

        private void DrawIntersections()
        {
            int i = 1;
            foreach (var curve in curves)
            {
                foreach (var otherCurve in curves.Skip(i))
                    Drawing.DrawIntersections(MainCanvas, curve, otherCurve);
                i += 1;
            }
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
