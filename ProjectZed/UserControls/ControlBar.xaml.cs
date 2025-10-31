using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    /// Interação lógica para ControlBar.xam
    /// </summary>
    public partial class ControlBar : UserControl
    {
        public ControlBar()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = "C:/Users/PC/Downloads/ProjectZed-main/ProjectZed-main/ProjectZed/DummyCode.lua";
            info.FileName = "lua\\Lua.exe";

            info.UseShellExecute = true;
            
            Process? p = Process.Start(info);
        }
    }
}
