using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem
{

    public List<Star> Stars { get; private set; }
    public List<Planet> Planets { get; private set; }
    public BezierCurve CurveAttachedTo { get; set; }
    public float CurveT { get; set; }
    public float XPos { get; set; }


    public StarSystem() {
        InitStars();
        InitPlanets();
    }

    private void InitPlanets() {
        Planets = new List<Planet>();
        float temp = Random.Range(0.0f, 100.0f);
        int numPlanets;

        if (temp <= 5.0f) {
            numPlanets = 1;
        } else if (temp <= 15.0f) {
            numPlanets = 2;
        } else if (temp <= 35.0f) {
            numPlanets = 3;
        } else if (temp <= 60.0f) {
            numPlanets = 4;
        } else if (temp <= 80.0f) {
            numPlanets = 5;
        } else if (temp <= 90.0f) {
            numPlanets = 6;
        } else if (temp <= 99.0f) {
            numPlanets = 7;
        } else {
            numPlanets = 8;
        }

        for (int i = 0; i < numPlanets; i++) {
            Planets.Add(new Planet());
        }
    }

    private void InitStars() {
        Stars = new List<Star>();
        float temp = Random.Range(0.0f, 100.0f);
        int numStars;

        if (temp <= 50.0f) {
            numStars = 1;
        } else if (temp <= 80.0f) {
            numStars = 2;
        } else {
            numStars = 3;
        }

        for (int i = 0; i < numStars; i++) {
            Stars.Add(new Star());
        }
    }
}
