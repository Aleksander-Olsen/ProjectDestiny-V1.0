using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationPanel : MonoBehaviour
{

    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Button destinationButton;

    private StarSystemHandler starSystem;
    private Ship ship;

    private bool canDisable = true;
    private float divergentZ;

    public Button DestinationButton { get => destinationButton; }
    public GameManager Game { get; set; }

    private void Awake() {
        
        Game = FindObjectOfType<GameManager>();
        if (!Game) {
            Debug.LogError("Game is NULL in DestinationPanel");
        }

        ship = FindObjectOfType<Ship>();
        if (!ship) {
            Debug.LogError("ship is NULL in DestinationPanel");
        }
    }

    void Update() {
        // Disable "set destination" button if ship is past divergent point, or already on side path.
        if(ship.transform.position.z >= divergentZ || !ship.OnAutopilot) {
            DestinationButton.interactable = false;
        }
    }

    public void Activate(StarSystemHandler _starSystem) {
        canDisable = false;
        gameObject.SetActive(true);

        // Find the point of divergent from main path
        float divergentT = _starSystem.StarSystem.CurveT - Game.Map.GetPathStarSeperation();
        int curveIndex = Game.Map.GetCurveIndex(_starSystem.StarSystem.CurveAttachedTo);
        if(divergentT < 0.0f) {
            divergentT += 1.0f;
            curveIndex -= 1;
        }
        divergentZ = Game.Map.MainPath[curveIndex].Curve.FindPointOnBezCurve(divergentT).z;

        DestinationButton.interactable = true;

        if (!_starSystem) {
            Debug.LogError("StarSystem is NULL in DestinationPanel");
        } else {
            starSystem = _starSystem;
            infoText.text = string.Format("Star Type: {0}\n" +
                "Number of planets in system: {1}",
                starSystem.StarSystem.Stars[0].Type,
                starSystem.StarSystem.Planets.Count);

            buttonText.text = "Set Destination";

            if (Game.Map.TargetStarSystem != null) {
                if (Game.Map.TargetStarSystem == starSystem) {
                    buttonText.text = "Unset Destination";
                }
            }
        }

        StartCoroutine(DisableDelay());
    }

    public void SetDestination() {
        if(Game.Map.TargetStarSystem == null) {
            Game.Map.SetDestinationPath(starSystem);
            buttonText.text = "Unset Destination";
        } else {
            if(Game.Map.TargetStarSystem != starSystem) {
                Game.Map.SetDestinationPath(starSystem);
                buttonText.text = "Unset Destination";
            } else {
                Game.Map.SetDestinationPath(null);
                buttonText.text = "Set Destination";
            }
        }

    }

    public void DeactivateDestination() {
        if (canDisable) {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DisableDelay() {
        yield return new WaitForSeconds(0.2f);
        canDisable = true;
    }

}
