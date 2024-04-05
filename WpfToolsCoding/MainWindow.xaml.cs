using System.Windows;
using WpfToolsCoding.ViewModels;

namespace WpfToolsCoding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VmShape vmShape;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartParserOnClick(object sender, RoutedEventArgs e)
        {
            vmShape = new VmShape(Path.Text);
            this.DataContext = vmShape;
        }
    }
}