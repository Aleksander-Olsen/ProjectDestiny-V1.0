using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameManager Game { get; set; }


    private void Awake() {
        Game = FindObjectOfType<GameManager>();
    }

    public void NewGame() {
        Game.LoadScene("TravelSceneBezier");
    }
}
