using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
  public int value = 1;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.tag == "Player")
    {
      Player player = other.gameObject.GetComponent<Player>();

      if (player != null)
        player.AddDiamonds(value);

      GetComponent<Animator>().SetTrigger("isCollected");
      GameManager.Instance.DestroyObject(this.gameObject);
    }
  }



}
