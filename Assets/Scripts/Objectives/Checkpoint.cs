using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
  [SerializeField]
  private bool isActive = false;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.tag == "Player" && !isActive)
    {
      isActive = true;
      GetComponent<Animator>().SetTrigger("Player");
      GameManager.Instance.updateCheckpoint(transform.position, other.transform.GetComponent<Player>().diamonds);
      GameManager.Instance.ClearDestroyedObjects();
    }
  }
}
