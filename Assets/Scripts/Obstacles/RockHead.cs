using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : Trap
{

  public float fallSpeed;
  private BoxCollider2D rockCollider;

  private Rigidbody2D rockRigidBody;

  private Animator rockAnimator;

  private float groundDistance;

  private Vector2 origin;

  private bool isActive;

  private RigidbodyConstraints2D activeConstraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

  private RigidbodyConstraints2D InactiveConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

  void Start()
  {
    isActive = true;
    rockCollider = GetComponent<BoxCollider2D>();
    rockRigidBody = GetComponent<Rigidbody2D>();
    rockAnimator = GetComponent<Animator>();
    origin = transform.position;
    groundDistance = DistanceFromGround();
  }

  // Update is called once per frame
  void Update()
  {
    CheckOriginPosition();
    Player player = null;

    if (isActive)
    {
      player = DetectPlayer();
      rockRigidBody.constraints = activeConstraints;
    }


    if (player != null)
      CrushPlayer();


  }

  private Player DetectPlayer()
  {
    RaycastHit2D[] hits = new RaycastHit2D[] {
        Physics2D.Raycast(new Vector3(transform.position.x - rockCollider.bounds.extents.x + 0.1f, transform.position.y), -transform.up, groundDistance, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x - rockCollider.bounds.extents.x + 0.1f, transform.position.y), -transform.up, groundDistance, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x + rockCollider.bounds.extents.x - 0.1f, transform.position.y), -transform.up, groundDistance, 1 << 7)
    };

    foreach (RaycastHit2D hit in hits)
    {
      if (hit && hit.collider.GetComponent<Player>())
        return hit.collider.GetComponent<Player>();

    }

    return null;
  }

  private float DistanceFromGround()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, 1 << 6);
    return hit.distance;
  }

  private void CrushPlayer()
  {
    rockRigidBody.constraints = InactiveConstraints;
    rockRigidBody.velocity = new Vector2(0, -fallSpeed);
    isActive = false;
  }

  private void ReturnToOrigin()
  {
    rockRigidBody.velocity = new Vector2(0, fallSpeed);
  }

  private void CheckOriginPosition()
  {
    if (rockRigidBody.velocity.y > 0 && transform.position.y >= origin.y)
    {
      rockRigidBody.velocity = new Vector2(0, 0);
      StartCoroutine(DelayCrush());
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    rockAnimator.SetTrigger("Crush");

    if (other.transform.tag == "Player" && DetectPlayer() != null)
    {
      Player player = other.transform.GetComponent<Player>();

      if (player != null)
        player.Damage();
    }
    StartCoroutine(WaitThenReturn((other.transform.tag == "Player") ? 0.2f : 0.5f));
  }

  private IEnumerator WaitThenReturn(float time)
  {
    yield return new WaitForSeconds(time);
    ReturnToOrigin();
  }

  private IEnumerator DelayCrush()
  {
    yield return new WaitForSeconds(1f);
    isActive = true;
  }
}
