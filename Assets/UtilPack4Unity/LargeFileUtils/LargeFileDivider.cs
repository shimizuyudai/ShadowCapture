using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace LargeFileUtils
{
    public static class LargeFileDivider
    {
        //public event Action<string> OnProgress;
        public static async Task<string> Divide(string filePath, string saveDirectory, int maxLineNUM, long offset = 0, long step = 1, long num = long.MaxValue, Action<string> onProgress = null)
        {
            var infoFilePath = string.Empty;
            step = Math.Max(1, step);
            await Task.Run(
             () =>
                {
                    var prefix = Path.GetFileNameWithoutExtension(filePath) + "_";
                    var extension = Path.GetExtension(filePath);

                    var fileNames = new List<string>();
                    var lineCount = 0;
                    var fileNumber = 0L;
                    var totalLineCount = 0L;
                    var offsetCount = 0L;
                    var count = 0L;
                    onProgress?.Invoke("start divide.");
                    using (var streamReader = new StreamReader(filePath))
                    {
                        var fileName = prefix + fileNumber + extension;
                        var path = Path.Combine(saveDirectory, fileName);
                        fileNames.Add(fileName);
                        var streamWriter = new StreamWriter(path);
                        var lines = new List<string>();
                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();

                            //offsetによる制限
                            if (offsetCount >= offset)
                            {
                                //stepによる制限
                                if (count % step == 0)
                                {
                                    streamWriter.WriteLine(line);
                                    lineCount++;
                                    totalLineCount++;
                                    if (lineCount >= maxLineNUM)
                                    {
                                        //File.WriteAllLines(path, lines);
                                        onProgress?.Invoke("complete write lines. file : " + path);
                                        //ready next file.
                                        fileNumber++;
                                        fileName = prefix + fileNumber + extension;
                                        fileNames.Add(fileName);
                                        path = Path.Combine(saveDirectory, fileName);
                                        streamWriter.Dispose();
                                        streamWriter = new StreamWriter(path);
                                        //lines = new List<string>();
                                        lineCount = 0;
                                    }
                                }
                                count++; 
                            }
                            //lines.Add(line);

                            offsetCount++;
                            //numによる制限
                            if (totalLineCount >= num)
                            {
                                break;
                            }
                        }
                        streamWriter.Dispose();
                        if (lineCount < 1)
                        {
                            File.Delete(path);
                            fileNames.RemoveAt(fileNames.Count - 1);
                        }
                    }

                    var info = new DivideInfo { MaxLineNUM = maxLineNUM, FileNames = fileNames, TotalLineNUM = totalLineCount, Prefix = prefix, Extension = extension };
                    infoFilePath = Path.Combine(saveDirectory, prefix + "divideInfo.json");
                    SaveInfo(infoFilePath, info);
                    onProgress?.Invoke("complete divide. infofile : " + infoFilePath);
                }
            );
            return infoFilePath;
        }

        private static void SaveInfo(string path, DivideInfo info)
        {
            var json = JsonConvert.SerializeObject(info, Formatting.Indented);
            File.WriteAllText(path, json);
        }


    }

    public class DivideInfo
    {
        public long TotalLineNUM;
        public int MaxLineNUM;
        public string Prefix;
        public string Extension;
        public List<string> FileNames;
    }


}

