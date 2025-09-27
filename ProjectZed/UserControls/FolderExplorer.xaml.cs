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
        }

        public void SetFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                MessageBox.Show("Folder does not exist", "Error opening folder", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenFolderButton.Visibility = Visibility.Collapsed;

            FolderLabel.Content = Path.GetDirectoryName(folderName);

            foreach (string? file in Directory.GetFiles(folderName))
            {
                Label label = new Label();

                string filename = Path.GetFileName(file);
                m_FileNameToPath[filename] = file;

                label.Content = filename;
                label.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("AliceBlue");
                label.MouseDoubleClick += (e, i) => { MainWindow.GetInstance().OpenFile(m_FileNameToPath[(e as Label)?.Content as string]); };

                FileStackPanel.Children.Add(label);
            }
        }
        
        private void OpenFolderButtonClickedEventHandler(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().OpenFolder();
        }
    }
}
