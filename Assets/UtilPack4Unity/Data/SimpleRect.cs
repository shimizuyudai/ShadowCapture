using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class SimpleRect
    {
        public float X;
        public float Y;
        public float W;
        public float H;

        public SimpleRect()
        {

        }

        public SimpleRect(float x, float y, float w, float h)
        {
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
        }
    }

    public class SimpleIntRect
    {
        public int X;
        public int Y;
        public int W;
        public int H;

        public SimpleIntRect()
        {

        }

        public SimpleIntRect(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
        }
    }
}
