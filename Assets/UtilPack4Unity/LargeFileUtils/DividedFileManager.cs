using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Concurrent;

namespace LargeFileUtils
{
    public class DividedFileManager
    {

        public DivideInfo Info
        {
            get;
            private set;
        }

        public string InfoFileDirectory
        {
            get;
            private set;
        }

        public string InfoFileName
        {
            get;
            private set;
        }

        public string InfoFilePath
        {
            get {
                return Path.Combine(InfoFileDirectory, InfoFileName);
            }
        }

        public DividedFileManager(string path)
        {
            Info = LoadDividedInfo(path);
            InfoFileDirectory = Path.GetDirectoryName(path);
            InfoFileName = Path.GetFileName(path);
            Clean();
        }

        public DividedFileManager(string path, int maxLineNUM, string prefix, string extension)
        {
            var info = LoadDividedInfo(path);
            var isExist = info != null;
            if (!isExist)
            {
                info = new DivideInfo
                {
                    TotalLineNUM = 0,
                    MaxLineNUM = maxLineNUM,
                    Prefix = prefix,
                    Extension = extension,
                    FileNames = new List<string>()
                };
            }

            Info = info;
            InfoFileDirectory = Path.GetDirectoryName(path);
            InfoFileName = Path.GetFileName(path);
            if (!isExist) SaveInfo(Info);
            Clean();
        }

