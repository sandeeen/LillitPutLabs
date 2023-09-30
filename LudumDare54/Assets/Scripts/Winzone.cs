using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winzone : MonoBehaviour
{
    bool hasActivated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasActivated)
            {
                hasActivated = true;
                SceneChanger.Instance.NextLevel();

            }
        }
    }
}
