using Backup.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Retention.Service
{
    public partial class RetentionService : ServiceBase, IService
    {

        private IRetentionServiceFactory ServiceFactory { get; }

        private Thread MainThread { get; set; }

        public RetentionService(IRetentionServiceFactory serviceFactory)
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
            List<Task> taskList = new List<Task>();
            foreach (IRetention retention in ServiceFactory.Retentions)
            {
                taskList.Add(Task.Factory.StartNew(() => retention.KeepAndClearBackups()));
            }
            Task.WaitAll(taskList.ToArray());
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            List<Task> taskList = new List<Task>();
            foreach(IRetention retention in ServiceFactory.Retentions)
            {
                taskList.Add(Task.Factory.StartNew(() => retention.KeepAndClearBackups()));
            }
            Task.WaitAll(taskList.ToArray());
        }

        protected override void OnStop()
        {
        }
    }
}