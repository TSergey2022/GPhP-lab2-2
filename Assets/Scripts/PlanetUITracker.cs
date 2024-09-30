using UnityEngine;
using UnityEngine.UI;

public class PlanetUITracker : MonoBehaviour
{
    public GameObject circlePrefab; // Prefab for the UI circle (should be an Image component)
    public RectTransform canvasRectTransform; // Reference to the Canvas's RectTransform
    public Transform player; // Reference to the player's transform (like the spaceship)

    private PlanetScript[] planets; // Array to hold all the planets with PlanetScript
    private GameObject[] planetCircles; // Array to hold references to the circles for each planet

    void Start()
    {
        // Find all GameObjects with the PlanetScript component
        planets = FindObjectsOfType<PlanetScript>();

        // Create a corresponding UI circle for each planet
        planetCircles = new GameObject[planets.Length];

        for (int i = 0; i < planets.Length; i++)
        {
            // Instantiate a new circle on the canvas
            GameObject circle = Instantiate(circlePrefab, canvasRectTransform);
            planetCircles[i] = circle;
        }
    }

    void Update()
    {
        // Update each circle's position and direction
        for (int i = 0; i < planets.Length; i++)
        {
            UpdateCirclePosition(i);
        }
    }

    void UpdateCirclePosition(int index)
    {
        // Get the planet and corresponding UI circle
        PlanetScript planet = planets[index];
        GameObject circle = planetCircles[index];

        // Direction from the player to the planet
        Vector3 directionToPlanet = planet.transform.position - player.position;

        // Convert the 3D world position of the planet to a 2D screen point
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(planet.transform.position);

        // Check if the planet is behind the camera
        if (screenPosition.z > 0)
        {
            // If the planet is in front of the camera, position the circle on the screen
            circle.transform.position = screenPosition;
        }
        else
        {
            // If the planet is behind the player, place the circle at the edge of the screen
            screenPosition.x = screenPosition.x < Screen.width / 2 ? 0 : Screen.width;
            screenPosition.y = screenPosition.y < Screen.height / 2 ? 0 : Screen.height;
            circle.transform.position = screenPosition;
        }

        // Rotate the circle to point towards the planet
        Vector2 direction = new Vector2(directionToPlanet.x, directionToPlanet.z).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        circle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f)); // Adjusting by -90 to make sure the top of the circle points toward the planet
    }
}
