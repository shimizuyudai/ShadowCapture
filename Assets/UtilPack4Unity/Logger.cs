using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
namespace UtilPack4Unity
{
    public static class Logger
    {

        public static void Write(string text, string fileName = "", bool isClear = false)
        {
            var directory = Application.streamingAssetsPath + "/Log";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "log";
            }
            fileName += DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt";
            var timeStamp = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            var path = Path.Combine(directory, fileName);
            var log = timeStamp + "---" + text + "\n";
            if (File.Exists(path) && !isClear)
            {
                File.AppendAllText(path, log);
            }
            else
            {
                File.WriteAllText(path, log);
            }
        }
    }
}