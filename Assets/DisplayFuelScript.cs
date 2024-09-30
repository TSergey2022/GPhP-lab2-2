using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFuelScript : MonoBehaviour
{
    DickShip ds;
    Text text;

    void Start()
    {
        ds = GameObject.FindObjectOfType<DickShip>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Fuel: {(int)(ds.fuelMass / 1e-8f)}";
    }
}
