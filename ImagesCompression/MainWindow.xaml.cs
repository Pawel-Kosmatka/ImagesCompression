using ImagesCompression.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
