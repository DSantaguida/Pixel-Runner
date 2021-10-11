using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Player")
    {
      SaveInfo saveInfo = new SaveInfo();
      saveInfo.LoadData();
      GetComponent<Animator>().SetTrigger("Player");
      saveInfo.SaveData();
      GameManager.Instance.EndLevel();
    }
  }
}
