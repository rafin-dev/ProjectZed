using System.IO;
using System.Windows.Controls;

namespace ProjectZed
{
    /// <summary>
    /// Interaction logic for TabHeader.xaml
    /// </summary>
    public partial class TabHeader : UserControl
    {
        public TabHeader(string title, TabControl tabControl)
        {
            InitializeComponent();

            m_TabControl = tabControl;

            m_Title = title;
            TabHeaderLabel.Content = title;
        }

        private string m_Title = string.Empty;
        private TabControl m_TabControl;

        private void CloseButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = 0;
            foreach (TabItem tab in m_TabControl.Items)
            {
                FileTabControl? control = tab.Content as FileTabControl;
                if (Path.GetFileName(control?.GetFilePath()) == m_Title)
                {
                    break;
                }
                index++;
            }

            m_TabControl.Items.RemoveAt(index);
        }
    }
}
