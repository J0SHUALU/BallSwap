using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Runs the main menu scene: play, quit, and menu music.
public class MenuController : MonoBehaviour
{
    public AudioSource menuMusic;

    public void OnPlay()
    {
        StartCoroutine(LoadGameNextFrame());
    }

    // Wait one frame so the button click finishes before the scene loads.
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
