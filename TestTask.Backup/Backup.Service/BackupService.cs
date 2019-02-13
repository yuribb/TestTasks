using Backup.Core.Interfaces;
using System;
using System.ServiceProcess;
using System.Threading;

namespace Backup.Service
{
    public partial class BackupService : ServiceBase, IService
    {
        private IBackupServiceFactory ServiceFactory { get; }

        private Thread MainThread { get; set; }

        public BackupService(IBackupServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MainThread = new Thread(new ThreadStart(ThreadProc));
            MainThread.IsBackground = true;
            MainThread.Start();
        }

        private void ThreadProc()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void ManualRun()
        {
            ServiceFactory.Backup.Backup();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            ServiceFactory.Backup.Backup();
        }

        protected override void OnStop()
        {
        }
    }
}