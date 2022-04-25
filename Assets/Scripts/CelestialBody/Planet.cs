using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : CelestialBody
{
    private const float ROCK_MIN_SIZE = 8.0f;
    private const float GAS_MIN_SIZE = 13.0f;
    private const float SIZE_STEP = 3.0f;

    public enum TYPE { ROCK, GAS }
    public enum ROCK_TYPE { ICE, DESSERT, JUNGLE, WATER, ARID }
    public enum GAS_TYPE { HELIUM, HYDROGEN }

    public TYPE Type { get; set; }
    public ROCK_TYPE RockType { get; set; }
    public GAS_TYPE GasType { get; set; }
    public List<Moon> Moons { get; set; }

    public Planet() {
        Moons = new List<Moon>();
        Type = (TYPE)Random.Range(0, Enum.GetNames(typeof(TYPE)).Length);
        if(Type == TYPE.ROCK) {
            RockType = (ROCK_TYPE)Random.Range(0, Enum.GetNames(typeof(ROCK_TYPE)).Length);
        } else {
            GasType = (GAS_TYPE)Random.Range(0, Enum.GetNames(typeof(GAS_TYPE)).Length);
        }

        SetScale();
    }

    protected override void SetScale() {

        switch (Type) {
            case TYPE.ROCK:
                Scale = Random.Range(ROCK_MIN_SIZE, ROCK_MIN_SIZE + SIZE_STEP);
                break;
            case TYPE.GAS:
                Scale = Random.Range(GAS_MIN_SIZE, GAS_MIN_SIZE + SIZE_STEP);
                break;
            default:
                Debug.LogError("ERROR WITH PLANET SCALE.");
                break;
        }
    }
}
