using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

  private float collisionTime;
  private float threshold = 1f;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.tag == "Player")
    {
      collisionTime = 0;
      Player player = other.transform.GetComponent<Player>();

      if (player != null)
        player.Damage();
    }
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    if (other.transform.tag == "Player")
    {
      if (collisionTime < threshold)
        collisionTime += Time.deltaTime;
      else
      {
        Player player = other.gameObject.GetComponent<Player>();
        player.Damage();
        collisionTime = 0;
      }
    }
  }
}
