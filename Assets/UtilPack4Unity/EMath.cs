using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//計算処理クラス
namespace UtilPack4Unity
{
    public static class EMath
    {
        public static float Map(float value, float start1, float stop1, float start2, float stop2)
        {
            return ((value - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        //public static Vector2 Map(Vector2 value, Vector2 areaSize1, Vector2 areaSize2)
        //{
        //    return new Vector2(EMath.Map(value.x, -areaSize1.x / 2f, -areaSize1.x / 2f, -areaSize2.x / 2f, -areaSize2.x / 2f),
        //    EMath.Map(value.x, -areaSize1.y / 2f, -areaSize1.y / 2f, -areaSize2.y / 2f, -areaSize2.y / 2f));
        //}

        public static Vector2 GetNormalizedAspect(Vector2 aspcet)
        {
            var total = aspcet.x + aspcet.y;
            return new Vector2(aspcet.x / total, aspcet.y / total);
        }

        public static Vector2 GetNormalizedShirnkAspect(Vector2 aspect)
        {
            return GetShrinkFitSize(aspect, Vector2.one);
        }

        public static Vector2 GetNormalizedExpandAspect(Vector2 aspect)
        {
            return GetExpandFitSize(aspect, Vector2.one);
        }

        //areasizeに収まる形
        public static Vector2 GetShrinkFitSize(Vector2 baseSize, Vector2 areaSize)
        {
            var size = areaSize;
            var rate = new Vector2(baseSize.x / areaSize.x, baseSize.y / areaSize.y);
            if (rate.x > rate.y)
            {
                size.y = baseSize.y * (size.x / (float)baseSize.x);
            }
            else
            {
                size.x = baseSize.x * (size.y / baseSize.y);
            }
            return size;
        }

        //areasizeを包む形
        public static Vector2 GetExpandFitSize(Vector2 baseSize, Vector2 areaSize)
        {
            var size = areaSize;
            var rate = new Vector2(baseSize.x / areaSize.x, baseSize.y / areaSize.y);
            if (rate.x < rate.y)
            {
                size.y = baseSize.y * (size.x / (float)baseSize.x);
            }
            else
            {
                size.x = baseSize.x * (size.y / baseSize.y);
            }
            return size;
        }


        //偏差(値-平均値)
        public static float GetDeviation(float value, float average)
        {
            return value - average;
        }

        //標準偏差
        public static float GetStandardDeviation(float[] values)
        {
            return Mathf.Sqrt(GetVariance(values));
        }

        //分散
        public static float GetVariance(float[] values)
        {
            var average = values.Average();
            var sum = 0f;
            foreach (var value in values)
            {
                var deviation = GetDeviation(value, average);
                sum += Mathf.Pow(deviation, 2);
            }
            return sum / values.Length;
        }

        //中央値
        public static float GetMedian(float[] array)
        {
            if (array.Length == 1) return array[0];
            var orderedArray = array.OrderBy(e => e).ToArray();
            if (orderedArray.Length % 2 == 0)
            {
                var i = orderedArray.Length / 2 - 1;
                var j = orderedArray.Length / 2;
                return (orderedArray[i] + orderedArray[j]) / 2f;
            }
            else
            {
                var i = orderedArray.Length / 2;
                return orderedArray[i];
            }
        }
    }
}
