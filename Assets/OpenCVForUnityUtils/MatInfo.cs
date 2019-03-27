using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.CoreModule;

public class MatInfo
{
    public int Columns;
    public int Rows;
    public int Type;
    public double[,][] Grid;

    public MatInfo() { }

    public MatInfo(Mat mat)
    {
        this.Columns = mat.cols();
        this.Rows = mat.rows();
        this.Type = mat.type();
        Grid = new double[mat.rows(), mat.cols()][];
        for (var r = 0; r < mat.rows(); r++)
        {
            for (var c = 0; c < mat.cols(); c++)
            {
                Grid[r, c] = mat.get(r, c);
            }
        }
    }

    public Mat ToMat()
    {
        var mat = new Mat(Rows, Columns, Type);
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                mat.put(r, c, Grid[r, c]);
            }
        }
        return mat;
    }

    //public List<MatElement> MatElements;

    //public MatInfo(Mat mat)
    //{
    //    MatElements = new List<MatElement>();
    //    for (var r = 0; r < mat.rows(); r++)
    //    {
    //        for (var c = 0; c < mat.cols(); c++)
    //        {
    //            var e = new MatElement();
    //            e.Row = r;
    //            e.Colmn = c;
    //            e.Values = mat.get(r, c);
    //            MatElements.Add(e);
    //        }
    //    }
    //}

    //public Mat ToMat()
    //{
    //    var mat = new Mat(Rows, Columns, Type);
    //    foreach (var e in MatElements)
    //    {
    //        mat.put(e.Row, e.Colmn, e.Values);
    //    }
    //    Debug.Log(mat.cols());
    //    Debug.Log(Columns);
    //    return mat;
    //}
}

public class MatElement
{
    public int Row;
    public int Colmn;
    public double[] Values;
}
