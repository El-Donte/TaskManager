using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskManagerModel tmModel;
        CheckBox check;
        ProcessesListItem processListItem;
        OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
            dialog.DefaultExt = ".exe";
            dialog.Filter = "Exe файлы (.exe)|*.exe";
            Priorities.ItemsSource = Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>();
            Priorities.SelectedIndex = 0;
        }

        private void killButton_Click(object sender, RoutedEventArgs e)
        {
            tmModel = (TaskManagerModel)DataContext;
            tmModel.killSelectedProcesses();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            check = (CheckBox)sender;
            processListItem = (ProcessesListItem)check.DataContext;
            processListItem.Checked = true;
        }
        private void CheckBox_unChecked(object sender, RoutedEventArgs e)
        {
            check = (CheckBox)sender;
            processListItem = (ProcessesListItem)check.DataContext;
            processListItem.Checked = false;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            tmModel = (TaskManagerModel)DataContext;
            tmModel.updateProcesses(sender,e);
        }

        private void openNewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            bool? result = dialog.ShowDialog();
            if (result == true) 
            {
                string filename = dialog.FileName;
                Process.Start(filename);
            }
        }

        private void ProcessesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var listBox = (ListBox)sender;
                Process sProcess = null;
                if (listBox.SelectedItems.Count > 0)
                {
                    sProcess = ((ProcessesListItem)listBox.SelectedItems[0]).Process;
                    tmModel = (TaskManagerModel)DataContext;
                    tmModel.sProcess = sProcess;
                }
                LabelId.Content = sProcess.Id;
                LabelName.Content = sProcess.ProcessName;
                LabelNonpMemory.Content = sProcess.NonpagedSystemMemorySize64;
                LabelPagedMemory.Content = sProcess.PagedMemorySize64;
                LabelPriority.Content = sProcess.PriorityClass;
                LabelPrivateMemory.Content = sProcess.PrivateMemorySize64;
                LabelStartTime.Content = sProcess.StartTime;
                LabelThreadsNum.Content = sProcess.Threads.Count;
                LabelVirtualMemory.Content = sProcess.VirtualMemorySize64;
            }
            catch(Exception ex)
            {
                LabelPriority.Content = " ";
                LabelPrivateMemory.Content = " ";
                LabelStartTime.Content = " ";
                LabelThreadsNum.Content = " ";
            }

        }

        private void PrioritiesButton_Click(object sender, RoutedEventArgs e)
        {
            var priority = (ProcessPriorityClass)Priorities.SelectionBoxItem;
            tmModel = (TaskManagerModel)DataContext;
            tmModel.ChangePriority(priority);
            LabelPriority.Content = priority;
        }
    }
}
