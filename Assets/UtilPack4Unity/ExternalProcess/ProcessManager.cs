using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace UtilPack4Unity
{
    public class ProcessManager : MonoBehaviour
    {
        public enum InitMode
        {
            SettingsFile,
            Inspector
        }
        [SerializeField]
        bool playOnAwake;
        public InitMode initMode;
        [SerializeField]
        string settingsFileName;
        [SerializeField]
        ProcessSettings settings;



        public delegate void RedirectProcessMessage(ProcessMessage processMessage);
        public event RedirectProcessMessage RedirectOutputEvent;
        public event RedirectProcessMessage RedirectErrorEvent;

        public event RedirectProcessMessage AsyncRedirectOutputEvent;
        public event RedirectProcessMessage AsyncRedirectErrorEvent;

        public delegate void OnExitProcess(ProcessController processController);
        public event OnExitProcess ExitProessEvent;
        public event OnExitProcess AsyncExitProessEvent;

        private Queue outputMessageQueue, errorMessageQueue, processExitQueue;

        public ProcessController ProcessController;

        bool isPlaying;

        private void Awake()
        {
            if (playOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            outputMessageQueue = new Queue();
            outputMessageQueue = Queue.Synchronized(outputMessageQueue);
            errorMessageQueue = new Queue();
            errorMessageQueue = Queue.Synchronized(errorMessageQueue);
            processExitQueue = new Queue();
            processExitQueue = Queue.Synchronized(processExitQueue);

            switch (initMode)
            {
                case InitMode.SettingsFile:
                    this.settings = loadSettings();
                    break;

                case InitMode.Inspector:

                    break;
            }

            Execute(settings);
        }

        // Use this for initialization
        void Start()
        {
            //print(Path.GetFullPath("cmd.exe"));
        }

        public bool Execute(ProcessSettings settings)
        {
            Dispose();
            ProcessController = new ProcessController();

            if (settings.IsEnableRaisingEvents)
            {
                ProcessController.ExitProessEvent += ProcessController_ExitProessEvent;
            }
            if (settings.IsRedirectStandardOutput)
            {
                ProcessController.RedirectOutputEvent += ProcessController_RedirectOutputEvent;
            }
            if (settings.IsRedirectStandardError)
            {
                ProcessController.RedirectErrorEvent += ProcessController_RedirectErrorEvent; ;
            }
            var result = ProcessController.Execute(settings);

            return result;
        }

        private void ProcessController_RedirectErrorEvent(ProcessMessage processMessage)
        {
            if (RedirectErrorEvent != null) errorMessageQueue.Enqueue(processMessage);
            if (AsyncRedirectErrorEvent != null) AsyncRedirectErrorEvent(processMessage);
        }

        private void ProcessController_RedirectOutputEvent(ProcessMessage processMessage)
        {
            if (RedirectOutputEvent != null) outputMessageQueue.Enqueue(processMessage);
            if (AsyncRedirectOutputEvent != null) AsyncRedirectOutputEvent(processMessage);
        }

        private void ProcessController_ExitProessEvent(ProcessController controller)
        {
            if (ExitProessEvent != null) processExitQueue.Enqueue(controller);
            if (AsyncExitProessEvent != null) AsyncExitProessEvent(controller);
        }

        private void Update()
        {
            if (!isPlaying) return;

            while (outputMessageQueue.Count > 0)
            {
                var msg = outputMessageQueue.Dequeue() as ProcessMessage;
                if (RedirectOutputEvent != null) RedirectOutputEvent(msg);
            }

            while (errorMessageQueue.Count > 0)
            {
                var msg = errorMessageQueue.Dequeue() as ProcessMessage;
                if (RedirectErrorEvent != null) RedirectErrorEvent(msg);
            }

            while (processExitQueue.Count > 0)
            {
                var info = processExitQueue.Dequeue() as ProcessController;
                if (ExitProessEvent != null) ExitProessEvent(info);
            }
        }

        public void WriteLine(string line)
        {
            ProcessController.WriteLine(line);
        }

        ProcessSettings loadSettings()
        {
            var settings = new ProcessSettings();
            if (string.IsNullOrEmpty(settingsFileName)) return settings;
            var path = Path.Combine(Application.streamingAssetsPath, settingsFileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                settings = JsonConvert.DeserializeObject<ProcessSettings>(json);
            }
            return settings;
        }

        private void Dispose()
        {
            if (ProcessController != null)
            {
                ProcessController.Dispose();
            }
        }

        private void OnDestroy()
        {
            Dispose();
            ExitProessEvent = null;
            RedirectErrorEvent = null;
            RedirectOutputEvent = null;
            AsyncExitProessEvent = null;
            AsyncRedirectErrorEvent = null;
            AsyncRedirectOutputEvent = null;

        }

        [ContextMenu("ExportSettings")]
        void exportSettings()
        {
            if (string.IsNullOrEmpty(settingsFileName)) return;
            var path = Path.Combine(Application.streamingAssetsPath, settingsFileName);
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
