using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public List<GameManager.Bottle> Bottles { get; set; }
    public int Cost { get; set; }
    public int Heuristic { get; set; }
    public GameState Parent { get; set; }
    public Tuple<int, int> LastMove { get; set; }

    public int TotalCost => Cost + Heuristic;

    public GameState(List<GameManager.Bottle> bottles)
    {
        Bottles = bottles.Select(b => new GameManager.Bottle { balls = new List<GameManager.Ball>(b.balls) }).ToList();
    }

    public bool IsGoal()
    {
        return Bottles.All(b => b.balls.Count == 0 || (b.balls.Count == 4 && b.balls.All(ball => ball.type == b.balls[0].type)));
    }

    public int CalculateHeuristic()
    {
        int h = 0;
        foreach (var bottle in Bottles)
        {
            if (bottle.balls.Count == 0) continue;
            var groups = bottle.balls.GroupBy(b => b.type).ToList();
            h += groups.Count - 1;
            h += 4 - bottle.balls.Count;
        }
        return h;
    }

    public List<GameState> GetNextStates()
    {
        var nextStates = new List<GameState>();

        for (int i = 0; i < Bottles.Count; i++)
        {
            for (int j = 0; j < Bottles.Count; j++)
            {
                if (i == j) continue;

                var newState = new GameState(Bottles);
                if (newState.TryMove(i, j))
                {
                    newState.Parent = this;
                    newState.LastMove = new Tuple<int, int>(i, j);
                    newState.Cost = Cost + 1;
                    newState.Heuristic = newState.CalculateHeuristic();
                    nextStates.Add(newState);
                }
            }
        }

        return nextStates;
    }

    private bool TryMove(int fromIndex, int toIndex)
    {
        var fromBottle = Bottles[fromIndex];
        var toBottle = Bottles[toIndex];

        if (fromBottle.balls.Count == 0 || toBottle.balls.Count == 4) return false;

        var ballToMove = fromBottle.balls.Last();
        if (toBottle.balls.Count > 0 && toBottle.balls.Last().type != ballToMove.type) return false;

        int count = fromBottle.balls.Count - 1;
        while (count >= 0 && fromBottle.balls[count].type == ballToMove.type && toBottle.balls.Count < 4)
        {
            toBottle.balls.Add(fromBottle.balls[count]);
            fromBottle.balls.RemoveAt(count);
            count--;
        }

        return true;
    }
}
