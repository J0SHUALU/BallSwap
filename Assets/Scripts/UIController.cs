using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Singleton pattern: one UI manager for the counters, win panel, and buttons.
public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    public TMP_Text movesText;
    public TMP_Text levelText;
    public TMP_Text winMovesText;
    public GameObject winPanel;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (movesText) movesText.text = "Moves " + GameManager.Instance.MoveCount;
        if (levelText) levelText.text = "Level " + GameManager.Instance.level;
    }

    // Button handlers pass the work to the GameManager.
    public void OnUndo() => GameManager.Instance.Undo();
    public void OnHint() => GameManager.Instance.Hint();
    public void OnRestart() => GameManager.Instance.BuildLevel();
    public void OnMenu() => SceneManager.LoadScene("MenuScene");

    public void ShowWin()
    {
        if (winMovesText)
            winMovesText.text = "completed in " + GameManager.Instance.MoveCount + " moves";
        if (winPanel) winPanel.SetActive(true);
    }

    public void OnNext()
    {
        if (winPanel) winPanel.SetActive(false);
        GameManager.Instance.NextLevel();
    }
}
