using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
  public int health;
  public float speed;
  protected BoxCollider2D enemyCollider;

  protected Animator enemyAnimator;

  protected SpriteRenderer enemySprite;

  public float knockbackForce;

  private float collisionTime;
  private float threshold = 1f;

  public virtual void Start()
  {
    enemyCollider = GetComponent<BoxCollider2D>();
    enemyAnimator = GetComponent<Animator>();
    enemySprite = GetComponent<SpriteRenderer>();
  }

  public virtual void Update()
  {
    if (Time.timeScale != 0)
      Movement();
  }

  public abstract void Movement();
  public virtual void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player")
    {
      collisionTime = 0;
      Player player = other.gameObject.GetComponent<Player>();
      if (CheckPlayerAbove())
      {
        health--;
        if (health == 0)
          EnemyDead();
        player.AddForce(new Vector2(0, knockbackForce));

      }
      else
        player.Damage();
    }
  }

  public virtual void OnCollisionStay2D(Collision2D other)
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

  protected virtual bool CheckPlayerAbove()
  {
    RaycastHit2D[] hits = new RaycastHit2D[] {
        Physics2D.Raycast(new Vector3(transform.position.x - enemyCollider.bounds.extents.x, transform.position.y), transform.up, 1f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y), transform.up, 1f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x + enemyCollider.bounds.extents.x, transform.position.y), transform.up, 1f, 1 << 7)
    };

    foreach (RaycastHit2D hit in hits)
    {
      if (hit && hit.transform.tag == "Player")
        return true;
    }
    return false;
  }

  public virtual void EnemyDead()
  {
    enemyAnimator.SetTrigger("Hit");
    GameManager.Instance.DestroyObject(this.gameObject);
  }

}
