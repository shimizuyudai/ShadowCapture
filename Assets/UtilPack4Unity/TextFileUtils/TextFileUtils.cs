using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextFileUtils
{
    public class TextFileProc
    {
        public static async Task<string> TailAsync(string path, long line, string encoding = "UTF-8")
        {
            var result = await Task.Run(() =>
            {
                return Tail(path, line, encoding);
            });
            return result;
        }

        public static string Tail(string path, long line = 1, string encoding = "UTF-8")
        {
            line = Math.Max(1, line);
            int BUFFER_SIZE = 32;       // バッファーサイズ(あえて小さく設定)
            long offset = 0;
            long loc = 0;
            long foundCount = 0;
            var buffer = new byte[BUFFER_SIZE];
            var isFirst = true;
            var isFound = false;
            long lineCount = 0;
            // ファイル共有モードで開く
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // 検索ブロック位置の繰り返し
                for (long i = 0; ; i++)
                {
                    // ブロック開始位置に移動
                    offset = Math.Min((int)fs.Length, (i + 1) * BUFFER_SIZE);
                    loc = 0;
                    if (fs.Length <= i * BUFFER_SIZE)
                    {
                        // ファイルの先頭まで達した場合
                        if (foundCount > 0 || fs.Length > 0)
                        {
                            if (lineCount == line)
                            {
                                isFound = true;
                            }
                            //return string.Empty;
                            break;
                        }
                        // 行が未存在
                        throw new ArgumentOutOfRangeException("NOT FOUND DATA");
                    }

                    fs.Seek(-offset, SeekOrigin.End);

                    // ブロックの読み込み
                    var readLength = offset - BUFFER_SIZE * i;
                    for (int j = 0; j < readLength; j += fs.Read(buffer, j, (int)readLength - j)) ;

                    // ブロック内の改行コードの検索
                    for (int k = (int)readLength - 1; k >= 0; k--)
                    {
                        if (buffer[k] == 0x0A)
                        {
                            lineCount++;
                            if (isFirst && k == readLength - 1) continue;
                            if (++foundCount == line)
                            {
                                // 所定の行数が見つかった場合
                                loc = k + 1;
                                isFound = true;
                                break;
                            }
                        }
                    }
                    isFirst = false;
                    if (isFound) break;
                }

                if (!isFound) return string.Empty;

                // 見つかった場合
                fs.Seek(-offset + loc, SeekOrigin.End);

                using (var sr = new StreamReader(fs, Encoding.GetEncoding(encoding)))
                {
                    var result = sr.ReadToEnd();
                    if (result.Contains(Environment.NewLine))
                    {
                        result = result.Substring(0, result.IndexOf(Environment.NewLine));
                    }
                    return result;
                }
            }
        }

        public static async Task<List<string>> TailAsync(string path, List<long> lines, string encoding = "UTF-8")
        {
            var result = await Task.Run(() =>
             {
                 return Tail(path, lines, encoding);
             });
            return result;
        }


        public static List<string> Tail(string path, List<long> lines, string encoding = "UTF-8")
        {
            int BUFFER_SIZE = 32;       // バッファーサイズ(あえて小さく設定)
            long offset = 0;
            long loc = 0;
            long foundCount = 0;
            var buffer = new byte[BUFFER_SIZE];
            var isFirst = true;
            var isFound = false;
            var result = new List<string>();
            long lineCount = 0;
            // ファイル共有モードで開く
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // 検索ブロック位置の繰り返し
                for (long i = 0; ; i++)
                {
                    // ブロック開始位置に移動
                    offset = Math.Min(fs.Length, (i + 1) * BUFFER_SIZE);
                    loc = 0;
                    if (fs.Length <= i * BUFFER_SIZE)
                    {
                        // ファイルの先頭まで達した場合
                        if (foundCount > 0 || fs.Length > 0)
                        {
                            if (lines.Contains(lineCount))
                            {
                                result.Add(GetLine(path, -offset + loc, encoding));
                                isFound = true;
                            }
                            break;
                        }
                        // 行が未存在
                        throw new ArgumentOutOfRangeException("NOT FOUND DATA");
                    }

                    fs.Seek(-offset, SeekOrigin.End);

                    // ブロックの読み込み
                    var readLength = offset - BUFFER_SIZE * i;
                    for (int j = 0; j < readLength; j += fs.Read(buffer, j, (int)readLength - j)) ;

                    // ブロック内の改行コードの検索
                    for (int k = (int)readLength - 1; k >= 0; k--)
                    {
                        if (buffer[k] == 0x0A)
                        {
                            lineCount++;
                            if (isFirst && k == readLength - 1) continue;
                            if (lines.Contains(++foundCount))
                            {
                                // 所定の行数が見つかった場合
                                loc = k + 1;
                                result.Add(GetLine(path, -offset + loc, encoding));
                                if (result.Count >= lines.Count)
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    isFirst = false;
                    if (isFound) break;
                }
                // 見つかった場合
                return result;
            }
        }

        private static string GetLine(string path, long offset, string encoding)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(offset, SeekOrigin.End);
            var sr = new StreamReader(fs, Encoding.GetEncoding(encoding));
            var result = sr.ReadToEnd();
            if (result.Contains(Environment.NewLine))
            {
                result = result.Substring(0, result.IndexOf(Environment.NewLine));
            }
            fs.Dispose();
            sr.Dispose();
            return result;
        }

        public static async Task TailAsync(string path, Action<string> callback, CancellationToken cancellationToken, string encoding = "UTF-8")
        {
            await Task.Run(
                () =>
                {
                    //lines = Math.Max(1, lines);
                    int BUFFER_SIZE = 32;       // バッファーサイズ(あえて小さく設定)
                    long offset = 0;
                    long loc = 0;
                    long foundCount = 0;
                    var buffer = new byte[BUFFER_SIZE];
                    var isFirst = true;
                    var isFound = false;
                    var result = new List<string>();
                    long lineCount = 0;
                    // ファイル共有モードで開く
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // 検索ブロック位置の繰り返し
                        for (long i = 0; ; i++)
                        {
                            if (cancellationToken.IsCancellationRequested) break;

                            // ブロック開始位置に移動
                            offset = Math.Min(fs.Length, (i + 1) * BUFFER_SIZE);
                            loc = 0;
                            if (fs.Length <= i * BUFFER_SIZE)
                            {
                                // ファイルの先頭まで達した場合
                                if (foundCount > 0 || fs.Length > 0)
                                {
                                    callback(GetLine(path, -offset + loc, encoding));
                                    break;
                                }
                                // 行が未存在
                                throw new ArgumentOutOfRangeException("NOT FOUND DATA");
                            }

                            fs.Seek(-offset, SeekOrigin.End);

                            // ブロックの読み込み
                            var readLength = offset - BUFFER_SIZE * i;
                            for (int j = 0; j < readLength; j += fs.Read(buffer, j, (int)readLength - j)) ;

                            // ブロック内の改行コードの検索
                            for (int k = (int)readLength - 1; k >= 0; k--)
                            {
                                if (buffer[k] == 0x0A)
                                {
                                    lineCount++;
                                    if (isFirst && k == readLength - 1) continue;
                                    // 所定の行数が見つかった場合
                                    loc = k + 1;
                                    callback(GetLine(path, -offset + loc, encoding));
                                }
                            }
                            isFirst = false;
                            if (isFound) break;
                        }
                    }
                }
                );

        }
    }
}
