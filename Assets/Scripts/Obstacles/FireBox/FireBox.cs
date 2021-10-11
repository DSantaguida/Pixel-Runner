using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBox : MonoBehaviour
{
  public GameObject fire;
  public float delay;
  public bool active;
  private Animator fireBoxAnimator;

  void Start()
  {
    fireBoxAnimator = GetComponent<Animator>();
    StartCoroutine(Cycle());
  }

  private IEnumerator Cycle()
  {
    while (active)
    {
      FlipState();
      yield return new WaitForSeconds(delay);
    }
  }

  private void FlipState()
  {
    fire.SetActive(!fireBoxAnimator.GetBool("isOn"));
    fireBoxAnimator.SetBool("isOn", !fireBoxAnimator.GetBool("isOn"));
  }
}
