using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
  public float knockbackForce;
  private float collisionTime;
  private float threshold = 1f;
  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player")
    {
      collisionTime = 0;
      Player player = other.transform.GetComponent<Player>();

      if (player != null)
      {
        player.Damage();
        player.AddForce(new Vector2(0, knockbackForce));
      }
    }
  }

  public void OnCollisionStay2D(Collision2D other)
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
