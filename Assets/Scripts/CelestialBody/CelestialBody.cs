using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CelestialBody
{

    protected SIZE size;

    public float Scale { get; set; }

    public CelestialBody() {
        size = (SIZE)Random.Range(0, Enum.GetNames(typeof(SIZE)).Length);
    }

    protected virtual void SetScale() {}
}
