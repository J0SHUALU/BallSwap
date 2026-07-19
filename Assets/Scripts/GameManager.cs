using System.Collections.Generic;
using UnityEngine;

// Singleton pattern: one central manager that runs the game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Tube tubePrefab;
    public Ball ballPrefab;
    public Material[] ballMaterials;
    public int level = 1;
    public int capacity = 4;

    private readonly List<Tube> tubes = new List<Tube>();
    private readonly MoveHistory history = new MoveHistory();
    private readonly LevelBuilder builder = new LevelBuilder();
    private Tube selected;
    public int MoveCount { get; private set; }

    // Singleton: make sure only one instance exists.
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start() => BuildLevel();

    public void BuildLevel()
    {
        foreach (Tube t in tubes) if (t) Destroy(t.gameObject);
        tubes.Clear();

        foreach (Ball stray in FindObjectsByType<Ball>(FindObjectsInactive.Exclude))
            Destroy(stray.gameObject);

        history.Clear();
        MoveCount = 0;
        selected = null;

        // Ask the builder for a scrambled, solvable board.
        var layout = builder.Build(level, capacity, out int tubeCount);
        float spacing = 2.2f;
        float startX = -(tubeCount - 1) * spacing / 2f;

        FrameCamera(tubeCount, spacing);

        // Spawn the tubes and fill them with balls.
        for (int i = 0; i < tubeCount; i++)
        {
            Tube tube = Instantiate(tubePrefab);
            tube.transform.position = new Vector3(startX + i * spacing, 0f, 0f);
            tube.Capacity = capacity;
            foreach (BallColor c in layout[i])
            {
                Ball ball = Instantiate(ballPrefab);
                ball.SetColor(c, ballMaterials[(int)c]);
                tube.Push(ball);
            }
            tubes.Add(tube);
        }
    }

    // Fit the camera to however many tubes there are this level.
    private void FrameCamera(int tubeCount, float spacing)
    {
        float totalWidth = (tubeCount - 1) * spacing + 4f;
        Camera cam = Camera.main;
        float halfWidth = totalWidth / 2f;
        float distance = halfWidth / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) / cam.aspect;
        cam.transform.position = new Vector3(0f, 0f, -Mathf.Max(distance, 8f));
        cam.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) HandleClick();
    }

    // Click a tube to select it, click another to pour.
    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        Tube clicked = hit.collider.GetComponentInParent<Tube>();
        if (clicked == null) return;

        if (selected == null)
        {
            if (clicked.IsEmpty) return;
            selected = clicked;
            selected.OnSelected();
        }
        else if (selected == clicked)
        {
            selected.OnDeselected();
            selected = null;
        }
        else
        {
            // Command pattern: wrap the pour as an object and record it for undo.
            PourCommand move = new PourCommand(selected, clicked);
            if (move.IsValid())
            {
                move.Execute();
                history.Record(move);
                MoveCount++;
            }
            selected.OnDeselected();
            selected = null;
            CheckWin();
        }
    }

    // Undo: let the history reverse the last move.
    public void Undo()
    {
        if (history.UndoLast()) MoveCount++;
    }

    // Hint: snapshot the board and ask the solver for the next move.
    public void Hint()
    {
        var snapshot = new List<List<BallColor>>();
        foreach (Tube t in tubes) snapshot.Add(t.Snapshot());
        var state = new BoardState(snapshot, capacity);

        var solver = new Solver();
        if (solver.TryFindNextMove(state, out Solver.Move m))
        {
            tubes[m.From].OnSelected();
            Invoke(nameof(ClearHint), 0.8f);
        }
    }

    private void ClearHint()
    {
        foreach (Tube t in tubes)
            if (t.State == TubeState.Selected) t.OnDeselected();
    }

    // Win when every tube is sorted.
    private void CheckWin()
    {
        foreach (Tube t in tubes)
            if (!t.IsSortedComplete()) return;
        UIController.Instance?.ShowWin();
    }

    public void NextLevel()
    {
        level++;
        BuildLevel();
    }
}
