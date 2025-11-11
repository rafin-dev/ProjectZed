using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectZed
{
    /// <summary>
    /// Interaction logic for FolderExplorer.xaml
    /// </summary>
    public partial class FolderExplorer : UserControl
    {
        private Dictionary<string, string> m_FileNameToPath = new Dictionary<string, string>();

        public FolderExplorer()
        {
            InitializeComponent();

            SetFolder("TestProject");
        }

        public void SetFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                MessageBox.Show("Folder does not exist", "Error opening folder", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenFolderButton.Visibility = Visibility.Collapsed;

            FolderLabel.Content = "TestProject";

            foreach (string? file in Directory.GetFiles(folderName))
            {
                FileEntry label = new();

                string filename = Path.GetFileName(file);
                m_FileNameToPath[filename] = file;

                label.FileName = filename;
                FileStackPanel.Children.Add(label);
            }
        }
        
        private void OpenFolderButtonClickedEventHandler(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().OpenFolder();
        }
    }
}
