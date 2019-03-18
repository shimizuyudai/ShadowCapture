using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Median
{

    public class MedianCut
    {
        public List<Cube> Cubes
        {
            get;
            set;
        }
        public event Action<List<Cube>> OnCut;
        public event Action<List<Cube>> OnComplete;
        public event Action<string> OnError;
        public bool isUseTimeout;
        public TimeSpan Timeout = TimeSpan.FromSeconds(30);
        public bool IsCutting;

        public enum Mode
        {
            MaxSide,
            Amount,
            Volume,
            Variance
        }

        public Mode mode = Mode.Variance;

        public MedianCut()
        {

        }

        public void CutCube1(ColorType colorType, Color[] colors, List<Color> colors1, List<Color> colors2, float t)
        {
            switch (colorType)
            {
                case ColorType.R:
                    foreach (var color in colors)
                    {
                        if (color.r >= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;

                case ColorType.G:
                    foreach (var color in colors)
                    {
                        if (color.g >= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;

                case ColorType.B:
                    foreach (var color in colors)
                    {
                        if (color.b >= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;
            }
        }

        public void CutCube2(ColorType colorType, Color[] colors, List<Color> colors1, List<Color> colors2, float t)
        {
            switch (colorType)
            {
                case ColorType.R:
                    foreach (var color in colors)
                    {
                        if (color.r <= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;

                case ColorType.G:
                    foreach (var color in colors)
                    {
                        if (color.g <= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;

                case ColorType.B:
                    foreach (var color in colors)
                    {
                        if (color.b <= t)
                        {
                            colors1.Add(color);
                        }
                        else
                        {
                            colors2.Add(color);
                        }
                    }
                    break;
            }
        }

        public async Task<Color[]> Cut(Color[] colors, int num)
        {
            IsCutting = true;
            var result = new List<Color>();
            var startTime = DateTime.Now;
            var elapsedTime = 0f;
            Cubes = new List<Cube>() { new Cube(colors) };
            OnCut?.Invoke(Cubes);
            while (Cubes.Count != num)
            {
                if (!IsCutting)
                {
                    await Task.Delay(10);
                    continue;
                }
                var hasAdded = false;
                var axis = ColorType.R;
                Cube cube = null;
                await Task.Run(() =>
                {
                    cube = Cubes.OrderByDescending(e => e.Volume).First();//一番体積の大きいCube
                    



                    switch (mode)
                    {
                        case Mode.Variance:
                            cube = Cubes.OrderByDescending(e => e.VarianceAverage).First();//分散平均値の一番小さいCube
                            axis = cube.MaxVarianceColor;
                            break;

                        case Mode.Amount:
                            cube = Cubes.OrderByDescending(e => e.Colors.Length).First();//一番所有色の多いCube
                            break;

                        case Mode.MaxSide:
                            cube = Cubes.OrderByDescending(e => e.MaxLength).First();//一番長い辺を持つCube
                            break;

                        case Mode.Volume:
                            cube = Cubes.OrderByDescending(e => e.Volume).First();//一番体積の大きいCube
                            break;
                    }

                    axis = cube.MaxLengthColor;
                    Cube cube1 = null;
                    Cube cube2 = null;
                    var colors1 = new List<Color>();
                    var colors2 = new List<Color>();

                    switch (axis)
                    {
                        case ColorType.R:
                            {
                                var median = cube.RMedian;
                                CutCube1(ColorType.R, cube.Colors, colors1, colors2, median);
                                if (colors1.Count > 0 && colors2.Count > 0)
                                {
                                    cube1 = new Cube(colors1.ToArray());
                                    cube2 = new Cube(colors2.ToArray());
                                }
                                else
                                {
                                    colors1 = new List<Color>();
                                    colors2 = new List<Color>();
                                    CutCube2(ColorType.R, cube.Colors, colors1, colors2, median);
                                    if (colors1.Count > 0 && colors2.Count > 0)
                                    {
                                        cube1 = new Cube(colors1.ToArray());
                                        cube2 = new Cube(colors2.ToArray());
                                    }
                                }

                            }
                            break;

                        case ColorType.G:
                            {
                                var median = cube.GMedian;
                                CutCube1(ColorType.G, cube.Colors, colors1, colors2, median);
                                if (colors1.Count > 0 && colors2.Count > 0)
                                {
                                    cube1 = new Cube(colors1.ToArray());
                                    cube2 = new Cube(colors2.ToArray());
                                }
                                else
                                {
                                    colors1 = new List<Color>();
                                    colors2 = new List<Color>();
                                    CutCube2(ColorType.G, cube.Colors, colors1, colors2, median);
                                    if (colors1.Count > 0 && colors2.Count > 0)
                                    {
                                        cube1 = new Cube(colors1.ToArray());
                                        cube2 = new Cube(colors2.ToArray());
                                    }
                                }
                            }
                            break;

                        case ColorType.B:
                            {
                                var median = cube.BCenter;
                                CutCube1(ColorType.B, cube.Colors, colors1, colors2, median);
                                if (colors1.Count > 0 && colors2.Count > 0)
                                {
                                    cube1 = new Cube(colors1.ToArray());
                                    cube2 = new Cube(colors2.ToArray());
                                }
                                else
                                {
                                    colors1 = new List<Color>();
                                    colors2 = new List<Color>();
                                    CutCube2(ColorType.B, cube.Colors, colors1, colors2, median);
                                    if (colors1.Count > 0 && colors2.Count > 0)
                                    {
                                        cube1 = new Cube(colors1.ToArray());
                                        cube2 = new Cube(colors2.ToArray());
                                    }
                                }
                            }
                            break;
                    }

                    var add1 = false;
                    var add2 = false;
                    if (cube1 != null)
                    {
                        if (cube1.Colors.Length > 0) Cubes.Add(cube1);
                        add1 = true;
                    }
                    if (cube2 != null)
                    {
                        if (cube2.Colors.Length > 0) Cubes.Add(cube2);
                        add2 = true;
                    }
                    if (add1 && add2)
                    {
                        Cubes.Remove(cube);
                        hasAdded = true;
                    }
                });

                if (!hasAdded)
                {
                    Debug.Log(axis);
                    Debug.Log(cube.Colors.Min(e => e.g));
                    Debug.Log(cube.Colors.Max(e => e.g));
                    OnError?.Invoke("error");
                    break;
                }

                OnCut?.Invoke(Cubes);
                if (isUseTimeout)
                {
                    var timeSpan = DateTime.Now - startTime;
                    if (timeSpan > Timeout)
                    {
                        OnError?.Invoke("timeout");
                        break;
                    }
                }

            }
            OnComplete?.Invoke(Cubes);
            return Cubes.Select(e => e.CenterColor).ToArray();
        }


    }


    public enum ColorType
    {
        R,
        G,
        B
    }

    public class Cube
    {
        public Cube(Color[] colors)
        {
            Init(colors);
        }

        public void Init(Color[] colors)
        {
            this.Colors = colors;
            var rMax = float.MinValue;
            var gMax = float.MinValue;
            var bMax = float.MinValue;
            var rMin = float.MaxValue;
            var gMin = float.MaxValue;
            var bMin = float.MaxValue;

            var rList = new List<float>();
            var gList = new List<float>();
            var bList = new List<float>();

            foreach (var color in colors)
            {
                if (color.r > rMax)
                {
                    rMax = color.r;
                }
                if (color.g > gMax)
                {
                    gMax = color.g;
                }
                if (color.b > bMax)
                {
                    bMax = color.b;
                }

                if (color.r < rMin)
                {
                    rMin = color.r;
                }

                if (color.g < gMin)
                {
                    gMin = color.g;
                }

                if (color.b < bMin)
                {
                    bMin = color.b;
                }
                rList.Add(color.r);
                gList.Add(color.g);
                bList.Add(color.b);
            }

            RLength = rMax - rMin;
            GLength = gMax - gMin;
            BLength = bMax - bMin;
            RCenter = rMin + RLength / 2f;
            GCenter = gMin + GLength / 2f;
            BCenter = bMin + BLength / 2f;
            Volume = RLength * GLength * BLength;

            RVariance = EMath.GetVariance(rList.ToArray());
            GVariance = EMath.GetVariance(gList.ToArray());
            BVariance = EMath.GetVariance(bList.ToArray());
        }

        public Cube()
        {

        }

        public Color[] Colors;
        public float RLength
        {
            get;
            private set;
        }
        public float GLength
        {
            get;
            private set;
        }
        public float BLength
        {
            get;
            private set;
        }

        public float RVariance
        {
            get;
            private set;
        }
        public float GVariance
        {
            get;
            private set;
        }
        public float BVariance
        {
            get;
            private set;
        }

        public float VarianceAverage
        {
            get {
                return (RVariance + GVariance + BVariance) / 3f;
            }
        }

        public float MaxVariance
        {
            get {
                return Mathf.Max(RVariance + GVariance + BVariance);
            }
        }

        public float RCenter
        {
            get;
            private set;
        }
        public float GCenter
        {
            get;
            private set;
        }
        public float BCenter
        {
            get;
            private set;
        }


        public float RMedian
        {
            get {
                if (Colors.Length == 1) return Colors[0].r;
                var colors = Colors.OrderBy(e => e.r).ToArray();
                if (Colors.Length % 2 == 0)
                {
                    var i = colors.Length / 2 - 1;
                    var j = colors.Length / 2;
                    return (colors[i].r + colors[j].r) / 2f;
                }
                else
                {
                    var i = colors.Length / 2;
                    return colors[i].r;
                }
            }
        }
        public float GMedian
        {
            get {
                if (Colors.Length == 1) return Colors[0].g;
                var colors = Colors.OrderBy(e => e.g).ToArray();
                if (Colors.Length % 2 == 0)
                {
                    var i = colors.Length / 2 - 1;
                    var j = colors.Length / 2;
                    return (colors[i].g + colors[j].g) / 2f;
                }
                else
                {
                    var i = colors.Length / 2;
                    return colors[i].g;
                }
            }
        }
        public float BMedian
        {
            get {
                if (Colors.Length == 1) return Colors[0].b;
                var colors = Colors.OrderBy(e => e.b).ToArray();
                if (Colors.Length % 2 == 0)
                {
                    var i = colors.Length / 2 - 1;
                    var j = colors.Length / 2;
                    return (colors[i].b + colors[j].b) / 2f;
                }
                else
                {
                    var i = colors.Length / 2;
                    return colors[i].b;
                }
            }
        }

        public float MaxLength
        {
            get {
                return Mathf.Max(RLength, GLength, BLength);
            }
        }

        public Color CenterColor
        {
            get {
                var r = RCenter;
                var g = GCenter;
                var b = BCenter;
                return new Color(r, g, b, 1);
            }
        }

        public Color ReferenceColor
        {
            get {
                var centerColor = CenterColor;
                var center = new Vector3(centerColor.r, centerColor.g, centerColor.b);
                return Colors.OrderBy(e => Vector3.Distance(new Vector3(e.r, e.g, e.b), center)).First();
            }
        }

        public ColorType MaxLengthColor
        {
            get {
                var rLength = RLength;
                var gLength = GLength;
                var bLength = BLength;
                if (rLength > gLength)
                {
                    if (rLength > bLength)
                    {
                        return ColorType.R;
                    }
                    else
                    {
                        return ColorType.B;
                    }
                }
                else
                {
                    if (gLength > bLength)
                    {
                        return ColorType.G;
                    }
                    else
                    {
                        return ColorType.B;
                    }
                }
            }
        }

        public ColorType MaxVarianceColor
        {
            get {
                if (RVariance > GVariance)
                {
                    if (RVariance > BVariance)
                    {
                        return ColorType.R;
                    }
                    else
                    {
                        return ColorType.B;
                    }
                }
                else
                {
                    if (GVariance > BVariance)
                    {
                        return ColorType.G;
                    }
                    else
                    {
                        return ColorType.B;
                    }
                }
            }
        }

        public float Volume
        {
            get;
            private set;
        }
    }
}