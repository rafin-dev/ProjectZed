using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectZed
{

    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class FileTabControl : UserControl
    {
        public static void OpenFile(string path, TabControl tabControl)
        {
            var t = new FileTabControl(path, tabControl);
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
                FileTextBox.Document.Blocks.Clear();
                FileTextBox.AppendText(File.ReadAllText(m_FilePath));
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
            m_TabItem.HeaderTemplate = tabControl.ItemTemplate;

            tabControl.Items.Add(m_TabItem);
            tabControl.SelectedIndex = tabControl.Items.Count - 1;

            m_Highlighter = KeywordHighlighter.CreateHighliter(Path.GetExtension(m_FilePath));

            m_DispatcherTimer.Tick += (o, e) =>
            {
                SaveCursor();

                // Change Text
                {
                    var nText = m_Highlighter.HighlightKeywords(GetText());

                    // Used to know what empty lines are actually part of the file
                    int emptyCount = 0;
                    foreach (var block in FileTextBox.Document.Blocks)
                    {
                        if ((new TextRange(block.ContentStart, block.ContentEnd).Text == string.Empty))
                        {
                            emptyCount++;
                        }
                    }

                    List<Block?> deleteBlocks = new();
                    FileTextBox.Document.Blocks.Clear();
                    foreach (var block in nText)
                    {
                        AppendText(FileTextBox, block.Text, block.Color);
                    }

                    // For some reason, the document gains new empty lines after changing colors, so we remove them
                    int nEmpCount = 0;
                    foreach (var block in FileTextBox.Document.Blocks)
                    {
                        var text = (new TextRange(block.ContentStart, block.ContentEnd).Text);
                        if (text == string.Empty)
                        {
                            nEmpCount++;

                            if (nEmpCount > emptyCount)
                            { 
                                deleteBlocks.Add(block);
                            }
                        }
                    }
                    foreach (var block in deleteBlocks)
                    {
                        FileTextBox.Document.Blocks.Remove(block);
                    }
                }

                RestoreCursor();
            };

            m_DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            m_DispatcherTimer.Start();
        }

        string _preText = "";
        private void SaveCursor()
        {
            _preText = new TextRange(FileTextBox.Document.ContentStart, FileTextBox.CaretPosition).Text;
        }
        private void RestoreCursor()
        {
            var startPos = FileTextBox.Document.ContentStart;
            var newPos = FileTextBox.Document.ContentStart;
            string _postText = "";
            while (newPos != null)
            {
                _postText = new TextRange(startPos, newPos).Text;
                if (_preText == _postText)
                    break;

                newPos = newPos.GetNextContextPosition(LogicalDirection.Forward);
            }
            FileTextBox.CaretPosition = newPos;
        }

        public void SaveFile()
        {
            if (File.Exists(m_FilePath))
            {
                File.WriteAllText(m_FilePath, GetText().TrimEnd());
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
                    File.WriteAllText(m_FilePath, GetText().TrimEnd());
                }
            }

            m_TabItem.Header = new TabHeader(Path.GetFileName(dialog.FileName), m_TabItem.Tag as TabControl);
        }

        public string GetText()
        {
            TextRange range = new TextRange(
                FileTextBox.Document.ContentStart,
                FileTextBox.Document.ContentEnd
                );

            return range.Text;
        }

        public string GetFilePath()
        {
            return m_FilePath;
        }

        private string m_FilePath = string.Empty;
        private TabItem m_TabItem = new TabItem();
        private KeywordHighlighter m_Highlighter = new LuaHighlighter();
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();

        private void Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            string text = GetText();

            var result = m_Highlighter.HighlightKeywords(text);

            FileTextBox.Document.Blocks.Clear();
            foreach (var t in result)
            {
                AppendText(FileTextBox, t.Text, t.Color);
            }
        }

        private static void AppendText(RichTextBox box, string text, string color)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                    bc.ConvertFrom(color));
            }
            catch (FormatException) 
            {
            }
        }
    }
}