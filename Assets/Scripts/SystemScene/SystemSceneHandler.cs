using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSceneHandler : MonoBehaviour
{

    /** Planet sizes
     *      Rock: 8-11
     *      Gas: 13-16
     *          Moon: 5-7
     *  
     *  Star sizes
     *      Small: 18-24
     *      Medium: 26-32
     *      Large: 34-40
     * 
     * */

    public GameManager Game { get; set; }
    public BezierCurveHandler Curve { get; set; }
    public StarSystemHandler System { get; set; }
    public Ship Ship { get; set; }
    public List<Vector3> StarPos { get; set; }
    public List<Vector3> PlanetPos { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Ship.StartShipCoroutine(2);

        System.LoadStarSystems();
        SetStarPositions();
        SetPlanetPositions();
    }

    private void Awake() {
        Game = FindObjectOfType<GameManager>();
        if (!Game) {
            Debug.LogError("GAME MANAGER is NULL in MAPCONTROLLER!");
        } else {
            Game.SystemScene = this;
        }

        Ship = FindObjectOfType<Ship>();
        if (!Ship) {
            Debug.LogError("SHIP is NULL in MAPCONTROLLER!");
        }

        System = GetComponent<StarSystemHandler>();
        System.StarSystem = Game.SavedTargetSystem;
        

        //TODO: Find better way to set positions.
        StarPos = new List<Vector3> {
            new Vector3( 0.0f, 0.0f, 0.0f),
            new Vector3(45.0f, 0.0f, 0.0f),
            new Vector3(90.0f, 0.0f, 0.0f)
        };

        PlanetPos = new List<Vector3> {
            new Vector3( 0.0f,   0.0f, 0.0f),
            new Vector3( 8.0f,  -18.0f, 0.0f),
            new Vector3(16.0f,  -36.0f, 0.0f),
            new Vector3(24.0f,  -54.0f, 0.0f),
            new Vector3(32.0f,  -72.0f, 0.0f),
            new Vector3(40.0f,  -90.0f, 0.0f),
            new Vector3(48.0f, -108.0f, 0.0f),
            new Vector3(56.0f, -126.0f, 0.0f)
        };


        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/BezCurve"));
        Curve = obj.GetComponent<BezierCurveHandler>();
        //TDOD: Find better way to set curve.
        List<Vector3> newCtrlPoints = new List<Vector3> {
            new Vector3( 20.0f, -175.0f, 100.0f),
            new Vector3(-60.0f,   15.0f, 100.0f),
            new Vector3(-65.0f,   55.0f, 100.0f),
            new Vector3(-35.0f,   90.0f, 100.0f)
        };
        Curve.Curve.CtrlPoints = newCtrlPoints;
        Curve.InitLineRendererPoints();
        Curve.GetComponent<LineRenderer>().widthMultiplier = 1.0f;
    }

    private void SetStarPositions() {
        for (int i = 0; i < System.Stars.Count; i++) {
            System.Stars[i].transform.localPosition = StarPos[i];
        }
    }

    private void SetPlanetPositions() {
        for (int i = 0; i < System.Planets.Count; i++) {
            System.Planets[i].transform.localPosition = PlanetPos[i];
        }
    }
}
