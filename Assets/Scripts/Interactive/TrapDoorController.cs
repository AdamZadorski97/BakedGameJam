using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorController : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController>();




    private void OnTriggerEnter(Collider other)
    {
        PlayerController enteringPlayer = other.GetComponent<PlayerController>();
        if (enteringPlayer != null && !playerControllers.Contains(enteringPlayer))
        {
            playerControllers.Add(enteringPlayer);
            CheckPlayers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController exitingPlayer = other.GetComponent<PlayerController>();
        if (exitingPlayer != null)
        {
            playerControllers.Remove(exitingPlayer);
            if (playerControllers.Count == 0)
            {
               
            }
        }
    }

   public void CheckPlayers()
    {
        if(playerControllers.Count >1)
        {
            InputController.Instance.Vibrate(1f, InputController.Instance.Player1Actions, 2f);
            InputController.Instance.Vibrate(1f, InputController.Instance.Player2Actions, 2f);
            Destroy(transform.parent.GetComponent<BoxCollider>());
            Destroy(transform.parent.GetComponent<MeshRenderer>());
            GetComponent<AudioSource>().Play();
        }
    }
}
