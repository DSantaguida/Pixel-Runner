using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{

  public Vector2 pointA;
  public Vector2 pointB;

  private Vector2 target;
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
      target = pointB;
      enemySprite.flipX = true;
    }
    else if (transform.position.x >= pointB.x)
    {
      target = pointA;
      enemySprite.flipX = false;
    }
  }
}
