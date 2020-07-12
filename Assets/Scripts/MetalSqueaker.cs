using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSqueaker : MonoBehaviour
{
    public float minTime, maxTime;
    public float sphereRadius;

    AudioSource source;
    public AudioClip[] clips;

    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine("Squeak");
    }

    IEnumerator Squeak()
    {
        while(true)
        {
            float timeToWait = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(timeToWait);
            source.clip = clips[Random.Range(0, clips.Length)];
            transform.position = Random.insideUnitSphere * sphereRadius;
            source.Play();
        }
    }
}
