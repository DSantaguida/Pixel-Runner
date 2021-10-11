using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
  int Lives { get; set; }
  void Damage();
}
