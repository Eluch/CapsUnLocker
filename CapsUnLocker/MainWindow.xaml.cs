using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Forms;

namespace CapsUnLocker
{
    /// <summary>
    /// Automatics Caps Lock disabler
    /// </summary>
    public partial class MainWindow : Window
    {
        Worker worker;
        Thread thread;
        NotifyIcon ni;

        public MainWindow()
        {
            InitializeComponent();
            enableBox.Checked += enableBox_Checked;
            enableBox.Unchecked += enableBox_Unchecked;
            mainWindow.Closing += mainWindow_Closing;
            worker = new Worker();

            ni = new NotifyIcon();
            ni.Text = mainWindow.Name;
            ni.Icon = Properties.Resources.Robot;
            ni.Visible = true;

            mainWindow.StateChanged += mainWindow_StateChanged;
            ni.DoubleClick += ni_DoubleClick;
        }

        void ni_DoubleClick(object sender, EventArgs e)
        {
            if (mainWindow.WindowState == System.Windows.WindowState.Minimized)
            {
                mainWindow.Show();
                mainWindow.WindowState = System.Windows.WindowState.Normal;
                mainWindow.Focus();
            }
        }

        void mainWindow_StateChanged(object sender, EventArgs e)
        {
            if (mainWindow.WindowState == System.Windows.WindowState.Minimized)
                mainWindow.Hide();
        }

        void enableBox_Checked(object sender, RoutedEventArgs e)
        {
            thread = new Thread(worker.run);
            if (!thread.IsAlive)
                thread.Start();
        }

        void enableBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (thread.IsAlive)
            {
                worker.stopGracefully();
                thread.Join();
            }
        }

        void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ni.Visible = false;
            if (thread != null)
                if (thread.IsAlive)
                {
                    worker.stopGracefully();
                    thread.Join();
                }
        }
    }
}
