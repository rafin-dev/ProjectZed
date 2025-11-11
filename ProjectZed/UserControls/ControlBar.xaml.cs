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
using static System.Net.Mime.MediaTypeNames;

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

        private static Process? m_Process = null;

        private void RunOnClick(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void ProcessExited(object sender, System.EventArgs e)
        {
            this.Dispatcher.Invoke(new Action<UIElement, Visibility>(SetVisibility), RunButtonStack, Visibility.Visible);
            this.Dispatcher.Invoke(new Action<UIElement, Visibility>(SetVisibility), StopButton, Visibility.Collapsed);
        }

        private void SetVisibility(UIElement element, Visibility visibility)
        {
            element.Visibility = visibility;
        }

        private void Run()
        {
            if (m_Process != null && !m_Process.HasExited) { return; }

            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = "TestProject\\DummyCode.lua";
            info.FileName = "Lua\\Lua.exe";

            info.UseShellExecute = true;

            m_Process = Process.Start(info);

            if (m_Process != null)
            {
                m_Process.EnableRaisingEvents = true;
                m_Process.Exited += new EventHandler(ProcessExited);

                RunButtonStack.Visibility = Visibility.Collapsed;
                StopButton.Visibility = Visibility.Visible;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                Run();
            }
        }

        private void StopOnClick(object sender, RoutedEventArgs e)
        {
            if (m_Process != null)
            {
                m_Process.Kill();
            }
        }
    }
}
