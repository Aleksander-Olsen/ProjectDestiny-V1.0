using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve {

    public List<Vector3> CtrlPoints { get; set; }

    public SIDE Side { get; set; }

    public BezierCurve() {
        CtrlPoints = new List<Vector3>();
    }

    public Vector3 FindPointOnBezCurve(float t) {
        float oneMinusT = 1f - t;

        Vector3 p = Mathf.Pow(oneMinusT, 3) * CtrlPoints[0] +
             3f * Mathf.Pow(oneMinusT, 2) * t * CtrlPoints[1] +
             3f * oneMinusT * Mathf.Pow(t, 2) * CtrlPoints[2] +
             Mathf.Pow(t, 3) * CtrlPoints[3];

        return p;
    }
}
