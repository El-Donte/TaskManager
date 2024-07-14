using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class ProcessesListItem
    {
        public int Id => Process.Id;
        public string ProcessName => Process.ProcessName;
        public long ProcessMemory => Process.PrivateMemorySize64;
        public bool Checked { get; set; }
        public Process Process { get; }
        public string FileName { get; }
        public string Arguments { get; }

        public ProcessesListItem(Process process)
        {
            Process = process;
            FileName = process.StartInfo.FileName;
            Arguments = process.StartInfo.Arguments;
        }
    }
}
