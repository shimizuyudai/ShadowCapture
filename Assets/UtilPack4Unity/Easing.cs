using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UtilPack4Unity
{
    public class Easing
    {
        public static float Invoke(string methodName, float t)
        {
            try
            {
                var mi = typeof(UtilPack4Unity.Easing).GetMethod(methodName);
                return (float)mi.Invoke(null, new object[] { t });
            }
            catch
            {
                return 0f;
            }

        }
        public static float EaseInBack(float t)
        {
            return t * t * ((1.70158f + 1f) * t - 1.70158f);
        }

        public static float EaseInBounce(float t)
        {
            t = 1f - t;

            if (t < 1f / 2.75f)
            {
                return 1f - (7.5625f * t * t);
            }
            if (t < 2f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 1f - (7.5625f * t * t + 0.75f);
            }
            if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 1f - (7.5625f * t * t + 0.9375f);
            }

            t -= 2.625f / 2.75f;
            return 1f - (7.5625f * t * t + 0.984375f);
        }

        public static float EaseInCircle(float t)
        {
            return -(Mathf.Sqrt(1f - t * t) - 1f);
        }

        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        public static float EaseInElastic(float t)
        {
            float p = 0.3f;
            float a = 1f;
            float s = 1.70158f;

            if (t == 0f)
            {
                return 0f;
            }
            if (t == 1.0)
            {
                return 1f;
            }

            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
            {
                s = p / (2f * 3.1419f) * Mathf.Asin(1f / a);
            }

            --t;
            return -(a * Mathf.Pow(2f, 10f * t) * Mathf.Sin((t - s) * (2f * 3.1419f) / p));
        }

        public static float EaseInExpo(float t)
        {
            return (t == 0)
              ? 0f
              : Mathf.Pow(2f, 10f * (t - 1f));
        }

        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        public static float EaseInQuart(float t)
        {
            return t * t * t * t;
        }

        public static float EaseInQuint(float t)
        {
            return t * t * t * t * t;
        }

        public static float EaseInSin(float t)
        {
            return -Mathf.Cos(t * (Mathf.PI / 2f)) + 1f;
        }

        public static float EaseOutBack(float t)
        {
            --t;
            return t * t * ((1.70158f + 1f) * t + 1.70158f) + 1f;
        }


        public static float EaseOutBounce(float t)
        {
            if (t < 1f / 2.75f)
            {
                return 7.5625f * t * t;
            }
            if (t < 2f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }
            if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }

            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }

        public static float EaseOutCircle(float t)
        {
            --t;
            return Mathf.Sqrt(1f - t * t);
        }

        public static float EaseOutCubic(float t)
        {
            --t;
            return t * t * t + 1f;
        }

        public static float EaseOutElastic(float t)
        {
            float p = 0.3f;
            float a = 1.0f;
            float s = 1.70158f;

            if (t == 0f)
            {
                return 0f;
            }
            if (t == 1f)
            {
                return 1f;
            }

            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
            {
                s = p / (2f * 3.1419f) * Mathf.Sin(1f / a);
            }
            return a * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * (2f * 3.1419f) / p) + 1f;
        }

        public static float EaseOutExpo(float t)
        {
            return (t == 1f)
              ? 1f
              : (-Mathf.Pow(2f, -10f * t) + 1f);
        }

        public static float EaseOutQuad(float t)
        {
            return -t * (t - 2f);
        }

        public static float EaseOutQuart(float t)
        {
            --t;
            return 1f - t * t * t * t;
        }

        public float easeOutQuint(float t)
        {
            --t;
            return t * t * t * t * t + 1f;
        }

        public static float EaseOutSin(float t)
        {
            return Mathf.Sin(t * (Mathf.PI / 2f));
        }

        public static float EaseInOutBack(float t)
        {
            float s = 1.70158f;
            float k = 1.525f;

            t *= 2f;
            s *= k;

            if (t < 1)
            {
                return 0.5f * (t * t * ((s + 1f) * t - s));
            }
            t -= 2f;
            return 0.5f * (t * t * ((s + 1f) * t + s) + 2f);
        }

        public static float EaseInOutBounce(float t)
        {
            return (t < 0.5f)
              ? (EaseInBounce(t * 2f) * 0.5f)
              : (EaseOutBounce(t * 2f - 1f) * 0.5f + 0.5f);
        }

        public static float EaseInOutCircle(float t)
        {
            t *= 2f;

            if (t < 1f)
            {
                return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
            }

            t -= 2f;
            return 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
        }

        public static float EaseInOutCubic(float t)
        {
            t *= 2f;

            if (t < 1f)
            {
                return 0.5f * t * t * t;
            }

            t -= 2f;
            return 0.5f * (t * t * t + 2f);
        }

        public static float EaseInOutElastic(float t)
        {
            float s = 1.70158f;
            float p = 0.3f * 1.5f;
            float a = 1f;

            if (t == 0f)
            {
                return 0f;
            }
            if (t == 1f)
            {
                return 1f;
            }

            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
            {
                s = p / (2f * 3.1419f) * Mathf.Asin(1f / a);
            }

            if (t < 1f)
            {
                --t;
                return -0.5f * (a * Mathf.Pow(2f, 10f * t) * Mathf.Sin((t - s) * (2f * 3.1419f) / p));
            }
            --t;
            return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2f * 3.1419f) / p) * 0.5f + 1f;
        }

        public static float EaseInOutExpo(float t)
        {
            if (t == 0f)
            {
                return 0f;
            }
            if (t == 1f)
            {
                return 1f;
            }

            t *= 2f;
            if (t < 1f)
            {
                return 0.5f * Mathf.Pow(2f, 10f * (t - 1f));
            }

            --t;
            return 0.5f * (-Mathf.Pow(2f, -10f * t) + 2f);
        }

        public static float EaseInOutQuad(float t)
        {
            t *= 2f;

            if (t < 1f)
            {
                return 0.5f * t * t;
            }

            --t;
            return -0.5f * (t * (t - 2f) - 1f);
        }

        public static float EaseInOutQuart(float t)
        {
            t *= 2f;

            if (t < 1f)
            {
                return 0.5f * t * t * t * t;
            }

            t -= 2f;
            return -0.5f * (t * t * t * t - 2f);
        }

        public static float EaseInOutQuint(float t)
        {
            t *= 2f;

            if (t < 1f)
            {
                return 0.5f * t * t * t * t * t;
            }

            t -= 2f;
            return 0.5f * (t * t * t * t * t + 2f);
        }

        public static float EaseInOutSin(float t)
        {
            return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1f);
        }

    }


}

