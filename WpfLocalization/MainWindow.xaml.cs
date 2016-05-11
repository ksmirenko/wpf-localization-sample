using System.Windows;

namespace WpfLocalization {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e) {
            App.SwitchCulture();
        }
    }
}
