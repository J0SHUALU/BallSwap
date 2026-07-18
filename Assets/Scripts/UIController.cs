using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    public TMP_Text movesText;
    public GameObject winPanel;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    private void Update()
    {
        if (movesText && GameManager.Instance)
            movesText.text = "Moves: " + GameManager.Instance.MoveCount;
    }

    public void OnUndo() => GameManager.Instance.Undo();
    public void OnHint() => GameManager.Instance.Hint();
    public void OnRestart() => GameManager.Instance.BuildLevel();
    public void ShowWin() { if (winPanel) winPanel.SetActive(true); }
    public void OnNext()
    {
        if (winPanel) winPanel.SetActive(false);
        GameManager.Instance.NextLevel();
    }
}