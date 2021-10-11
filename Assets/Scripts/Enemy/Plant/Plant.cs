using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{

  public float attackDelay;

  public PlantBullet plantBullet;

  private float xPos;
  public override void Start()
  {

    base.Start();
    xPos = (enemySprite.flipX) ? transform.position.x + enemyCollider.bounds.extents.x : transform.position.x - enemyCollider.bounds.extents.x;

    StartCoroutine(Attack());
  }

  public override void Movement() { }

  private IEnumerator Attack()
  {
    while (true)
    {
      yield return new WaitForSeconds(attackDelay);
      enemyAnimator.SetTrigger("Attack");
      yield return new WaitForSeconds(0.25f);
      PlantBullet spawn = Instantiate(plantBullet, new Vector2(xPos, transform.position.y + 0.1f), Quaternion.identity);
      spawn.transform.parent = this.gameObject.transform;
    }
  }
}
