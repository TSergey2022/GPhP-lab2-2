using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SolarSystem : MonoBehaviour
{
    public PlanetSO Sun;
    public PlanetSO Mercury;
    public PlanetSO Venus;
    public PlanetSO Earth;
    public PlanetSO Mars;
    public PlanetSO Jupiter;
    public PlanetSO Saturn;
    public PlanetSO Uranus;
    public PlanetSO Neptune;

    private List<PlanetSO> planets = new();
    
    public void SetupList() {
        planets.Clear();
        planets.Add(Sun);
        planets.Add(Mercury);
        planets.Add(Venus);
        planets.Add(Earth);
        planets.Add(Mars);
        planets.Add(Jupiter);
        planets.Add(Saturn);
        planets.Add(Uranus);
        planets.Add(Neptune);
    }

    public void SpawnPlanets() {
        for (int i = transform.childCount; i > 0; i--) {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(transform.GetChild(i - 1).gameObject);
#else
            GameObject.Destroy(transform.GetChild(i - 1).gameObject);
#endif
        }
        GameObject sun = null;
        PlanetScript.list.Clear();
        for (int i = 0; i < planets.Count; i++) {
            var planet = planets[i];
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = planet.Name;
            sphere.transform.localPosition = new Vector3(0, 0, planet.Distance);
            if (i == 0) sphere.transform.localScale *= Mathf.Sqrt(planet.Radius) * 4;
            else sphere.transform.localScale *= Mathf.Sqrt(planet.Radius) * 16;
            var collider = sphere.AddComponent<SphereCollider>();
            var rb = sphere.AddComponent<Rigidbody>();
            rb.isKinematic = i == 0;
            rb.useGravity = false;
            rb.mass = planet.Mass;
            var mesh = sphere.GetComponent<MeshRenderer>();
            mesh.material = planet.Material;
            var planetScript = sphere.AddComponent<PlanetScript>();
            PlanetScript.list.Add(planetScript);
            
            sphere.transform.SetParent(transform);

            if (i == 0) sun = sphere;
            else planetScript.autoVelocityTarget = sun;
        }
    }

    void Awake() {
        SetupList();
        SpawnPlanets();
    }
}

[CustomEditor(typeof(SolarSystem))]
public class SolarSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SolarSystem script = (SolarSystem)target;
        if (GUILayout.Button("Setup List"))
        {
            script.SetupList();
        }
        if (GUILayout.Button("Spawn Planets"))
        {
            script.SpawnPlanets();
        }
    }
}