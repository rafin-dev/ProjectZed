using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

            tabControl.Items.Add(m_TabItem);
            tabControl.SelectedIndex = tabControl.Items.Count - 1;
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

        private Dictionary<string, string> m_KeyWordToColor = new Dictionary<string, string>()
        {
            { "and",        "#ff0022" },
            { "break",      "#ff0022" },
            { "do",         "#ff0022" },
            { "else",       "#ff0022" },
            { "elseif",     "#ff0022" },
            { "end",        "#ff0022" },
            { "false",      "#ff0022" },
            { "for",        "#ff0022" },
            { "function",   "#ff0022" },
            { "if",         "#ff0022" },
            { "in",         "#ff0022" },
            { "local",      "#ff0022" },
            { "nil",        "#ff0022" },
            { "not",        "#ff0022" },
            { "or",         "#ff0022" },
            { "repeat",     "#ff0022" },
            { "return",     "#ff0022" },
            { "then",       "#ff0022" },
            { "true",       "#ff0022" },
            { "until",      "#ff0022" },
            { "while",      "#ff0022" }
        };

        struct TextPart
        {
            public TextPart(string text, string color, int index)
            {
                Text = text;
                Color = color;
                Index = index;
            }

            public string Text = string.Empty;
            public string Color = string.Empty;
            public int Index = 0;
        }

        private KeywordHighlighter m_Highlighter = new LuaHighlighter();

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