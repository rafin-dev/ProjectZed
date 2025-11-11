using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ProjectZed
{
    /// <summary>
    /// Interaction logic for FileEntry.xaml
    /// </summary>
    public partial class FileEntry : UserControl
    {
        public FileEntry()
        {
            InitializeComponent();
        }

        [Description("File name relative to its folder"), Category("Data")]
        public string FileName
        {
            get => FileLabel.Content.ToString();
            set => FileLabel.Content = value;
        }
    }
}
