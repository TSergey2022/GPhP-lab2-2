using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeCum : MonoBehaviour {
  public Transform Target;
  public Vector3 Offset = Vector3.zero;

  public float minZoom = 5f;    // Minimum zoom level
  public float maxZoom = 300f;   // Maximum zoom level

  private Camera cam;           // Reference to the Camera component

  void Start() {
      cam = GetComponent<Camera>();
  }

  void Update() {
    HandleTarget();
    HandleZoom();
  }

  void HandleTarget() {
    if (Target == null) return;
    transform.position = Target.position + Offset;
  }

  void HandleZoom()
  {
    // Get mouse scroll wheel input
    float scrollInput = Input.GetAxis("Mouse ScrollWheel");

    // Check if there is any input (scrollInput != 0)
    if (scrollInput != 0)
    {
      // Adjust the camera's field of view (FOV) for zooming
      Offset.y -= scrollInput * Offset.y * 0.2f;

      // Clamp the FOV value to stay within the minZoom and maxZoom range
      Offset.y = Mathf.Clamp(Offset.y, minZoom, maxZoom);
    }
  }
}
