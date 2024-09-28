using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "ScriptableObjects/PlanetSO", order = 1)]
public class PlanetSO : ScriptableObject
{
    public string Name = "Sun";
    public float Distance = 0.0f;
    public float Mass = 1.989E30f;
    public float Radius = 696.340f;
    public Material Material;
}
