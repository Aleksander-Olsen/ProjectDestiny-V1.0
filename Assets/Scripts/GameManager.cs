using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player Player { get; set; }
    public MapController Map { get; set; }
    public GuiHandler GUI { get; set; }
    public SystemSceneHandler SystemScene { get; set; }

    public List<BezierCurve> SavedPath { get; set; }
    public List<StarSystem> SavedSystems { get; set; }
    public BezierCurve SavedPathToStar { get; set; }
    public BezierCurve SavedPathFromStar { get; set; }
    public StarSystem SavedTargetSystem { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);

        Player = new Player();
        SavedPath = new List<BezierCurve>();
        SavedSystems = new List<StarSystem>();

    }

    public void LoadScene(string name) {
        StartCoroutine(LoadSceneAsync(name));
    }

    public void SaveMap() {
        SavedPath.Clear();
        SavedSystems.Clear();

        foreach (BezierCurveHandler curve in Map.MainPath) {
            SavedPath.Add(curve.Curve);
            foreach (StarSystemHandler system in curve.gameObject.GetComponentsInChildren<StarSystemHandler>()) {
                SavedSystems.Add(system.StarSystem);
            }
                
        }

        SavedPathFromStar = Map.PathFromStar.Curve;
        SavedPathToStar = Map.PathToStar.Curve;
        SavedTargetSystem = Map.TargetStarSystem.StarSystem;
    }

    private IEnumerator LoadSceneAsync(string name) {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
