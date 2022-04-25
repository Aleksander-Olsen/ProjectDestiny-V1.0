using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveHandler : MonoBehaviour {
    private const float CURVE_LENGTH = 500.0f;
    private const float MIN_CURVE_SWAY = 50.0f;
    private const float MAX_CURVE_SWAY = 160.0f;
    private const float STEP = 0.01f;

    private const float MIN_STAR_DIST = 100.0f;
    private const float MAX_STAR_DIST = 160.0f;
    private const float MIN_STAR_SEPERATION = 0.15f;
    private const float MAX_STAR_SEPERATION = 0.3f;

    private LineRenderer lineRenderer;

    public GameManager Game { get; set; }
    public List<Vector3> LineRenderPoints { get; set; }
    public BezierCurve Curve { get; set; }


    void Awake() {
        Game = FindObjectOfType<GameManager>();
        if (!Game) {
            Debug.LogError("GAME MANAGER is NULL in MAPCONTROLLER!");
        }

        lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer) {
            Debug.LogError("lineRenderer is empty in BezierCurve");
        }

        LineRenderPoints = new List<Vector3>();
        Curve = new BezierCurve();
    }

    public void Init(BezierCurve lastCurve = null) {
        List<Vector3> newCtrlPoints = new List<Vector3>();
        SIDE side;

        if (lastCurve == null) {
            side = (SIDE)(Random.Range(0, 2) * 2 - 1);

            newCtrlPoints.Add(new Vector3(0.0f, StaticGlobals.Y, transform.position.z));
            newCtrlPoints.Add(new Vector3(
                transform.position.x + (Random.Range(MIN_CURVE_SWAY, MAX_CURVE_SWAY) * (int)side),
                0.0f,
                transform.position.z + CURVE_LENGTH));
            newCtrlPoints.Add(new Vector3(
                transform.position.x + (Random.Range(MIN_CURVE_SWAY, MAX_CURVE_SWAY) * (int)side),
                0.0f,
                transform.position.z + (CURVE_LENGTH * 2)));
            newCtrlPoints.Add(new Vector3(0.0f, 0.0f, transform.position.z + CURVE_LENGTH * 3));

        } else {
            transform.position = lastCurve.CtrlPoints[lastCurve.CtrlPoints.Count - 1];
            side = (SIDE)((int)lastCurve.Side * -1);

            newCtrlPoints.Add(transform.position);
            // the second new control point is mirrored of the second to last control point of previous curve.
            newCtrlPoints.Add(new Vector3(
                lastCurve.CtrlPoints[lastCurve.CtrlPoints.Count - 2].x * -1,
                0.0f,
                lastCurve.CtrlPoints[lastCurve.CtrlPoints.Count - 1].z + CURVE_LENGTH
                ));

            newCtrlPoints.Add(new Vector3(
                Random.Range(MIN_CURVE_SWAY, MAX_CURVE_SWAY) * (int)side,
                0.0f,
                newCtrlPoints[newCtrlPoints.Count - 1].z + CURVE_LENGTH
                ));

            newCtrlPoints.Add(new Vector3(
                0.0f,
                0.0f,
                newCtrlPoints[newCtrlPoints.Count - 1].z + CURVE_LENGTH
                ));
        }

        Curve.CtrlPoints = newCtrlPoints;
        Curve.Side = side;

        InitLineRendererPoints();
        SpawnStarSystems();
    }


    public void ClearCurve() {
        Curve.CtrlPoints.Clear();
        LineRenderPoints.Clear();
        lineRenderer.positionCount = 0;
    }

    public void InitLineRendererPoints() {
        float t = 0.0f;

        while (t < 1.0f) {
            LineRenderPoints.Add(Curve.FindPointOnBezCurve(t));
            t += STEP;
        }

        lineRenderer.positionCount = LineRenderPoints.Count;
        lineRenderer.SetPositions(LineRenderPoints.ToArray());
    }

    public void LoadStarSystems() {
        GameObject newSystem;

        foreach (StarSystem system in Game.SavedSystems) {
            if(system.CurveAttachedTo == Curve) {
                Vector3 systemPos = Curve.FindPointOnBezCurve(system.CurveT);
                systemPos.x = system.XPos;

                newSystem = Instantiate(Resources.Load<GameObject>("Prefabs/StarSystem"), systemPos, Quaternion.identity);
                newSystem.transform.parent = transform;
                newSystem.GetComponent<StarSystemHandler>().StarSystem = system;
                newSystem.GetComponent<StarSystemHandler>().LoadStarSystems();

                if(newSystem.GetComponent<StarSystemHandler>().StarSystem == Game.SavedTargetSystem) {
                    Game.Map.TargetStarSystem = newSystem.GetComponent<StarSystemHandler>();
                }
            }
        }
    }

    private void SpawnStarSystems() {
        float t = Random.Range(MIN_STAR_SEPERATION, MAX_STAR_SEPERATION);

        while (t <= 0.9f) {
            Vector3 systemPos = Curve.FindPointOnBezCurve(t);

            SIDE side = (SIDE)(Random.Range(0, 2) * 2 - 1);
            float distance = (Random.Range(MIN_STAR_DIST, MAX_STAR_DIST) * (int)side);

            systemPos.x += distance;

            GameObject newSystem = Instantiate(Resources.Load<GameObject>("Prefabs/StarSystem"), systemPos, Quaternion.identity);
            newSystem.transform.parent = transform;
            newSystem.GetComponent<StarSystemHandler>().Init();
            newSystem.GetComponent<StarSystemHandler>().StarSystem.CurveAttachedTo = Curve;
            newSystem.GetComponent<StarSystemHandler>().StarSystem.CurveT = t;
            newSystem.GetComponent<StarSystemHandler>().StarSystem.XPos = systemPos.x;

            t += Random.Range(MIN_STAR_SEPERATION, MAX_STAR_SEPERATION);
        }
    }
}
