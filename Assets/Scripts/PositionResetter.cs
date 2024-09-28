using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PositionResetter : MonoBehaviour
{
    // This is where we will store the GameObject's current position
    public Vector3 storedPosition;

    // Function to store current position and reset to (0,0,0)
    public void StoreAndResetPosition()
    {
        // Store the current position
        storedPosition = transform.position;

        // Reset the GameObject's position to (0, 0, 0)
        transform.position = Vector3.zero;
    }
}

[CustomEditor(typeof(PositionResetter))]
public class PositionResetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector for other components/variables
        DrawDefaultInspector();

        // Get the script we are working with
        PositionResetter script = (PositionResetter)target;

        // Create a button in the inspector
        if (GUILayout.Button("Store Position and Reset"))
        {
            // Call the function to store and reset the position
            script.StoreAndResetPosition();
        }
    }
}
