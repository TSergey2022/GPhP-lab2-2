using System.Collections.Generic;
using UnityEngine;

public interface IGravity {
  static public List<IGravity> grav = new();

  Rigidbody GetRigidbody();
  Transform GetTransform();
}