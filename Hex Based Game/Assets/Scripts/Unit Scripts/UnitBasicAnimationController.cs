using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBasicAnimationController : MonoBehaviour
{
    
    public Animator animator;
    
    public void StartRunning() {
        animator.SetBool("isRunning", true);
    }

    public void StopRunning() {
        animator.SetBool("isRunning", false);
    }

}