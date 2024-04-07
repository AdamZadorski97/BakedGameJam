using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
  public InteractorController InteractorController;
    public VoiceController VoiceController;
    public void PickupKey()
    {
        InteractorController.playerController.hasKey = true;
        GetComponent<AudioSource>().Play();
        if (InteractorController.playerController.isMan)
        {
            VoiceController.PlayJasiu();
        }
        else
        {
            VoiceController.PlayMalgosia();
        }
        Destroy(this.gameObject, 1f);
    }
}
