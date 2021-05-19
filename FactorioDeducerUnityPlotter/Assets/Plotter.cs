using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plotter : MonoBehaviour
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

    public Plotter()
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
    public GameObject prefabLijn;

    // Start is called before the first frame update
    void Start()
    {
        var mul = 10.0f;
        var dfoen = new Sv(expectedValues[0], expectedValues[1], expectedValues[2]);

        DrawBol(Vector3.zero, dfoen.ToVector3() * mul, prefabjeFoen, Color.red);

        foreach (var value in coords)
        {
            DrawBol(Vector3.zero, value.ToVector3() * mul, prefabje, Color.blue);
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

    // Update is called once per frame
    void Update()
    {

    }
}
