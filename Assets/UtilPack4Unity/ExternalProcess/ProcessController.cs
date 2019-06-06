using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
namespace UtilPack4Unity
{
    public class ProcessController
    {
        public delegate void RedirectProcessMessage(ProcessMessage processMessage);
        public event RedirectProcessMessage RedirectOutputEvent;
        public event RedirectProcessMessage RedirectErrorEvent;

        public delegate void OnExitProcess(ProcessController processController);
        public event OnExitProcess ExitProessEvent;

        public Process TargetProcess
        {
            get;
            private set;
        }

        public int ProcessId
        {
            get;
            private set;
        }

        public string ProcessName
        {
            get;
            private set;
        }

        public ProcessController()
        {

        }

        public bool Execute(ProcessSettings settings)
        {
            if (settings.IsStreamingAssets)
            {
                settings.FileName = Path.Combine(Application.streamingAssetsPath, settings.FileName);
            }
            else
            {
                if (settings.IsRelativePath)
                {
                    settings.FileName = Path.GetFullPath(settings.FileName);
                }
            }

            if (!settings.IsCommand)
            {
                if (!File.Exists(settings.FileName))
                {
                    return false;
                }

            }

            TargetProcess = ProcessUtils.CreateProcess(settings);


            if (TargetProcess.EnableRaisingEvents)
            {
                TargetProcess.Exited += Process_Exited;
            }
            if (TargetProcess.StartInfo.RedirectStandardOutput)
            {
                TargetProcess.OutputDataReceived += Process_OutputDataReceived;
            }
            if (TargetProcess.StartInfo.RedirectStandardError)
            {
                TargetProcess.ErrorDataReceived += Process_ErrorDataReceived;
            }

            var result = TargetProcess.Start();

            if (result)
            {
                if (TargetProcess.StartInfo.RedirectStandardOutput)
                {
                    TargetProcess.BeginOutputReadLine();
                }
                if (TargetProcess.StartInfo.RedirectStandardError)
                {
                    TargetProcess.BeginErrorReadLine();
                }
                this.ProcessId = TargetProcess.Id;
                this.ProcessName = TargetProcess.ProcessName;
            }
            else
            {
                DisposeProcess();
            }

            return result;
        }

        public void WriteLine(string line)
        {
            TargetProcess.StandardInput.WriteLine(line);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var process = (Process)sender;
            var msg = new ProcessMessage(this, e.Data);
            if (RedirectErrorEvent != null) RedirectErrorEvent(msg);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var process = (Process)sender;
            var msg = new ProcessMessage(this, e.Data);
            if (RedirectOutputEvent != null) RedirectOutputEvent(msg);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            var process = (Process)sender;
            if (ExitProessEvent != null) ExitProessEvent(this);
        }

        public void DisposeProcess()
        {
            if (TargetProcess != null)
            {
                if (!TargetProcess.HasExited)
                {
                    TargetProcess.CancelErrorRead();
                    TargetProcess.CancelOutputRead();
                    TargetProcess.Kill();
                    TargetProcess.Dispose();
                }
                TargetProcess = null;
            }
        }

        public void Dispose()
        {
            DisposeProcess();
            ExitProessEvent = null;
            RedirectOutputEvent = null;
            RedirectErrorEvent = null;
        }
    }
}
