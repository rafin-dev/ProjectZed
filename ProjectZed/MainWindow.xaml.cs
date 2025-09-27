using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectZed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow s_Instance;
        public static MainWindow GetInstance()
        {
            return s_Instance;
        }

        public MainWindow()
        {
            InitializeComponent();

            s_Instance = this;

            KeywordHighlighter.Init();
        }

        //private List<FileTab> m_FileTabs= new List<FileTab>();
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.O)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift))
                    {
                        OpenFolder();
                    }
                    else
                    {
                        OpenFile();
                    }
                }
            
                if (e.Key == Key.S)
                {
                    SaveFile();
                }
            }
        }

        public void OpenFolder()
        {
            var dialog = new OpenFolderDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                FolderExplorerControl.SetFolder(dialog.FolderName);
            }
        }

        public void OpenFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = string.Empty;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                OpenFile(dialog.FileName);
            }
        }
        public void OpenFile(string path)
        {
            foreach (TabItem tabItem in TabControl.Items)
            {
                FileTabControl? control = tabItem.Content as FileTabControl;
                if (control?.GetFilePath() == path)
                {
                    TabControl.SelectedIndex = TabControl.Items.IndexOf(tabItem);
                    return;
                }
            }

            FileTabControl.OpenFile(path, TabControl);
        }

        public void SaveFile()
        {
            (TabControl.SelectedContent as FileTabControl)?.SaveFile();
        }

        private void CloseTabEventHandler(object sender, MouseButtonEventArgs e)
        {

        }
    }
}