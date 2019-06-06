using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;

namespace UtilPack4Unity
{
    public static class ProcessUtils
    {
        //public static ProcessSettings CreateProcessSettings(string fileName, string args, bool isUseShellExecute, bool isRedirectStandardOutput, bool isRedirectStandardError,
        //   bool isRedirectStandardInput, bool isCreateNoWindow, bool isEnableRaisingEvents)
        //{
        //    var settings = new ProcessSettings();
        //    settings.FilePath = fileName;
        //    settings.Arguments = args;
        //    settings.IsUseShellExecute = isUseShellExecute;
        //    settings.IsRedirectStandardInput = isRedirectStandardInput;
        //    settings.IsRedirectStandardOutput = isRedirectStandardOutput;
        //    settings.IsRedirectStandardError = isRedirectStandardError;
        //    settings.IsCreateNoWindow = isCreateNoWindow;
        //    settings.IsEnableRaisingEvents = isEnableRaisingEvents;
        //    return settings;
        //}


        //public static Process CreateProcess(string fileName, string args, bool isUseShellExecute, bool isRedirectStandardOutput, bool isRedirectStandardError,
        //   bool isRedirectStandardInput, bool isCreateNoWindow, bool isEnableRaisingEvents)
        //{
        //    return CreateProcess(CreateProcessSettings(
        //        fileName,args,isUseShellExecute, isRedirectStandardOutput, isRedirectStandardError, isRedirectStandardInput, isCreateNoWindow, isEnableRaisingEvents
        //        ));
        //}

        public static Process CreateProcess(ProcessSettings settings)
        {
            var process = new Process();


            process.StartInfo.FileName = settings.FileName;
            process.StartInfo.WorkingDirectory = settings.WorkingDirectory;
            process.StartInfo.Arguments = settings.Arguments;
            process.StartInfo.UseShellExecute = settings.IsUseShellExecute;
            process.StartInfo.RedirectStandardInput = settings.IsRedirectStandardInput;
            process.StartInfo.RedirectStandardOutput = settings.IsRedirectStandardOutput;
            process.StartInfo.RedirectStandardError = settings.IsRedirectStandardError;
            process.StartInfo.CreateNoWindow = settings.IsCreateNoWindow;
            process.StartInfo.Verb = settings.Verb;
            if (!string.IsNullOrEmpty(settings.WorkingDirectory))
            {
                process.StartInfo.WorkingDirectory = settings.WorkingDirectory;
            }
            process.EnableRaisingEvents = settings.IsEnableRaisingEvents;
            return process;
        }

        public static string GetProcessName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static void SaveCommandAsBatch(string path, List<string> commands)
        {
            var command = CreateCommandLine(commands);
            SaveCommandAsBatch(path, command);
        }

        public static void SaveCommandAsBatch(string path, string command)
        {
            File.WriteAllText(path, command);
        }

        public static string CreateCommandLine(List<string> commands)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < commands.Count; i++)
            {
                if (i == 0)
                {
                    sb.AppendLine(commands[i] + "^");
                }
                else
                {
                    sb.AppendLine("&" + commands[i] + "^");
                }
            }
            return sb.ToString();
        }

        public static ProcessSettings CreateSettings()
        {
            var settings = new ProcessSettings();
            settings.FileName = string.Empty;
            settings.Arguments = string.Empty;
            settings.WorkingDirectory = string.Empty;
            settings.IsCreateNoWindow = true;
            settings.IsUseShellExecute = false;
            settings.IsRedirectStandardError = true;
            settings.IsRedirectStandardInput = true;
            settings.IsRedirectStandardOutput = true;
            settings.IsEnableRaisingEvents = true;
            settings.IsRelativePath = false;
            settings.IsCommand = false;
            settings.IsStreamingAssets = false;
            settings.Verb = string.Empty;
            return settings;
        }
        //public string RelativeToAbsolute(string path)
        //{
        //    var result = path.Clone() as string;
        //    //相対パスなら絶対パスに変換する
        //    if (!Path.IsPathRooted(result))
        //    {
        //        Path.GetFullPath(result);
        //    }
        //    return result;
        //}
    }

    public class ProcessMessage
    {
        public ProcessController Controller;
        public string Arguments;

        public ProcessMessage(ProcessController controller, string arguments)
        {
            this.Controller = controller;
            this.Arguments = arguments;
        }
    }



    [Serializable]
    public class ProcessSettings
    {
        public string FileName;
        public string Arguments;
        public string WorkingDirectory;
        public bool IsUseShellExecute;
        public bool IsRedirectStandardInput;
        public bool IsRedirectStandardOutput;
        public bool IsRedirectStandardError;
        public bool IsCreateNoWindow;
        public bool IsEnableRaisingEvents;
        public bool IsRelativePath;
        public bool IsStreamingAssets;
        public bool IsCommand;
        public string Verb;

    }
}

