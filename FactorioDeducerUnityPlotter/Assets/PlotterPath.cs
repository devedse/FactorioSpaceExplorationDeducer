using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotterPath : MonoBehaviour
{
    private readonly List<decimal> expectedValues = new List<decimal>()
    {
        0.25686095751884m,
        -0.89310642318349m,
        -0.36930118517397m
    };

    private List<Sv> coords;

    private readonly List<decimal> v = new List<decimal>()
    {
        0.18983879552606m, // 0
        0.20847820715387m, // 1
        0.3373248250886m,  // 2
        0.39831700267993m, // 3
        0.6444904486331m,  // 4
        0.73564182776852m, // 5
        0.85296865578697m, // 6
        0.9818152737217m, // 7
    };

    public PlotterPath()
    {

        coords = new List<Sv>()
        {
            new Sv(v[4], -v[1], v[5]),
            new Sv(-v[6], -v[2], v[3]),
            new Sv(v[2], v[3], -v[6]),
            new Sv(-v[0], -v[7], 0), // eindigt op 0
                
            new Sv(v[1], -v[5], v[4]),
            new Sv(v[6], v[2], v[3]),
            new Sv(v[5], v[4], -v[1]),

            new Sv(v[0], -v[7], 0), //begint bij 0
            new Sv(v[7], 0, -v[0]),
            new Sv(-v[7], 0, -v[0]),


            new Sv(v[6], v[2], -v[3]), //Eerste zonder 0
            new Sv(v[6], -v[2], v[3]),
            new Sv(-v[1], -v[5], -v[4]),
            new Sv(v[3], -v[6], v[2]),
            new Sv(-v[2], -v[3], v[6])
        };
    }

    public GameObject prefabje;
    public GameObject prefabjeFoen;
    public GameObject prefabjeBlue;
    public GameObject prefabLijn;

    // Start is called before the first frame update
    void Start()
    {
        var mul = 10.0f;

        var it = new CombinationIterator(8, coords.Count);
        var destVectorRichtingFoenestra = new Sv(expectedValues[0], expectedValues[1], expectedValues[2]);
        var destVectorTerug = new Sv(-expectedValues[0], -expectedValues[1], -expectedValues[2]);

        var dest = destVectorRichtingFoenestra;

        var summedList = it
           .Select(t => new { ItemsToSelect = it, Data = coords.GetElementsAt(t) })
           .Select(t => new { ItemsToSelect = t.ItemsToSelect, Data = t.Data, Sum = SumSv(t.Data) })
           .OrderBy(t => DistTwoVectors(dest, t.Sum.Normalized))
           .ToList();

        float brrr = 1;

        foreach (var dist in summedList.Take(1))
        {
            var cur = Vector3.zero;
            foreach (var item in dist.Data)
            {
                DrawBol(cur * mul, (item.ToVector3() + cur) * mul, prefabje, new Color(1 - brrr, brrr, 0));
                cur = item.ToVector3() + cur;
            }

            DrawBol(cur * mul, dist.Sum.Normalized.ToVector3() * mul, prefabjeBlue, new Color(1 - brrr, brrr, 255));
            DrawLine(Vector3.zero, cur * mul, new Color(1 - brrr, brrr, 255));
            brrr = brrr - 0.1f;
        }
    }

    private void DrawBol(Vector3 from, Vector3 to, GameObject prefab, Color lineColor)
    {
        var ga = GameObject.Instantiate(prefab);
        ga.transform.SetParent(this.transform, true);
        ga.transform.localPosition = to;
        DrawLine(from, to, lineColor, 10000f);
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = GameObject.Instantiate(prefabLijn);
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public decimal DistTwoVectors(Sv a, Sv b)
    {
        var x = Math.Abs(a.X - b.X);
        var y = Math.Abs(a.Y - b.Y);
        var z = Math.Abs(a.Z - b.Z);
        return x + y + z;
    }

    public Sv SumSv(IEnumerable<Sv> svs)
    {
        decimal x = svs.Select(t => t.X).Sum();
        decimal y = svs.Select(t => t.Y).Sum();
        decimal z = svs.Select(t => t.Z).Sum();

        return new Sv(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
