using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    static public List<PlanetScript> list = new();

    public float gravityConstant = 1.0f;
    public float initialVelocity = 10f;
    public GameObject autoVelocityTarget;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb.isKinematic) return;
        Vector3 tangentDirection = (rb.transform.right).normalized;
        if (autoVelocityTarget != null) {
            var M = autoVelocityTarget.GetComponent<Rigidbody>().mass;
            var r = (autoVelocityTarget.transform.position - rb.position).magnitude;
            initialVelocity = Mathf.Sqrt(M / r);
        }
        rb.velocity = tangentDirection * initialVelocity;
    }

    void FixedUpdate()
    {
        if (rb.isKinematic) return;
        foreach (var motion in list) {
            if (motion == this) continue;
            Vector3 directionToCenter = motion.gameObject.transform.position - rb.position;
            float distance = directionToCenter.magnitude;
            Vector3 gravitationalForce = directionToCenter.normalized * rb.mass * motion.gameObject.GetComponent<Rigidbody>().mass / Mathf.Pow(distance, 2);
            rb.AddForce(gravitationalForce);
        }
    }

}
