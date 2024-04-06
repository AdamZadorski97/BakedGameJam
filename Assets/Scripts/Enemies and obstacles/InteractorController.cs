using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractorController : MonoBehaviour
{
    public UnityEvent OnInteractEvet;
    public PlayerController playerController;

    public void OnInteract(PlayerController _playerController)
    {
        playerController = _playerController;
        OnInteractEvet.Invoke();
    }






    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().tempInteractorController = this;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other != null && other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().tempInteractorController = null;
        }
    }


}
