using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : Trap
{

  public List<Vector2> path;

  public float moveSpeed;

  private int nextPos = 0;

  private Rigidbody2D spikeRigidBody;
  void Start()
  {
    spikeRigidBody = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    if (path.Count > 0)
      Movement();
  }

  protected virtual void Movement()
  {
    if ((Vector2)transform.position == path[nextPos])
    {
      nextPos++;
      if (nextPos == path.Count)
        nextPos = 0;
    }
    else
      transform.position = Vector2.MoveTowards(transform.position, path[nextPos], moveSpeed);

  }
}
