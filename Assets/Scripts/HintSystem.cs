using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HintSystem
{
    public List<Tuple<int, int>> FindSolution(GameState initialState)
    {
        var openSet = new List<GameState> { initialState };
        var closedSet = new HashSet<string>();

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(s => s.TotalCost).First();
            openSet.Remove(current);

            if (current.IsGoal())
            {
                return ReconstructPath(current);
            }

            closedSet.Add(GetStateString(current));

            foreach (var neighbor in current.GetNextStates())
            {
                var neighborString = GetStateString(neighbor);
                if (closedSet.Contains(neighborString)) continue;

                if (!openSet.Any(s => GetStateString(s) == neighborString))
                {
                    openSet.Add(neighbor);
                }
                else
                {
                    var existingState = openSet.First(s => GetStateString(s) == neighborString);
                    if (neighbor.Cost < existingState.Cost)
                    {
                        openSet.Remove(existingState);
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No solution found
    }

    private string GetStateString(GameState state)
    {
        return string.Join("|", state.Bottles.Select(b => string.Join(",", b.balls.Select(ball => (int)ball.type))));
    }

    private List<Tuple<int, int>> ReconstructPath(GameState state)
    {
        var path = new List<Tuple<int, int>>();
        while (state.Parent != null)
        {
            path.Add(state.LastMove);
            state = state.Parent;
        }
        path.Reverse();
        return path;
    }
}

