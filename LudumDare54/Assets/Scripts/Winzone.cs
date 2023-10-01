using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winzone : MonoBehaviour
{
    bool hasActivated = false;
    [SerializeField] AudioClip audioClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            if (!hasActivated)
            {
                hasActivated = true;
                SoundManager.Instance.PlayAudio(audioClip, 1f);
                SceneChanger.Instance.NextLevel();

            }
        }
    }
}
