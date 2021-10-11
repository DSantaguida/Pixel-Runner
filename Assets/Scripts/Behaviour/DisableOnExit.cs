using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnExit : StateMachineBehaviour
{
  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (animator.transform.parent != null)
      animator.transform.parent.gameObject.SetActive(false);
    else
      animator.transform.gameObject.SetActive(false);
  }

}
