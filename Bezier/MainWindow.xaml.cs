using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Bezier
{
    public class DrawingOptions : INotifyPropertyChanged
    {
        private bool drawExtrema;
        private bool drawBoundingBoxes;
        private bool drawIntersections;
        private bool drawSkeletons = true;
        private int precision = 30;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool DrawExtrema { get { return drawExtrema; } set { drawExtrema = value; OnPropertyChanged(); } }

        public bool DrawBoundingBoxes { get { return drawBoundingBoxes; } set { drawBoundingBoxes = value; OnPropertyChanged(); } }

        public bool DrawIntersections { get { return drawIntersections; } set { drawIntersections = value; OnPropertyChanged(); } }

        public bool DrawSkeletons { get { return drawSkeletons; } set { drawSkeletons = value; OnPropertyChanged(); } }

        public int Precision { get { return precision; } set { precision = value; OnPropertyChanged(); } }
    }

    public partial class MainWindow : Window
    {
        private readonly ICurve[] curves;
        private readonly WeightsController[] weightsControllers;
        private readonly DrawingOptions drawingOptions = new DrawingOptions();

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
                    new Vector2(150.0f, 400.0f),
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
            drawingOptions.PropertyChanged += (sender, eventArgs) => RedrawCanvas();
            RedrawCanvas();
        }

        private void RedrawCanvas()
        {
            MainCanvas.Clear();
            if (drawingOptions.DrawBoundingBoxes)
                DrawBoundingBoxes();
            if (drawingOptions.DrawSkeletons)
                DrawSkeletons();
            DrawWeights();
            DrawCurves();
            if (drawingOptions.DrawExtrema)
                DrawExtrema();
            if (drawingOptions.DrawIntersections)
                DrawIntersections();
        }

        private void DrawBoundingBoxes()
        {
            foreach (var curve in curves)
                MainCanvas.DrawBoundingBox(curve);
        }

        private void DrawSkeletons()
        {
            foreach (var curve in curves)
                MainCanvas.DrawSkeleton(curve);
        }

        private void DrawWeights()
        {
            foreach (var curve in curves)
                MainCanvas.DrawWeights(curve);
        }

        private void DrawCurves()
        {
            foreach (var curve in curves)
                MainCanvas.DrawCurve(curve, drawingOptions.Precision);
        }

        private void DrawExtrema()
        {
            foreach (var curve in curves)
                MainCanvas.DrawExtrema(curve);
        }

        private void DrawIntersections()
        {
            int i = 1;
            foreach (var curve in curves)
            {
                foreach (var otherCurve in curves.Skip(i))
                {
                    try
                    {
                        MainCanvas.DrawIntersections(curve, otherCurve);
                    }
                    catch (NotImplementedException)
                    {
                        // Some of these haven't been implemented yet
                    }
                }
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawingOptions.Precision = (int)e.NewValue;

        private void Button_Click(object sender, RoutedEventArgs e) => new Options(drawingOptions).Show();
    }
}
