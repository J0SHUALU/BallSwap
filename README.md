# BallSwap

A 3D ball-sorting puzzle made in Unity. Sort the balls so each tube holds one colour. Has undo, a hint that finds the next move, and levels that get harder.

[Watch the walkthrough](media/Movie_001.mp4)

## How to play

- Click a tube to pick it up (it lifts).
- Click a second tube to move the top ball across.
- A ball can only move onto an empty tube or onto a ball of the same colour.
- You win when every tube is empty or full of one colour.
- Each level adds another colour.

## Controls

- **Menu** - go back to the main menu.
- **Undo** - take back your last move.
- **Hint** - lifts the tube you should move from next.
- **Restart** - rebuild the current level.
- **Next** - go to the next level (on the win screen).

## Setup

1. Clone the repo:
   ```
   git clone https://github.com/J0SHUALU/BallSwap.git
   ```
2. Open it in Unity 6 (6000.x) with Unity Hub.
3. Open `MenuScene` in `Assets/Scenes`.
4. Press Play.

## How the code is organised

- `GameManager` - runs the game: builds levels, handles clicks, tracks moves, checks for a win, runs hints. (Singleton)
- `UIController` - counters, win panel, scene switching. (Singleton)
- `MenuController` - the menu scene: play, quit, music.
- `Tube` - holds a private stack of balls and the pour rules. Uses a `TubeState` enum. (State pattern)
- `Ball` - holds its colour and moves itself.
- `PourCommand` and `MoveHistory` - each move can undo itself; the history powers undo. (Command pattern)
- `Solver` and `BoardState` - breadth-first search that finds the next move for the hint.
- `LevelBuilder` - builds a solvable board by scrambling a solved one (recursion).
- `ISelectable`, `IMove` - interfaces.

### Patterns

- Singleton - `GameManager`, `UIController`
- State - `Tube` with `TubeState`
- Command - `PourCommand` and `MoveHistory` (undo)

### Algorithms

- Breadth-first search - `Solver` finds the shortest path to a solved board and returns its first move.
- Recursion - `LevelBuilder` scrambles a solved board with random legal moves.

## Asset sources

- Music (menu and game): Pixabay (pixabay.com), Pixabay Content License, free to use, no credit needed.
- Font: 1001fonts.com, under the licence on the font's page.
- 3D models and materials: made from Unity's built-in sphere and cylinder shapes with my own colours, no downloaded 3D assets.

**Author:** Joshua Moses 