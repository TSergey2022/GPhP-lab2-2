using System.Collections.Generic;
using UnityEngine;

public class OrbitalMotion : MonoBehaviour
{
    static List<OrbitalMotion> list = new();
    public Transform centralBody;  // The body at the center of the world (e.g., planet or star)
    public Rigidbody orbitingBody; // The orbiting body (e.g., satellite or moon)
    public float gravityConstant = 1.0f;  // Strength of the gravitational force
    public float initialVelocity = 10f;   // Initial tangential velocity of the orbiting body

    void Awake() {
        list.Add(this);
    }

    void Start()
    {
        if (orbitingBody.isKinematic) return;
        // Make sure the central body doesn't move by freezing its Rigidbody (optional)
        Rigidbody centralBodyRigidbody = centralBody.GetComponent<Rigidbody>();
        if (centralBodyRigidbody != null)
        {
            centralBodyRigidbody.isKinematic = true;  // Prevent central body from moving
        }

        // Set the initial velocity to create orbital motion (tangential to the central body)
        Vector3 tangentDirection = (orbitingBody.transform.right).normalized; // Right direction for tangential velocity
        orbitingBody.velocity = tangentDirection * initialVelocity;
    }

    void FixedUpdate()
    {
        if (orbitingBody.isKinematic) return;
        foreach (var motion in list) {
            if (motion == this) continue;
            // Calculate the direction from the orbiting body to the central body
            // Vector3 directionToCenter = centralBody.position - orbitingBody.position;
            Vector3 directionToCenter = motion.gameObject.transform.position - orbitingBody.position;

            // Calculate the distance between the two bodies
            float distance = directionToCenter.magnitude;

            // Normalize the direction and calculate gravitational force magnitude
            // Vector3 gravitationalForce = directionToCenter.normalized * (gravityConstant * orbitingBody.mass * centralBody.GetComponent<Rigidbody>().mass) / Mathf.Pow(distance, 2);
            Vector3 gravitationalForce = directionToCenter.normalized * orbitingBody.mass * motion.gameObject.GetComponent<Rigidbody>().mass / Mathf.Pow(distance, 2);

            // Apply gravitational force to the orbiting body
            orbitingBody.AddForce(gravitationalForce);
        }
    }
}
