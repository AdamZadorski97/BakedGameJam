using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private Animator animatorController;



    [Button]
    public void Run()
    {
        animatorController.SetTrigger("Run");
    }
    [Button]
    public void Idle()
    {
        animatorController.SetTrigger("Idle");
    }
}
