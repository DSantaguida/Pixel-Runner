using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
  public Vector2 pointA;
  public Vector2 pointB;

  private Vector2 target;

  private bool spikes;
  public override void Start()
  {
    base.Start();
    target = pointA;
  }

  public override void Movement() //Patrol animation
  {
    transform.position = Vector2.MoveTowards(transform.position, target, speed / 2);

    if (transform.position.x <= pointA.x)
    {
      spikes = true;
      enemyAnimator.SetTrigger("Spikes");
      target = pointB;
      enemySprite.flipX = true;
    }
    else if (transform.position.x >= pointB.x)
    {
      spikes = false;
      enemyAnimator.SetTrigger("Spikes");
      target = pointA;
      enemySprite.flipX = false;
    }
  }

  public override void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player")
    {
      Player player = other.gameObject.GetComponent<Player>();
      if (spikes)
        player.Damage();

      player.AddForce(new Vector2(0, knockbackForce));

    }
  }
}
