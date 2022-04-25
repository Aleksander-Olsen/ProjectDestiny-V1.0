using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Star : CelestialBody
{
    private const float SMALL_SIZE = 18.0f;
    private const float MEDIUM_SIZE = 26.0f;
    private const float LARGE_SIZE = 34.0f;
    private const float SIZE_STEP = 6.0f;
    public enum TYPE { BLUE, WHITE, YELLOW, RED };

    public TYPE Type { get; set; }

    public Star() {
        Type = (TYPE)Random.Range(0, Enum.GetNames(typeof(TYPE)).Length);
        SetScale();
    }

    protected override void SetScale() {
        
        switch (size) {
            case SIZE.SMALL:
                Scale = Random.Range(SMALL_SIZE, SMALL_SIZE + SIZE_STEP);
                break;
            case SIZE.MEDIUM:
                Scale = Random.Range(MEDIUM_SIZE, MEDIUM_SIZE + SIZE_STEP);
                break;
            case SIZE.LARGE:
                Scale = Random.Range(LARGE_SIZE, LARGE_SIZE + SIZE_STEP);
                break;
            default:
                Debug.LogError("ERROR WITH STAR SCALE.");
                break;
        }
    }

    /*private void OnMouseDown() {
        guiHandler.DestinationPanel.GetComponent<DestinationPanel>().Activate(transform.parent.gameObject);
    }*/
}
