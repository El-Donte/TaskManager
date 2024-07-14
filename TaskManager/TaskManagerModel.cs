using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TaskManager
{
    internal class TaskManagerModel 
    {
        private List<ProcessesListItem>SelectedProcesses = new List<ProcessesListItem>();
        public ObservableCollection<ProcessesListItem> Processes { get; } = new ObservableCollection<ProcessesListItem>();

        private Process _sProcess;
        public Process sProcess
        {
            get => _sProcess;
            set
            {
                _sProcess = value;
            }
        }
        public TaskManagerModel()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Tick += updateProcesses;
            timer.Start();
        }

        public void updateProcesses(object sender, EventArgs e)
        {

            var currentProcesses = Processes.Select(process => process.Id).ToList();

            foreach (var process in Process.GetProcesses())
            {
                if (!currentProcesses.Remove(process.Id)) 
                {
                    Processes.Add(new ProcessesListItem(process));
                }
            }

            foreach (var id in currentProcesses) 
            {
                var process = Processes.First(p => p.Id == id);
                Processes.Remove(process);
            }
        }

        public void getSelectedProcesses()
        {
            foreach (var process in Processes)
            {
                if (process.Checked)
                {
                    SelectedProcesses.Add(process);
                }
            }
        }

        public void killSelectedProcesses()
        {
            try
            {
                getSelectedProcesses();
                foreach (var process in SelectedProcesses)
                {
                    process.Process.Kill();
                }
                SelectedProcesses.Clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message,"ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        public void ChangePriority(ProcessPriorityClass priority)
        {
            try
            {
                sProcess.PriorityClass = priority;

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
