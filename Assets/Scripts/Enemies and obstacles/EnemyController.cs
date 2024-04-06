using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerSpawner.Instance.ResetPlayerPosition(collision.gameObject.GetComponent<PlayerController>());
        }
    }

}
