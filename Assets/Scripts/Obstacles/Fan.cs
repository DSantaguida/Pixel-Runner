using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{

  public float fanStrength;
  public float fanDistance;
  private Vector2 direction;
  private BoxCollider2D fanCollider;

  void Start()
  {
    fanCollider = GetComponent<BoxCollider2D>();
  }

  // Update is called once per frame
  void Update()
  {
    Player player = detectPlayer();

    if (player != null)
      BlowPlayer(player);
  }

  private Player detectPlayer()
  {

    RaycastHit2D[] hits = new RaycastHit2D[] {
        Physics2D.Raycast(new Vector3(transform.position.x - fanCollider.bounds.extents.x, transform.position.y), transform.up, fanDistance, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y), transform.up, fanDistance, 1 << 7),
        Physics2D.Raycast(new Vector3(transform.position.x + fanCollider.bounds.extents.x, transform.position.y), transform.up, fanDistance, 1 << 7)
    };

    // Debug.DrawRay(new Vector3(transform.position.x + fanCollider.bounds.extents.x, transform.position.y), transform.up * fanDistance, Color.red);
    // Debug.DrawRay(new Vector3(transform.position.x, transform.position.y), transform.up * fanDistance, Color.red);
    // Debug.DrawRay(new Vector3(transform.position.x - fanCollider.bounds.extents.x, transform.position.y), transform.up * fanDistance, Color.red);

    foreach (RaycastHit2D hit in hits)
    {
      if (hit && hit.collider.GetComponent<Player>())
        return hit.collider.GetComponent<Player>();
    }

    return null;
  }

  private void BlowPlayer(Player player)
  {
    float xForce = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * -fanStrength;
    float yForce = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * fanStrength;

    player.AddForce(new Vector2(xForce, yForce));
  }

}
