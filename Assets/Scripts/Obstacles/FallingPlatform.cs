using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

  public float dropDelay;
  public float fallSpeed;
  private Animator platformAnimator;
  private Rigidbody2D platformBody;

  // Start is called before the first frame update
  void Start()
  {
    platformAnimator = GetComponent<Animator>();
    platformBody = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player")
    {
      StartCoroutine(DropPlatform());
    }
    else if (other.transform.tag == "Ground")
      Destroy(this.gameObject, 1f);
  }

  private IEnumerator DropPlatform()
  {
    yield return new WaitForSeconds(dropDelay);
    platformAnimator.SetTrigger("Fall");
    platformBody.velocity = new Vector2(0, -fallSpeed);
  }
}
