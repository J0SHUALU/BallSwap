using System.Collections.Generic;
using UnityEngine;

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
        history.Clear();
        MoveCount = 0;
        selected = null;

        var layout = builder.Build(level, capacity, out int tubeCount);
        float spacing = 2.2f;
        float startX = -(tubeCount - 1) * spacing / 2f;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) HandleClick();
    }

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

    public void Undo()
    {
        if (history.UndoLast()) MoveCount++;
    }

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