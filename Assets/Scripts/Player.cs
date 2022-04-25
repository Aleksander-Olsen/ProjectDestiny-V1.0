using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private const float P_DEC_RATE = 1.0f;
    private const float F_DEC_RATE = 0.1f;
    private const float W_DEC_RATE = 0.333f;


    private float power;
    private float food;
    private float water;

    private int crew;
    private int scientists;
    private int military;

    public float Power { get => power; set => power = value; }
    public float Food { get => food; set => food = value; }
    public float Water { get => water; set => water = value; }
    public int Crew { get => crew; set => crew = value; }
    public int Scientists { get => scientists; set => scientists = value; }
    public int Military { get => military; set => military = value; }

    public Player() {
        Power = Random.Range(15.0f, 40.0f);
        Food = Random.Range(40.0f, 70.5f);
        Water = Random.Range(50.0f, 80.0f);
    }


    // Start is called before the first frame update
    void Init()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease power, food, and water values over time.
        Power = Mathf.Clamp(Power - (P_DEC_RATE * Time.deltaTime), 0.0f, 100.0f);
        Food = Mathf.Clamp(Food - (F_DEC_RATE * Time.deltaTime), 0.0f, 100.0f);
        Water = Mathf.Clamp(Food - (W_DEC_RATE * Time.deltaTime), 0.0f, 100.0f);


        // If power, food, or water reaches 0, stop at next system for one last chance
        if (Power <= 0.0f || Food <= 0.0f || Water <= 0.0f) {

        }
    }
}
