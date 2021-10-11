using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

  public float power;
  private BoxCollider2D trampolineCollider;
  private Animator trampolineAnimator;

  void Start()
  {
    trampolineCollider = GetComponent<BoxCollider2D>();
    trampolineAnimator = GetComponent<Animator>();
  }

  void Update()
  {
    // Debug.DrawRay(new Vector3(transform.position.x - trampolineCollider.bounds.extents.x, transform.position.y), transform.up, Color.red);
    // Debug.DrawRay(new Vector3(transform.position.x, transform.position.y), transform.up, Color.red);
    // Debug.DrawRay(new Vector3(transform.position.x + trampolineCollider.bounds.extents.x, transform.position.y), transform.up, Color.red);
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player" && DetectPlayer()) //Need to check for above using raycast
    {
      Player player = other.transform.GetComponent<Player>();

      if (player != null)
      {
        trampolineAnimator.SetTrigger("Bounce");
        player.AddForce(new Vector2(0, power));
      }
    }
  }

  private bool DetectPlayer()
  {
    RaycastHit2D[] hits = new RaycastHit2D[] {
        Physics2D.Raycast(new Vector3(transform.position.x - trampolineCollider.bounds.extents.x, transform.position.y), transform.up, 0.25f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y), transform.up, 0.25f, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x + trampolineCollider.bounds.extents.x, transform.position.y), transform.up, 0.25f, 1 << 7)
    };

    foreach (RaycastHit2D hit in hits)
    {
      if (hit && hit.transform.tag == "Player")
        return true;
    }
    return false;
  }
}