        private DivideInfo LoadDividedInfo(string path)
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            try
            {
                var result = JsonConvert.DeserializeObject<DivideInfo>(json);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public string GetFileName(int fileNumber)
        {
            return Info.Prefix + fileNumber + Info.Extension;
        }

        public string GetFilePath(int fileNumber)
        {
            return Path.Combine(InfoFileDirectory, GetFileName(fileNumber));
        }

        public void SaveInfo(DivideInfo info)
        {
            var json = JsonConvert.SerializeObject(info, Formatting.Indented);
            File.WriteAllText(InfoFilePath, json);
        }

        public void Clean()
        {
            var fileNames = Info.FileNames;
            var files = Directory.GetFiles(InfoFileDirectory);
            var length = files.Length;
            var filePaths = new ConcurrentBag<string>();
            filePaths.Add(InfoFilePath);
            Parallel.For(0, fileNames.Count, i =>
            {
                filePaths.Add(Path.Combine(InfoFileDirectory, fileNames[i]));
            });

            Parallel.For(0, length, i =>
            {
                var extension = Path.GetExtension(files[i]);
                if (extension == Info.Extension && files[i].Contains(Info.Prefix))
                {
                    if (!filePaths.Contains(files[i]))
                    {
                        File.Delete(files[i]);
                    }
                }
            });

        }

        public async Task CleanAsync()
        {
            await Task.Run(() =>
            {
                Clean();
            });
        }

        //最終ファイルを取得し、書き加え、maxLine以上になったら新しいファイルに記述。またその旨をjsonにも書き加える
        public async Task AppendLines(string[] lines)
        {
            await Task.Run(
                async () =>
                {
                    var fileNumber = Info.FileNames.Count - 1;
                    fileNumber = Math.Max(0,fileNumber);
                    var filePath = GetFilePath(fileNumber);
                    var lastFileLines = await GetAllLinesFromFile(filePath);
                    var lineCount = File.Exists(filePath) ? lastFileLines.Length : 0;
                    if (lineCount >= Info.MaxLineNUM)
                    {
                        fileNumber++;
                        filePath = GetFilePath(fileNumber);
                        lineCount = 0;
                    }
                    
                    var list = new List<string>();
                    foreach (var line in lines)
                    {
                        list.Add(line);
                        lineCount++;
                        if (lineCount >= Info.MaxLineNUM)
                        {
                            File.AppendAllLines(filePath, list);
                            Info.TotalLineNUM += list.Count;
                            Info.FileNames.Add(Path.GetFileName(filePath));
                            SaveInfo(Info);
                            fileNumber++;
                            filePath = GetFilePath(fileNumber);
                            lineCount = 0;
                            list = new List<string>();
                        }
                    }

                    if (list.Count > 0)
                    {
                        File.AppendAllLines(filePath, list);
                        Info.TotalLineNUM += list.Count;
                        Info.FileNames.Add(Path.GetFileName(filePath));
                        SaveInfo(Info);
                    }
                }
                );
        }

        //指定したファイルのすべての行を取得する
        public async Task<string[]> GetAllLinesFromFile(string path)
        {
            var result = new string[0];
            if (!File.Exists(path)) return result;
            await Task.Run(
                () =>
                {
                    result = File.ReadAllLines(path);
                }
                );
            return result;
        }

        //指定したファイル番号のすべての行を取得する
        public async Task<string[]> GetAllLinesFromFile(int fileNumber)
        {
            var result = new string[0];
            if (fileNumber < 0 || fileNumber >= Info.FileNames.Count) return result;
            var path = GetFilePath(fileNumber);
            result = await GetAllLinesFromFile(path);
            return result;
        }

        //末尾から取得する
        public async Task<string[]> GetLinesFromLast(long offset, long num, long step = 1)
        {
            var end = Math.Max((Info.TotalLineNUM - 1) - offset, 0);
            var start = end - (num * step) + 1;
            start = Math.Max(start, 0);
            var lines = await GetLines(start, num, step);
            return lines;
        }

        //取得する行のオフセット、取得する行数、ステップを指定
        public async Task<string[]> GetLines(long offset, long num, long step = 1)
        {
            var start = Math.Max(0, offset);
            var end = offset + num * step - 1;


            step = Math.Max(step, 1);
            var result = new List<string>();
            await Task.Run(() =>
            {
                var info = Info;
                if (info != null)
                {
                    end = Math.Min(end, info.TotalLineNUM - 1);
                    if (start >= end) return;
                    var list = new ConcurrentBag<DividedFile>();
                    var fileNUM = info.FileNames.Count;
                    Parallel.For(
                        0, fileNUM, i =>
                        {
                            var fileStart = (info.MaxLineNUM * i);
                            var fileEnd = Math.Min(fileStart + info.MaxLineNUM - 1, end);
                            var lineNUM = (long)(fileEnd - fileStart);
                            if (fileEnd >= start && lineNUM > 0)
                            {
                                var dividedFile = new DividedFile(i);
                                var filePath = Path.Combine(InfoFileDirectory, info.FileNames[i]);
                                dividedFile.Load(filePath, fileStart, start, fileEnd, step);
                                list.Add(dividedFile);
                            }
                        }
                        );
                    if (list.Count > 0)
                    {
                        var array = list.OrderBy(e => e.Id).ToArray();
                        foreach (var file in array)
                        {
                            foreach (var line in file.Lines)
                            {
                                result.Add(line);
                            }
                        }
                    }
                }
            });
            return result.ToArray();
        }

        public async Task<string[]> GetAllLines()
        {
            var result = new List<string>();
            await Task.Run(() =>
            {
                var info = Info;
                if (info != null)
                {
                    var list = new ConcurrentBag<DividedFile>();
                    var fileNUM = info.FileNames.Count;
                    Parallel.For(
                        0, fileNUM, i =>
                        {
                            var dividedFile = new DividedFile(i);
                            var filePath = Path.Combine(InfoFileDirectory, info.FileNames[i]);
                            dividedFile.LoadAllLines(filePath);
                            list.Add(dividedFile);
                        }
                        );
                    if (list.Count > 0)
                    {
                        var array = list.OrderBy(e => e.Id).ToArray();
                        foreach (var file in array)
                        {
                            foreach (var line in file.Lines)
                            {
                                result.Add(line);
                            }
                        }
                    }

                }
            });
            return result.ToArray();
        }
    }

    public class DividedFile
    {
        public int Id
        {
            get;
            private set;
        }

        public string[] Lines
        {
            get;
            private set;
        }

        public DividedFile(int id)
        {
            this.Id = id;
        }

        public void LoadAllLines(string path)
        {
            Lines = File.ReadAllLines(path);
        }

        public void Load(string path, long offset, long start, long end, long step)
        {
            var list = new List<string>();
            var count = offset;
            using (var streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (count >= start)
                    {
                        if ((count - start) % step == 0)
                        {
                            list.Add(line);
                        }
                    }
                    count++;
                    if (count > end)
                    {
                        break;
                    }
                }
            }
            Lines = list.ToArray();
        }
    }
}



