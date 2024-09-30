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

    private Rigidbody rb; // Reference to the Rigidbody component
    [SerializeField] private ParticleSystem ps;

    private bool move = false;

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
        emission.enabled = move;
    }

    void FixedUpdate() {
        rb.mass = dryMass + fuelMass;
        // Handle forward movement (W key)
        if (Input.GetKey(KeyCode.W)) {
            MoveForward(Time.fixedDeltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            MoveBackward(Time.fixedDeltaTime);
        } else {
            move = false;
        }

        Rotate(Time.fixedDeltaTime);
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
        if (fuelMass <= 0)
            return; // Нет топлива для движения

        // Рассчитываем массу корабля с топливом
        float currentMass = dryMass + fuelMass;

        // Сколько топлива можем сжечь за данный промежуток времени
        float fuelToBurn = fuelConsumptionRate * deltaTime;
        if (fuelToBurn > fuelMass)
        {
            fuelToBurn = fuelMass; // Ограничиваем сжигаемое топливо доступным количеством
        }

        // Рассчёт изменения скорости по формуле импульса: Δv = v_exhaust * ln(m0 / m1)
        float finalMass = currentMass - fuelToBurn;
        float deltaV = exhaustVelocity * Mathf.Log(currentMass / finalMass);

        // Применяем силу для ускорения корабля
        Vector3 thrustDirection = transform.forward; // Двигаемся вперёд
        rb.AddForce(thrustDirection * deltaV * rb.mass, ForceMode.Impulse);

        // Уменьшаем массу топлива
        fuelMass -= fuelToBurn;

        move = true;
    }

    private void MoveBackward(float deltaTime)
    {
        if (fuelMass <= 0)
            return; // Нет топлива для движения

        // Рассчитываем массу корабля с топливом
        float currentMass = dryMass + fuelMass;

        // Сколько топлива можем сжечь за данный промежуток времени
        float fuelToBurn = fuelConsumptionRate * deltaTime * 0.5f;
        if (fuelToBurn > fuelMass)
        {
            fuelToBurn = fuelMass; // Ограничиваем сжигаемое топливо доступным количеством
        }

        // Рассчёт изменения скорости по формуле импульса: Δv = v_exhaust * ln(m0 / m1)
        float finalMass = currentMass - fuelToBurn;
        float deltaV = exhaustVelocity  * 0.5f * Mathf.Log(currentMass / finalMass);

        // Применяем силу для ускорения корабля
        Vector3 thrustDirection = transform.forward; // Двигаемся вперёд
        rb.AddForce(thrustDirection * deltaV * rb.mass, ForceMode.Impulse);

        // Уменьшаем массу топлива
        fuelMass -= fuelToBurn;

        move = true;
    }

     private void Rotate(float deltaTime)
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
            if (fuelMass <= 0)
                return; // Нет топлива для поворота

            float fuelToBurn = rotationFuelConsumptionRate * deltaTime;
            if (fuelToBurn > fuelMass)
            {
                fuelToBurn = fuelMass; // Ограничиваем сжигаемое топливо доступным количеством
            }

            float rotationAmount = rotationSpeed * horizontalInput * deltaTime;

            // Calculate the target rotation based on input
            Quaternion rotation = Quaternion.Euler(rotationAmount, 0, 0);

            // Apply the rotation to the Rigidbody
            rb.MoveRotation(rb.rotation * rotation);

            fuelMass -= fuelToBurn;
        }
    }

    public Rigidbody GetRigidbody() {
        return rb;
    }

    public Transform GetTransform() {
        return transform;
    }
}
