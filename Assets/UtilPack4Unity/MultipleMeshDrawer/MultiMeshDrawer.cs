using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MultiMeshDrawer : MonoBehaviour
{
    [SerializeField]
    protected uint instanceCount;

    [SerializeField]
    protected Mesh mesh;
    

    [SerializeField]
    protected ComputeShader computeShader;
    public ComputeShader ComputeShader
    {
        get {
            return computeShader;
        }
    }

    [SerializeField]
    protected Material material;

    protected ComputeBuffer computeBuffer;
    public ComputeBuffer ComputeBuffer
    {
        get {
            return computeBuffer;
        }
    }
    ComputeBuffer argsBuffer;

    [SerializeField]
    Vector3 areaScale;

    protected virtual void Awake()
    {
        //computeShader = Instantiate(computeShader) as ComputeShader;
    }
    // Use this for initialization
    protected virtual void Start()
    {
        
        var pList = new List<SimpleParticle>();
        for (var i = 0; i < instanceCount; i++)
        {
            var pos = new Vector3(Random.Range(-areaScale.x / 2f, areaScale.x / 2f), Random.Range(-areaScale.y / 2f, areaScale.y / 2f), Random.Range(-areaScale.z / 2f, areaScale.z / 2f));
            var angles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var s = Random.Range(0.1f, 0.5f);
            var scale = new Vector3(s,s,s);
            var color = Random.ColorHSV();
            var p = new SimpleParticle(pos, angles, scale, color);
            pList.Add(p);
        }
        Init(pList, "_TestBuffer", "_TestBuffer");
    }

    public virtual void Init<T>(List<T> list, string computeShaderBufferProp, string shaderBufferProp)
    {
        Release();
        computeBuffer = new ComputeBuffer((int)instanceCount, Marshal.SizeOf(typeof(T)));
        var args = new uint[5];
        args[0] = mesh.GetIndexCount(0);
        args[1] = instanceCount;
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);
        computeBuffer.SetData(list.ToArray());
        computeShader.SetBuffer(0, computeShaderBufferProp, computeBuffer);
        material.SetBuffer(shaderBufferProp, computeBuffer);
    }

    protected virtual void Release()
    {
        if (computeBuffer != null) computeBuffer.Release();
        if (argsBuffer != null) argsBuffer.Release();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Refresh();
    }

    protected virtual void Refresh()
    {
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, new Bounds(this.transform.position, areaScale), argsBuffer);
        computeShader.Dispatch(0, Mathf.CeilToInt((float)instanceCount / 8), 1, 1);
    }

    protected virtual void OnDestroy()
    {
        Release();
    }
}
