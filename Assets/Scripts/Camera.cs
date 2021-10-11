using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

  public Vector3 offset;

  public bool followY;

  private Transform player;

  void Start()
  {
    player = GameObject.FindWithTag("Player").transform;
  }

  void LateUpdate()
  {
    try
    {
      transform.position = new Vector3(player.position.x + offset.x, (followY) ? player.position.y + offset.y : offset.y, offset.z);
    }
    catch (MissingReferenceException)
    {
      // player = GameObject.FindWithTag("Player").transform;
      StartCoroutine(WaitForSpawn());
    }
  }

  private IEnumerator WaitForSpawn()
  {
    yield return new WaitForSeconds(0.1f);
    player = GameObject.FindWithTag("Player").transform;
  }
}
