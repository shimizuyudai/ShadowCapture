using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SimpleParticle
{
    public Vector3 position;
    public Vector3 angles;
    public Vector3 scale;
    public Color color;

    public SimpleParticle(Vector3 position, Vector3 angles, Vector3 scale, Color color)
    {
        this.position = position;
        this.angles = angles;
        this.scale = scale;
        this.color = color;
    }
}
