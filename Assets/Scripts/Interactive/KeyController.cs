using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
  public InteractorController InteractorController;

    public void PickupKey()
    {
        InteractorController.playerController.hasKey = true;
        Destroy(this.gameObject);
    }
}
