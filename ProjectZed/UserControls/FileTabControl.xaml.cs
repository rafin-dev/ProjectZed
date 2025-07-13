using System.IO;
using System.Windows.Controls;

namespace ProjectZed
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class FileTabControl : UserControl
    {
        public static void OpenFile(string path, TabControl tabControl)
        {
            var t = new FileTabControl(path,  tabControl);
        }

        public static void CreateEmpty(TabControl tabControl)
        {
            var t = new FileTabControl(tabControl);
        }

        private FileTabControl(string filePath, TabControl tabControl)
        {
            InitializeComponent();

            if (File.Exists(filePath))
            {
                m_FilePath = filePath;
                TextBox.Text = File.ReadAllText(m_FilePath);
                m_TabItem.Header = new TabHeader(Path.GetFileName(m_FilePath), tabControl);
            }
            else
            {
                m_TabItem.Header = new TabHeader("Unsaved File", tabControl);
            }

            AttachToTabControl(tabControl);
        }

        private FileTabControl(TabControl tabControl)
        {
            InitializeComponent();

            m_TabItem.Header = new TabHeader("Unsaved File", tabControl);

            AttachToTabControl(tabControl);
        }

        private void AttachToTabControl(TabControl tabControl)
        {
            m_TabItem.Content = this;

            tabControl.Items.Add(m_TabItem);
            tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        public void SaveFile()
        {
            if (File.Exists(m_FilePath))
            {
                File.WriteAllText(m_FilePath, TextBox.Text);
            }
            else
            {
                SaveFileAs();
            }
        }

        public void SaveFileAs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                m_FilePath = dialog.FileName;
                if (File.Exists(m_FilePath))
                {
                    File.WriteAllText(m_FilePath, TextBox.Text);
                }
            }

            m_TabItem.Header = new TabHeader(Path.GetFileName(dialog.FileName), m_TabItem.Tag as TabControl);
        }

        public string GetText()
        {
            return TextBox.Text;
        }

        public string GetFilePath()
        {
            return m_FilePath;
        }


        private string m_FilePath = string.Empty;
        private TabItem m_TabItem = new TabItem();
    }
}
