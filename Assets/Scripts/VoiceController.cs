using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour
{

    public bool isForEveryOne;
    public AudioClip audioOnStart;
    public AudioClip audioOnJasTrigger;
    public AudioClip audioOnMalgosiaTrigger;
    public AudioClip audioOnEveryoneTrigger;

    public bool playOneTime;
    public bool wasPlayed;

    public bool playOnStart;
    public float startDelay;


    private void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(PlayOnStart());
        }
    }
    IEnumerator PlayOnStart()
    {
        yield return new WaitForSeconds(startDelay);
        GetComponent<AudioSource>().PlayOneShot(audioOnStart);
    }

    public void PlayJasiu()
    {
        GetComponent<AudioSource>().PlayOneShot(audioOnJasTrigger);
        wasPlayed = true;
        playOneTime = true;
    }

    public void PlayMalgosia()
    {
        GetComponent<AudioSource>().PlayOneShot(audioOnMalgosiaTrigger);
        wasPlayed = true;
        playOneTime = true;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (playOnStart)
            return;

        if (other != null && other.GetComponent<PlayerController>())
        {
            if (wasPlayed && playOneTime)
                return;
            wasPlayed = true;


            if (isForEveryOne)
            {
                GetComponent<AudioSource>().PlayOneShot(audioOnEveryoneTrigger);
            }
            if (other.GetComponent<PlayerController>().isMan)
            {
                GetComponent<AudioSource>().PlayOneShot(audioOnJasTrigger);
            }
            if (!other.GetComponent<PlayerController>().isMan)
            {
                GetComponent<AudioSource>().PlayOneShot(audioOnMalgosiaTrigger);
            }

        }
    }
}
