using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const float PATH_STAR_SEPERATION = 0.15f;

    private bool newGame = false;

    public List<BezierCurveHandler> MainPath { get; set; }
    public BezierCurveHandler PathToStar { get; set; }
    public BezierCurveHandler PathFromStar { get; set; }
    public StarSystemHandler TargetStarSystem { get; set; }
    public GameManager Game { get; set; }
    public Ship Ship { get; set; }
    public float ConvergenceT { get; set; }
    public float DivergentT { get; set; }

    // Start is called before the first frame update
    void Awake() {
        MainPath = new List<BezierCurveHandler>();

        Ship = FindObjectOfType<Ship>();
        if (!Ship) {
            Debug.LogError("SHIP is NULL in MAPCONTROLLER!");
        }
        
        Game = FindObjectOfType<GameManager>();
        if(!Game) {
            Debug.LogError("GAME MANAGER is NULL in MAPCONTROLLER!");
        } else {
            Game.Map = this;

            if (Game.SavedPath.Count != 0) {
                LoadPath();
            } else {
                NewGame();
            }
        }
    }

    private void Start() {
        if(newGame) {
            Ship.StartShipCoroutine();
        } else {
            Ship.StartShipCoroutine(1, false);
        }
        
    }

    private void NewGame() {
        newGame = true;

        // Init first curve
        GameObject curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(0);
        MainPath.Add(curve.GetComponent<BezierCurveHandler>());
        MainPath[MainPath.Count - 1].Init();

        // Init three more curves
        for (int i = 0; i < 3; i++) {
            curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
            curve.transform.parent = transform.GetChild(0);
            MainPath.Add(curve.GetComponent<BezierCurveHandler>());
            MainPath[MainPath.Count - 1].Init(MainPath[MainPath.Count - 2].Curve);
        }

        curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(1);
        PathToStar = curve.GetComponent<BezierCurveHandler>();
        curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(1);
        PathFromStar = curve.GetComponent<BezierCurveHandler>();
    }

    private void LoadPath() {
        GameObject curve;

        foreach (BezierCurve bezCurve in Game.SavedPath) {
            curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
            curve.transform.parent = transform.GetChild(0);
            MainPath.Add(curve.GetComponent<BezierCurveHandler>());
            MainPath[MainPath.Count - 1].Curve = bezCurve;
            MainPath[MainPath.Count - 1].InitLineRendererPoints();
            MainPath[MainPath.Count - 1].LoadStarSystems();
        }

        DivergentT = 0.0f;
        ConvergenceT = TargetStarSystem.StarSystem.CurveT + PATH_STAR_SEPERATION;

        curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(1);
        PathFromStar = curve.GetComponent<BezierCurveHandler>();
        PathFromStar.Curve = Game.SavedPathFromStar;
        PathFromStar.InitLineRendererPoints();

        curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(1);
        PathToStar = curve.GetComponent<BezierCurveHandler>();
        PathToStar.Curve = Game.SavedPathToStar;
        PathToStar.InitLineRendererPoints();
    }

    public void EndOfCurve() {
        // Instaniate and init a new bezier curve and add it to path.
        GameObject curve = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        curve.transform.parent = transform.GetChild(0);
        MainPath.Add(curve.GetComponent<BezierCurveHandler>());
        MainPath[MainPath.Count - 1].Init(MainPath[MainPath.Count - 2].Curve);

        // Remove the oldest bezier curve in path.
        Destroy(MainPath[0].gameObject);
        MainPath.RemoveAt(0);
    }

    public bool HasReachedDestination(float shipZ) {
        if (TargetStarSystem != null) {
            if (shipZ >= PathToStar.GetComponent<BezierCurveHandler>().Curve.CtrlPoints[0].z) {
                return true;
            }
        }

        return false;
    }

    public void SetDestinationPath(StarSystemHandler _targetSystem = null) {
        PathToStar.GetComponent<BezierCurveHandler>().ClearCurve();
        PathFromStar.GetComponent<BezierCurveHandler>().ClearCurve();
        TargetStarSystem = _targetSystem;

        if (TargetStarSystem != null) {
            int curveIndex;
            Vector3 targetPos = TargetStarSystem.transform.position;
            targetPos.y = -0.1f;
            List<Vector3> tmpCtrlPoints = new List<Vector3>();


            // <--- Set start curve parameters
            curveIndex = GetCurveIndex(TargetStarSystem.StarSystem.CurveAttachedTo);
            DivergentT = TargetStarSystem.StarSystem.CurveT - PATH_STAR_SEPERATION;
            if (DivergentT < 0.0f) {
                DivergentT += 1.0f;
                curveIndex -= 1;
            }

            tmpCtrlPoints.Add(MainPath[curveIndex].Curve.FindPointOnBezCurve(DivergentT));
            tmpCtrlPoints.Add(MainPath[curveIndex].Curve.FindPointOnBezCurve(DivergentT + (PATH_STAR_SEPERATION / 2)));
            tmpCtrlPoints.Add(new Vector3(targetPos.x, targetPos.y, tmpCtrlPoints[1].z));
            tmpCtrlPoints.Add(targetPos);

            PathToStar.Curve.CtrlPoints.AddRange(tmpCtrlPoints);
            PathToStar.InitLineRendererPoints();
            // --->

            // <--- Set end curve parameters
            tmpCtrlPoints.Clear();
            curveIndex = GetCurveIndex(TargetStarSystem.StarSystem.CurveAttachedTo);
            ConvergenceT = TargetStarSystem.StarSystem.CurveT + PATH_STAR_SEPERATION;
            if (ConvergenceT > 1.0f) {
                ConvergenceT -= 1.0f;
                curveIndex += 1;
            }

            tmpCtrlPoints.Add(targetPos);
            tmpCtrlPoints.Add(MainPath[curveIndex].Curve.FindPointOnBezCurve(ConvergenceT - (PATH_STAR_SEPERATION / 2)));
            tmpCtrlPoints.Insert(1, new Vector3(targetPos.x, targetPos.y, tmpCtrlPoints[1].z));
            tmpCtrlPoints.Add(MainPath[curveIndex].Curve.FindPointOnBezCurve(ConvergenceT));

            PathFromStar.Curve.CtrlPoints.AddRange(tmpCtrlPoints);
            PathFromStar.InitLineRendererPoints();
            // --->

            tmpCtrlPoints.Clear();
        }
    }

    public int GetCurveIndex(BezierCurve _curve) {
        foreach (BezierCurveHandler curve in MainPath) {
            if (curve.Curve == _curve) {
                return MainPath.IndexOf(curve);
            }
        }

        return -1;
    }

    public float GetPathStarSeperation() {
        return PATH_STAR_SEPERATION;
    }
}
