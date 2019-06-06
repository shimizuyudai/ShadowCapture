using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace UtilPack4Unity
{
    public static class EUtils
    {

        public static string GetUniqueId(List<string> list)
        {
            var guid = new System.Guid().ToString();
            while (list.Contains(guid))
            {
                guid = new System.Guid().ToString();
            }
            return guid;
        }

        public static T GetRandomElement<T>(T[] array)
        {
            var i = Random.Range(0, array.Length);
            return array[i];
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            var i = Random.Range(0, list.Count);
            return list[i];
        }

        public static T[] Shuffle<T>(T[] array)
        {
            var length = array.Length;
            for (var i = length - 1; i > 0; i--)
            {
                var j = (int)Mathf.Floor(Random.Range(0f, 1f) * ((float)i + 1f));
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
            return array;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            var length = list.Count;
            for (var i = length - 1; i > 0; i--)
            {
                var j = (int)Mathf.Floor(Random.Range(0f, 1f) * ((float)i + 1f));
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
            return list;
        }

        public static string GetProcessName(string path)
        {
            var fileName = System.IO.Path.GetFileName(path);
            var extension = System.IO.Path.GetExtension(fileName);
            return fileName.Replace(extension, string.Empty);
        }

        public static bool StringToEnumElement<T>(System.Type type, string str, ref T elm)
        {
            var result = false;
            if (!type.IsEnum) return result;
            //定義済みであるか
            if (!System.Enum.IsDefined(type, str)) return result;
            elm = (T)System.Enum.Parse(type, str);
            result = true;
            return result;
        }

        public static class Reflection
        {
            //public string[] GetMethods()
            //{

            //}
        }

        public static class RandomUtils
        {

            public static async Task<Vector2[]> GetInsideCirclePointsAsync(int num, float radius)
            {
                var result = new Vector2[num];
                await Task.Run(
                    () =>
                    {
                        var random = new System.Random();
                        Parallel.For(0, num, i =>
                          {
                              var r = (float)random.NextDouble() * radius;
                              var angle = (float)random.NextDouble() * Mathf.PI * 2.0f;
                              var x = (float)Mathf.Cos(angle) * r;
                              var y = (float)Mathf.Sin(angle) * r;
                              result[i] = new Vector2(x, y);
                          });
                    }
                    );
                return result;
            }

            public static async Task<Vector3[]> GetOnSpherePositionsAsync(int num, float radius)
            {
                var result = new Vector3[num];
                await Task.Run(
                   () =>
                   {
                       var random = new System.Random();
                       Parallel.For(0, num, i =>
                       {
                           var r = radius;
                           var angle = (float)random.NextDouble() * Mathf.PI * 2.0f;
                           var t = (float)random.NextDouble() * 2f - 1f;
                           var x = (float)Mathf.Cos(angle) * Mathf.Sqrt(1f - t * t) * r;
                           var y = (float)Mathf.Sin(angle) * Mathf.Sqrt(1f - t * t) * r;
                           var z = (float)t * r;
                           result[i] = new Vector3(x, y, z);
                       });
                   }
                   );
                return result;
            }

            public static async Task<Vector3[]> GetInsideSpherePositionsAsync(int num, float radius)
            {
                var result = new Vector3[num];
                await Task.Run(
                   () =>
                   {
                       var random = new System.Random();
                       Parallel.For(0, num, i =>
                       {
                           var r = (float)random.NextDouble() * radius;
                           var angle = (float)random.NextDouble() * Mathf.PI * 2.0f;
                           var t = (float)random.NextDouble() * 2f - 1f;
                           var x = (float)Mathf.Cos(angle) * Mathf.Sqrt(1f - t * t) * r;
                           var y = (float)Mathf.Sin(angle) * Mathf.Sqrt(1f - t * t) * r;
                           var z = (float)t * r;
                           result[i] = new Vector3(x, y, z);
                       });
                   }
                   );
                return result;
            }

            public static async Task<Vector2[]> GetOnCirclePointsAsync(int num, float radius)
            {
                var result = new Vector2[num];
                await Task.Run(
                    () =>
                    {
                        var random = new System.Random();
                        Parallel.For(0, num, i =>
                          {
                              var angle = (float)random.NextDouble() * Mathf.PI * 2.0f;
                              var x = (float)Mathf.Cos(angle) * radius;
                              var y = (float)Mathf.Sin(angle) * radius;
                              result[i] = new Vector2(x, y);
                          });
                    }
                    );
                return result;
            }

            public static async Task<Vector2[]> GetInside2DAreaPointsAsync(int num, Vector2 areaSize)
            {
                var result = new Vector2[num];
                await Task.Run(
                    () =>
                    {
                        var random = new System.Random();
                        Parallel.For(0, num, i =>
                        {
                            var x = (float)random.NextDouble() * areaSize.x - areaSize.x / 2f;
                            var y = (float)random.NextDouble() * areaSize.y - areaSize.y / 2f;
                            result[i] = new Vector2(x, y);
                        });
                    }
                    );
                return result;
            }

            public static async Task<Vector3[]> GetInside3DAreaPointsAsync(int num, Vector3 areaSize)
            {
                var result = new Vector3[num];
                await Task.Run(
                    () =>
                    {
                        var random = new System.Random();
                        Parallel.For(0, num, i =>
                        {
                            var x = (float)random.NextDouble() * areaSize.x - areaSize.x / 2f;
                            var y = (float)random.NextDouble() * areaSize.y - areaSize.y / 2f;
                            var z = (float)random.NextDouble() * areaSize.z - areaSize.z / 2f;
                            result[i] = new Vector3(x, y, z);
                        });
                    }
                    );
                return result;
            }

            public static async Task<Vector3[]> GetBoxSurfacePointsAsync(int num, Vector3 boxSize)
            {
                var result = new Vector3[num];
                await Task.Run(
                    () =>
                    {
                        var random = new System.Random();
                        Parallel.For(0, num, i =>
                        {
                            var j = random.Next(3);
                            var k = random.Next(2);
                            var pos = Vector3.zero;
                            switch (j)
                            {
                                case 0:
                                    pos.x = (k > 0 ? -0.5f : 0.5f) * boxSize.x;
                                    pos.y = (float)random.NextDouble() * boxSize.y - boxSize.y / 2f;
                                    pos.z = (float)random.NextDouble() * boxSize.z - boxSize.z / 2f;
                                    break;

                                case 1:
                                    pos.y = (k > 0 ? -0.5f : 0.5f) * boxSize.y;
                                    pos.x = (float)random.NextDouble() * boxSize.x - boxSize.x / 2f;
                                    pos.z = (float)random.NextDouble() * boxSize.z - boxSize.z / 2f;
                                    break;

                                case 2:
                                    pos.z = (k > 0 ? -0.5f : 0.5f) * boxSize.z;
                                    pos.x = (float)random.NextDouble() * boxSize.x - boxSize.x / 2f;
                                    pos.y = (float)random.NextDouble() * boxSize.y - boxSize.y / 2f;
                                    break;
                            }
                            result[i] = pos;
                        });
                    }
                    );
                return result;
            }
        }



        public static class Music
        {
            public static readonly string[] pitchNames = new string[12]
            {
            "C","C#","D","D#","E","F","F#","G","G#","A","A#","B"
            };
            public static float MtoF(int noteNum)
            {
                return 440f * Mathf.Pow(2, ((float)(noteNum - 69)) / 12f);
            }

            //ヤマハ式
            //音名,音域
            public static int NtoM(string name, int register)
            {
                var m = -1;
                if (string.IsNullOrEmpty(name)) return m;
                if (!pitchNames.Contains(name)) return m;
                if (register < -2 || register > 8) return m;
                var index = pitchNames.Select((e, i) => new { index = i, name = e }).First(e => e.name == name).index;
                m = index + pitchNames.Length * register + pitchNames.Length * 2;
                return m;
            }

            //ヤマハ式
            //音名+音域
            public static int NtoM(string name)
            {
                var m = -1;
                if (string.IsNullOrEmpty(name)) return m;
                if (name.Length < 2) return m;
                var chars = name.ToCharArray();
                var registerStr = chars[chars.Length - 1].ToString();
                var n = string.Empty;
                for (var i = 0; i < chars.Length - 1; i++)
                {
                    n += chars[i];
                }
                int register = 0;
                if (!int.TryParse(registerStr, out register)) return m;
                return NtoM(n, register);
            }

            //pitchNameからノート番号のスケールを取得
            public static List<int> GetScaleNoteNumbers(string[] pitchNames)
            {
                var notes = new List<int>();
                var baseNotes = new List<int>();
                for (var i = 0; i < pitchNames.Length; i++)
                {
                    var m = EUtils.Music.NtoM(pitchNames[i], -2);
                    if (m >= 0)
                    {
                        baseNotes.Add(m);
                    }
                }

                for (var i = 0; i < baseNotes.Count; i++)
                {
                    var n = baseNotes[i];
                    while (n < 128)
                    {
                        notes.Add(n);
                        n += 12;
                    }
                }
                return notes.OrderBy(e => e).ToList();
            }
        }
    }
}
