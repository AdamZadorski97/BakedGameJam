using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
   public PlayerController controller;

    public void Walk()
    {
        controller.WalkSound();
    }
}
