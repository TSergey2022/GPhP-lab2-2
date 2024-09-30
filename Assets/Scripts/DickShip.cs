using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DickShip : MonoBehaviour, IGravity {
    // Параметры корабля
    public float dryMass = 1000f; // Бесполезная масса (сухая масса корабля)
    public float fuelMass = 500f; // Масса топлива
    public float fuelConsumptionRate = 5f; // Потребление топлива за секунду (при движении вперёд)
    public float exhaustVelocity = 3000f; // Скорость выброса топлива (в метрах в секунду)

    // Параметры вращения
    public float rotationFuelConsumptionRate = 2.5f; // Потребление топлива при вращении (вдвое меньшее)
    public float rotationSpeed = 90f; // Скорость вращения (градусов в секунду)

    public float moveSpeed = 10f;

    private Rigidbody rb; // Reference to the Rigidbody component
    [SerializeField] private ParticleSystem ps;

    private float finalSpeed = 0;

    void Start()
    {
        // Get the Rigidbody component attached to the spaceship
        rb = GetComponent<Rigidbody>();

        // Optionally, freeze rotation on the X and Z axes to prevent unintended tilting
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        IGravity.grav.Add(this);
    }

    void Update() {
        var emission = ps.emission;
        emission.enabled = finalSpeed > 0;
    }

    void FixedUpdate() {
        // Handle forward movement (W key)
        if (Input.GetKey(KeyCode.W)) {
            MoveForward(Time.deltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            MoveBackward(Time.deltaTime);
        } else {
            finalSpeed = 0;
        }

        Rotate();
        foreach (var motion in IGravity.grav) {
            if (motion == (IGravity)this) {
                continue;
            }
            Vector3 directionToCenter = motion.GetTransform().position - rb.position;
            float distance = directionToCenter.magnitude;
            var planet = rb.GetComponent<PlanetScript>();
            if (planet != null) distance -= motion.GetTransform().localScale.x;
            Vector3 gravitationalForce = directionToCenter.normalized * rb.mass * motion.GetRigidbody().mass / Mathf.Pow(distance, 2);
            rb.AddForce(gravitationalForce);
        }
    }

    // Move the spaceship forward using Rigidbody
    private void MoveForward(float deltaTime)
    {
        finalSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed;
        // Add force in the forward direction based on moveSpeed
        rb.AddForce(transform.forward * finalSpeed, ForceMode.Acceleration);
    }
    private void MoveBackward(float deltaTime)
    {
        finalSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed;
        // Add force in the forward direction based on moveSpeed
        rb.AddForce(-transform.forward * finalSpeed * 0.5f, ForceMode.Acceleration);
    }

     private void Rotate()
    {
        // Calculate rotation based on player input
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.R))
        {
            rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; // Rotate left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f; // Rotate right
        }

        // If there is rotation input, apply the rotation
        if (horizontalInput != 0)
        {
            if (Input.GetKey(KeyCode.LeftControl)) horizontalInput *= 2;

            // Calculate the target rotation based on input
            Quaternion rotation = Quaternion.Euler(horizontalInput * rotationSpeed * Time.fixedDeltaTime, 0, 0f);

            // Apply the rotation to the Rigidbody
            rb.MoveRotation(rb.rotation * rotation);
        }
    }

    public Rigidbody GetRigidbody() {
        return rb;
    }

    public Transform GetTransform() {
        return transform;
    }
}
