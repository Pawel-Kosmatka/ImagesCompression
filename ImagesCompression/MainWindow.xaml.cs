using ImagesCompression.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace ImagesCompression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        private void FileLoadButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "BMP File | *.bmp;";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() ?? false)
            {
                vm.SourceFilePath = fileDialog.FileName;
            }
        }
    }
}
