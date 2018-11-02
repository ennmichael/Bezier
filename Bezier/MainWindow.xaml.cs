using System.Windows;
using System.Windows.Controls;

namespace Bezier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem)ExampleSelection.SelectedItem;
            switch (item.Content)
            {
                case "Rough Draw Example":
                    Examples.RoughDrawExample(MainCanvas);
                    break;
            }
        }
    }
}
