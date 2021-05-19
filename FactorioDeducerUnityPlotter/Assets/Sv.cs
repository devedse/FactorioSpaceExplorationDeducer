using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Sv
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Z { get; set; }

    public Sv(decimal x, decimal y, decimal z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double VectorLength => Math.Sqrt((double)(X * X + Y * Y + Z * Z));

    public int CountCloseToZero()
    {
        int c = 0;
        if (MathHelpers.IsCloseToZero(X))
        {
            c++;
        }
        if (MathHelpers.IsCloseToZero(Y))
        {
            c++;
        }
        if (MathHelpers.IsCloseToZero(Z))
        {
            c++;
        }
        return c;
    }

    private Sv _normalized;
    public Sv Normalized
    {
        get
        {
            if (_normalized == null)
            {
                _normalized = new Sv(X / (decimal)VectorLength, Y / (decimal)VectorLength, Z / (decimal)VectorLength);
            }
            return _normalized;
        }
    }

    public Vector3 ToVector3()
    {
        return new Vector3((float)X, (float)Y, (float)Z);
    }
}