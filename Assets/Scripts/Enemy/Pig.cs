using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Enemy
{
  [SerializeField]
  private float aggroRange;

  public Vector2 pointA;
  public Vector2 pointB;

  private Vector2 target;
  public override void Start()
  {
    base.Start();
    target = pointA;
  }

  public override void Movement()
  {
    Player player = CheckForPlayer();

    if (player != null)
      RunAtPlayer(player);
    else
      Patrol();
  }

  private Player CheckForPlayer() //Raycast to detect player on x axis
  {
    Vector2 direction = (enemySprite.flipX) ? transform.right : -transform.right;
    RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y), direction, aggroRange, 1 << 7);

    if (hit && hit.transform.tag == "Player")
      return hit.transform.GetComponent<Player>();

    return null;
  }

  private void Patrol() //Walking between patrol points at half speed
  {
    enemyAnimator.SetBool("isChasing", false);
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

  private void RunAtPlayer(Player player) //Player is detected and enemy runs towards player
  {
    enemyAnimator.SetBool("isChasing", true);
    if (CheckBounds())
    {
      transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed);

      if (player.transform.position.x > transform.position.x)
        enemySprite.flipX = true;
      else if (player.transform.position.x < transform.position.x)
        enemySprite.flipX = false;
    }
  }

  private bool CheckBounds() //Check if transform exceeds pointA/pointB
  {
    return transform.position.x >= pointA.x && transform.position.x <= pointB.x;
  }

  protected override bool CheckPlayerAbove()
  {
    RaycastHit2D[] hits = new RaycastHit2D[] {
        Physics2D.Raycast(new Vector3(transform.position.x - enemyCollider.bounds.extents.x + 0.1f, transform.position.y), transform.up, 1f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y), transform.up, 1f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x + enemyCollider.bounds.extents.x - 0.1f, transform.position.y), transform.up, 1f, 1 << 7)
    };

    foreach (RaycastHit2D hit in hits)
    {
      if (hit && hit.transform.tag == "Player")
        return true;
    }
    return false;
  }

}
