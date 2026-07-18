using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public AudioSource menuMusic;

    public void OnPlay()
    {
        StartCoroutine(LoadGameNextFrame());
    }

    private IEnumerator LoadGameNextFrame()
    {
        yield return null;
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuit()
    {
        Debug.Log("Quit pressed - exiting BallSwap");
        if (menuMusic) menuMusic.Stop();
        Application.Quit();
    }
}
