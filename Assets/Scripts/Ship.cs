using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    private const float defaultSpeed = 0.05f;
    private const float rotationSpeed = 5.0f;

    private BezierCurve currentCurve;

    private float shipSpeed = defaultSpeed;

    public float ShipT { get; set; }
    public GameManager Game { get; set; }
    public bool OnAutopilot { get; set; } = true;



    void Awake()
    {
        Game = FindObjectOfType<GameManager>();
        if (!Game) {
            Debug.LogError("Game Manager is NULL in Ship");
        }

        ShipT = 0.0f;
    }

    public void StartShipCoroutine(int i = 0, bool toStarSystem = true) {
        switch (i) {
            case 0:
                StartCoroutine(AutoPilot());
                break;
            case 1:
                StartCoroutine(FollowDestination(toStarSystem));
                break;
            case 2:
                StartCoroutine(StarSystemRoutine());
                break;
            default:
                break;
        }
    }

    private void MoveAndRotate() {
        Vector3 point = currentCurve.FindPointOnBezCurve(ShipT);

        Vector3 targetPoint = (point - transform.position).normalized;


        if (targetPoint != Vector3.zero) {
            // Calculate a rotation a step closer to the target and applies rotation to this object
            Quaternion lookRotation = Quaternion.LookRotation(targetPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        transform.position = point;

        ShipT += shipSpeed * Time.deltaTime;
    }

    private IEnumerator AutoPilot(float _shiptT = 0.0f) {
        OnAutopilot = true;
        shipSpeed = defaultSpeed;
        currentCurve = Game.Map.MainPath[1].Curve;

        ShipT = _shiptT;

        while (OnAutopilot) {
            MoveAndRotate();


            // End of bezier curve, reset curve position and run end of cruve protocol.
            if (ShipT >= 1.0f) {
                ShipT = 0.0f;
                Game.Map.EndOfCurve();
                currentCurve = Game.Map.MainPath[1].Curve;
            }

            if(Game.Map.HasReachedDestination(transform.position.z)) {
                OnAutopilot = false;
            }

            yield return null;
        }

        StartCoroutine(FollowDestination());
    }

    private IEnumerator FollowDestination(bool starting = true) {
        OnAutopilot = false;
        Game.GUI.DestinationPanel.GetComponent<DestinationPanel>().DestinationButton.interactable = false;
        
        ShipT = 0.0f;
        shipSpeed = defaultSpeed * 5.0f;
        currentCurve = Game.Map.PathToStar.Curve;
        bool onStart = starting;

        while (onStart) {
            MoveAndRotate();

            if (ShipT >= 1.0f) {
                onStart = false;
                ShipT = 0.0f;
                Game.SaveMap();
                Game.LoadScene("SystemScene");
                yield return new WaitForSeconds(2.0f);
            }

            yield return null;
        }

        currentCurve = Game.Map.PathFromStar.Curve;

        while (!onStart) {
            MoveAndRotate();
            
            if (ShipT >= 1.0f) {
                onStart = true;
                ShipT = 0.0f;
            }

            yield return null;
        }

        Game.GUI.DestinationPanel.GetComponent<DestinationPanel>().DestinationButton.interactable = true;
        Game.Map.TargetStarSystem = null;
        StartCoroutine(AutoPilot(Game.Map.ConvergenceT));
    }

    private IEnumerator StarSystemRoutine() {
        ShipT = 0.0f;
        shipSpeed = defaultSpeed; // / 5.0f;

        currentCurve = Game.SystemScene.Curve.Curve;

        while (ShipT <= 1.0f) {
            MoveAndRotate();
            yield return null;
        }

        Game.LoadScene("TravelSceneBezier");
    }
}
