using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiHandler : MonoBehaviour {
    
    [SerializeField]
    private GameObject destinationPanel;
    [SerializeField]
    private GameObject resourcePanel;

    public GameObject DestinationPanel { get => destinationPanel; set => destinationPanel = value; }
    public GameObject ResourcePanel { get => resourcePanel; set => resourcePanel = value; }
    public GameManager Game { get; set; }

    private void Awake() {
        Game = FindObjectOfType<GameManager>();
        if(!Game) {
            Debug.LogError("Game is NULL in GuiHandler");
        } else {
            Game.GUI = this;
        }
    }
}
