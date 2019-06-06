using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity.TypeUtils
{
    [Serializable]
    public struct IntVec2
    {
        public int x;
        public int y;

        public IntVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public struct IntVec3
    {
        public int x;
        public int y;
        public int z;

        public IntVec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class Format
    {
        public static string DateTime2String(DateTime dateTime)
        {
            return dateTime.Year.ToString("0000") + "_" + dateTime.Month.ToString("00") + "_" + dateTime.Day.ToString("00")
                 + "_" + dateTime.Hour.ToString("00") + "_" + dateTime.Minute.ToString("00") + "_" + dateTime.Second.ToString("00")
                  + "_" + dateTime.Millisecond.ToString("0000");
        }
    }


    namespace Json
    {
        public static class Convert
        {
            public static Vector2 Vec2ToVector2(Vec2 vec)
            {
                return new Vector2(vec.x, vec.y);
            }

            public static Vector3 Vec3ToVector3(Vec3 vec)
            {
                return new Vector3(vec.x, vec.y, vec.z);
            }

            public static Vector4 Vec4ToVector4(Vec4 vec)
            {
                return new Vector4(vec.x, vec.y, vec.z, vec.w);
            }

            public static Color JColorToColor(JColor color)
            {
                return new UnityEngine.Color(color.r, color.g, color.b, color.a);
            }

            public static Vec2 Vector2ToVec2(Vector2 vec)
            {
                return new Vec2(vec.x, vec.y);
            }

            public static Vec3 Vector3ToVec3(Vector3 vec)
            {
                return new Vec3(vec.x, vec.y, vec.z);
            }

            public static Vec4 Vector4ToVec4(Vector4 vec)
            {
                return new Vec4(vec.x, vec.y, vec.z, vec.w);
            }

            public static JColor ColorToJcolor(Color color)
            {
                return new JColor(color.r, color.g, color.b, color.a);
            }
        }

        public struct Vec2
        {
            public float x;
            public float y;
            public static Vec2 zero
            {
                get {
                    return new Vec2(0, 0);
                }
            }
            public Vec2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public Vec2(Vector2 vec)
            {
                this.x = vec.x;
                this.y = vec.y;
            }

            public Vector2 ToVector2()
            {
                return Convert.Vec2ToVector2(this);
            }
        }

        public struct Vec3
        {
            public float x;
            public float y;
            public float z;
            public static Vec3 zero
            {
                get {
                    return new Vec3(0, 0, 0);
                }
            }
            public Vec3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Vec3(Vector3 vec)
            {
                this.x = vec.x;
                this.y = vec.y;
                this.z = vec.z;
            }

            public Vector3 ToVector3()
            {
                return Convert.Vec3ToVector3(this);
            }
        }

        public struct Vec4
        {
            public float x;
            public float y;
            public float z;
            public float w;
            public static Vec4 zero
            {
                get {
                    return new Vec4(0, 0, 0, 0);
                }
            }
            public Vec4(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public Vec4(Vector4 vec)
            {
                this.x = vec.x;
                this.y = vec.y;
                this.z = vec.z;
                this.w = vec.w;
            }

            public Vector4 ToVector4()
            {
                return Convert.Vec4ToVector4(this);
            }
        }

        public struct JColor
        {
            public float r;
            public float g;
            public float b;
            public float a;

            public JColor(float r, float g, float b, float a)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }

            public JColor(Color color)
            {
                this.r = color.r;
                this.g = color.g;
                this.b = color.b;
                this.a = color.a;
            }

            public Color ToColor()
            {
                return new Color(r,g,b,a);
            }
        }
    }
}