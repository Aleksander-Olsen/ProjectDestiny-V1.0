using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSystemHandler : MonoBehaviour
{
    private const string STAR_MATERIAL_PATH = "Materials/Stars/mat_";
    private const string PLANET_MATERIAL_PATH = "Materials/Planets/";

    public StarSystem StarSystem { get; set; }
    public List<GameObject> Stars { get; set; }
    public List<GameObject> Planets { get; set; }
    public List<GameObject> Moons { get; set; }
    public GameManager Game { get; set; }

    private SphereCollider sphereCollider;

    private void Awake() {
        Game = FindObjectOfType<GameManager>();
        if (!Game) {
            Debug.LogError("GAME MANAGER is NULL in MAPCONTROLLER!");
        }
        
        Stars = new List<GameObject>();
        Planets = new List<GameObject>();
        Moons = new List<GameObject>();

        sphereCollider = GetComponent<SphereCollider>();
    }

    public void Init() {
        StarSystem = new StarSystem();
        LoadStarSystems();
        
    }

    public void LoadStarSystems() {
        foreach (Star star in StarSystem.Stars) {
            
            GameObject newStar = Instantiate(Resources.Load<GameObject>("Prefabs/Star"), transform.position, Quaternion.identity);
            newStar.transform.localScale = new Vector3(star.Scale, star.Scale, star.Scale);
            newStar.transform.parent = transform.GetChild(0);
            
            
            newStar.GetComponent<MeshRenderer>().material = Resources.Load<Material>(STAR_MATERIAL_PATH + "Star" + star.Type);
            
            Stars.Add(newStar);

            

            if (sphereCollider != null) {
                if (star.Scale > sphereCollider.radius) {
                    sphereCollider.radius = star.Scale + 0.1f;
                }
            }

            
        }
        
        Vector3 distance = new Vector3(22.0f, 0.0f, 0.0f);

        if(StarSystem.Stars.Count == 2) {
            Stars[0].transform.localPosition = distance;
            Stars[1].transform.localPosition = -distance;
        }

        if (StarSystem.Stars.Count == 3) {
            Stars[0].transform.localPosition = distance;
            distance = Quaternion.AngleAxis(120, Vector3.up) * distance;
            Stars[1].transform.localPosition = distance;
            distance = Quaternion.AngleAxis(120, Vector3.up) * distance;
            Stars[2].transform.localPosition = distance;
        }

        
        if (Game.SystemScene) {
            foreach (Planet planet in StarSystem.Planets) {
                GameObject newPlanet = Instantiate(Resources.Load<GameObject>("Prefabs/Planet"), transform.position, Quaternion.identity);
                newPlanet.transform.localScale = new Vector3(planet.Scale, planet.Scale, planet.Scale);
                newPlanet.transform.parent = transform.GetChild(1);
                if(planet.Type == Planet.TYPE.ROCK) {
                    newPlanet.GetComponent<MeshRenderer>().material = Resources.Load<Material>(PLANET_MATERIAL_PATH + planet.Type + "/mat_Planet" + planet.RockType);
                } else {
                    newPlanet.GetComponent<MeshRenderer>().material = Resources.Load<Material>(PLANET_MATERIAL_PATH + planet.Type + "/mat_Planet" + planet.GasType);
                }
                
                Planets.Add(newPlanet);

                foreach (Moon moon in planet.Moons) {
                    GameObject newMoon = Instantiate(Resources.Load<GameObject>("Prefabs/Moon"), transform.position, Quaternion.identity);
                    newMoon.transform.localScale = new Vector3(moon.Scale, moon.Scale, moon.Scale);
                    newMoon.transform.parent = transform.GetChild(1);
                    Moons.Add(newMoon);
                }
            }
        } else {
            transform.Rotate(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
            StartCoroutine(RotateRoutine());
        }
    }

    private void OnMouseDown() {
        Game.GUI.DestinationPanel.GetComponent<DestinationPanel>().Activate(this);
    }

    private IEnumerator RotateRoutine() {
        float rotSpeed = Random.Range(20.0f, 60.0f);
        while (true) {
            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
            yield return null;
        }
    }
}
