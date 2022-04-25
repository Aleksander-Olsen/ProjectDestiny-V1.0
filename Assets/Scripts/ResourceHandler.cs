using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHandler : MonoBehaviour
{

    [SerializeField]
    private Slider power;
    [SerializeField]
    private Slider food;
    [SerializeField]
    private Slider water;

    private GameManager Game { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Game = FindObjectOfType<GameManager>();
        if(!Game) {
            Debug.LogError("Game Manager is null in RESOURCE_HANDLER");
        }

        power.value = Game.Player.Power;
        food.value = Game.Player.Food;
        water.value = Game.Player.Water;
    }

    // Update is called once per frame
    void Update()
    {
        power.value = Game.Player.Power;
        food.value = Game.Player.Food;
        water.value = Game.Player.Water;
    }
}
