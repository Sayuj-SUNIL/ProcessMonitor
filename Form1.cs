using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace ProcessMonitor
{
    public partial class FormProcessMonitor : Form
    {
        public FormProcessMonitor()
        {
            InitializeComponent();
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(ProcessStarted);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(ProcessStopped);
            stopWatch.Start();
        }


        private static void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            Program.Log = DateTime.Now.ToString("hh:MM:ss tt") +e.NewEvent.Properties["ProcessName"].Value + Environment.NewLine + Program.Log;
        }
        private static void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            Program.Log = DateTime.Now.ToString("hh:MM:ss tt") + e.NewEvent.Properties["ProcessName"].Value + Environment.NewLine + Program.Log;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void FormProcessMonitor_Load(object sender, EventArgs e)
        {

        }

        private void RefreshList()
        {
            Process[] processes = Process.GetProcesses();
            listBox1.Items.Clear();
            foreach (Process process in processes)
            {
                listBox1.Items.Add(string.Format("{0}-{1}", process.Id, process.ProcessName));
            }
            txtLog.Text = Program.Log;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
