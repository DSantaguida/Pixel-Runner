using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
  public float speed;
  public float time;

  private SpriteRenderer bulletSprite;

  void Start()
  {
    bulletSprite = GetComponent<SpriteRenderer>();
    Destroy(this.gameObject, time);

  }

  void Update()
  {
    Vector2 direction = (transform.parent.GetComponent<SpriteRenderer>().flipX) ? Vector3.right : Vector3.left;
    transform.Translate(direction * speed * Time.deltaTime);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.tag == "Player")
    {
      Player player = other.GetComponent<Player>();

      if (player != null)
      {
        player.Damage();
        Destroy(this.gameObject);
      }
    }
  }
}
