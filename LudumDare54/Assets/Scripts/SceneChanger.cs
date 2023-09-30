using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    [SerializeField] List<string> allScenes;
    [SerializeField] List<string> nextScenes;

    int currentLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var item in allScenes)
        {
            nextScenes.Add(item);
        }

        DontDestroyOnLoad(gameObject);

    }

    public void RestartCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }

    public void NextLevel()
    {

            SceneManager.LoadScene(nextScenes[0]);
        nextScenes.RemoveAt(0);


    }

}
