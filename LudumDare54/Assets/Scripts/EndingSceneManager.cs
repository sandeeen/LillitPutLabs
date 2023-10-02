using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;
    [SerializeField] AudioClip endingSound;
    [SerializeField] AudioClip endingMusic;
    void Start()
    {
        //SoundManager.Instance.RemoveLoopingSound()
        SoundManager.Instance.PlayAudio(endingSound, 1f);
        StartCoroutine(PlayEndingMusic());
    }

    private IEnumerator PlayEndingMusic()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayGlobalLoopingSound(endingMusic, 1f);
    }
}
