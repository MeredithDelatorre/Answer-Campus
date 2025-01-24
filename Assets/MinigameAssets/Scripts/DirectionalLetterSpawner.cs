using UnityEngine;
using System.Collections;

public class DirectionalLetterSpawner : LetterSpawner
{
    // Override only the method to define directional spawning logic
    protected override Vector3 GetSpawnPosition()
    {
        float[] angles = { 0f, 90f, 180f, 270f }; // Fixed angles for directional spawning
        float angle = angles[Random.Range(0, angles.Length)] * Mathf.Deg2Rad;
        float radius = 10f; // Fixed radius for spawn positions
        return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
    }
}
