using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Moon : CelestialBody
{
    public enum TYPE { ARID, ICE, VOLCANIC, LIVABLE }

    public TYPE Type { get; set; }


    public Moon() {
        Type = (TYPE)Random.Range(0, Enum.GetNames(typeof(TYPE)).Length);
    }
}
