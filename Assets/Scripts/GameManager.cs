using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameGraphic gGraphic;
    public GameLevelManager gameLevelManager;

    public List<Bottle> bottles;

    private Stack<List<Bottle>> historyStack = new Stack<List<Bottle>>();


    private HintSystem hintSystem;

    private void Awake()
    {
        hintSystem = new HintSystem();
    }


    public bool ExecuteHint()
    {
        if (bottles == null || bottles.Count == 0)
        {
            Debug.LogWarning("No bottles available for hint.");
            return false;
        }

        var initialState = new GameState(bottles);
        var solution = hintSystem.FindSolution(initialState);

        if (solution != null && solution.Count > 0)
        {
            var move = solution[0];
            SwitchBall(move.Item1, move.Item2);
            return true;
        }

        Debug.Log("No valid moves available or puzzle already solved.");
        return false;
    }

    public void LoadLevel(List<int[]> listArray)
    {
        bottles = new List<Bottle>();

        foreach (int[] arr in listArray)
        {
            Bottle b = new Bottle();
            for (int i = 0; i < arr.Length; i++)
            {
                int element = arr[i];
                if (element == 0)
                    continue;

                b.balls.Add(new Ball { type = (BallType)element -1});
            }
            bottles.Add(b);
        }
        gGraphic.createBottleGraphic(bottles);
    }

    public void DestroyCurrentLevel()
    {
        // Clear the bottles list
        bottles.Clear();

        // Clear the history stack
        historyStack.Clear();

        // Destroy all bottle graphics
        gGraphic.DestroyAllBottleGraphics();

        gGraphic.selectedBottleIndex = -1;
    }

    public void AddNewBottle()
    {
        SaveState();
        Bottle newBottle = new Bottle();
        bottles.Add(newBottle);
        gGraphic.AddNewBottleGraphic(newBottle);
    }

    public void SwitchBall(Bottle bottle1, Bottle bottle2)
    {
        List<Ball> bottle1Balls = bottle1.balls;
        List<Ball> bottle2Balls = bottle2.balls;

        if (bottle1Balls.Count == 0 || bottle2Balls.Count == 4)
        {
            return;
        }

        int index = bottle1Balls.Count - 1;
        Ball b = bottle1Balls[index];

        var type = b.type;

        if (bottle2Balls.Count > 0 && bottle2Balls[bottle2Balls.Count - 1].type != type)
        {
            return;
        }

        for (int i = index; i >= 0; i--)
        {
            Ball ball = bottle1Balls[i];
            if (ball.type == type)
            {

                if(bottle2Balls.Count ==4)
                {
                    break;
                }
                bottle1Balls.RemoveAt(i);
                bottle2Balls.Add(ball);
            }
            else
            {
                break;
            }
        }
        bool isWin = CheckWinCondition();

        Debug.Log("Is win" + isWin);
        if (isWin)
        {
            gameLevelManager.OnLevelComplete();
        }
    }

    public void SaveState()
    {
        List<Bottle> stateCopy = new List<Bottle>();
        foreach (Bottle bottle in bottles)
        {
            Bottle copyBottle = new Bottle();
            foreach (Ball ball in bottle.balls)
            {
                copyBottle.balls.Add(new Ball { type = ball.type });
            }
            stateCopy.Add(copyBottle);
        }
        historyStack.Push(stateCopy);
    }

    public void Undo()
    {
        if (historyStack.Count == 0) return;

        bottles = historyStack.Pop();
        gGraphic.RefreshBottleGraphic(bottles);
    }


    public void SwitchBall(int bottleIndex1, int bottleIndex2) 
    {
        SaveState();

        Bottle b1 = bottles[bottleIndex1];
        Bottle b2 = bottles[bottleIndex2];

        SwitchBall(b1, b2);

        gGraphic.RefreshBottleGraphic(bottles);
    }



    public List<SwitchBallCommand> CheckSwitchBall(int  bottleIndex1, int bottleIndex2)
    {
        List<SwitchBallCommand > commands = new List<SwitchBallCommand>();

        Bottle bottle1 = bottles[bottleIndex1];
        Bottle bottle2 = bottles[bottleIndex2];

        List<Ball> bottle1Balls = bottle1.balls;
        List<Ball> bottle2Balls = bottle2.balls;

        if (bottle1Balls.Count == 0 || bottle2Balls.Count == 4)
            return commands;

        int index = bottle1Balls.Count - 1;
        Ball b = bottle1Balls[index];

        var type = b.type;

        if (bottle2Balls.Count > 0 && bottle2Balls[bottle2Balls.Count - 1].type != type)
        {
            return commands;
        }
        int targetIndex = bottle2Balls.Count;

        for (int i = index; i >= 0; i--)
        {

            Ball ball = bottle1Balls[i];
            if (ball.type == type)
            {
                if (targetIndex >= 4)
                {
                    break;
                }

                int fromBallIndex = i;
                int toBallIndex = targetIndex;
                int fromBottlelIndex = bottleIndex1;
                int toBottlelIndex = bottleIndex2;

                commands.Add(new SwitchBallCommand
                {
                    type = type,
                    fromBallIndex = fromBallIndex,
                    toBallIndex = toBallIndex,
                    fromBottleIndex = fromBottlelIndex,
                    toBottleIndex = toBottlelIndex,
                });

                targetIndex++;

                if (bottle2Balls.Count == 4)
                {
                    break;
                }

                //bottle1Balls.RemoveAt(i);
                //bottle2Balls.Add(ball);
            }
            else
            {
                break;
            }
        }

        return commands;
    }


    public bool CheckWinCondition()
    {
        bool winFlag = true;
        foreach (Bottle bottle in bottles)
        {
            if (bottle.balls.Count == 0)
                continue;

            if (bottle.balls.Count < 4)
            {
                winFlag = false;
                break;
            }

            bool sameTypeFlag = true;
            BallType type = bottle.balls[0].type;
            foreach (Ball ball in bottle.balls)
            {
                if (ball.type != type)
                {
                    sameTypeFlag = false;
                    break;
                }
            }
            if (!sameTypeFlag)
            {
                winFlag = false;
                break;
            }

        }
        return winFlag;
    }

    public bool CheckWincoditionDFS()
    {
        HashSet<int> visited = new HashSet<int>();

        for (int i = 0; i < bottles.Count; i++)
        {
            if (!visited.Contains(i) && !IsBottleWinCondition(bottles[i]))
            {
                if (!DFS(i, visited))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool DFS(int index, HashSet<int> visited)
    {
        visited.Add(index);

        Bottle bottle = bottles[index];

        // Kiểm tra nếu bình hiện tại không đáp ứng điều kiện thắng
        if (!IsBottleWinCondition(bottle))
        {
            return false;
        }

        for (int i = 0; i < bottles.Count; i++)
        {
            if (!visited.Contains(i) && !IsBottleWinCondition(bottles[i]))
            {
                if (!DFS(i, visited))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsBottleWinCondition(Bottle bottle)
    {
        if (bottle.balls.Count == 0)
            return true;

        if (bottle.balls.Count < 4)
            return false;

        BallType type = bottle.balls[0].type;
        foreach (Ball ball in bottle.balls)
        {
            if (ball.type != type)
            {
                return false;
            }
        }

        return true;
    }

    public class SwitchBallCommand
    {
        public BallType type;

        public int fromBottleIndex;
        public int fromBallIndex;

        public int toBottleIndex;
        public int toBallIndex;

    }

    public class Bottle
    {
        public List<Ball> balls = new List<Ball>();
    }

    public class Ball
    {
        public BallType type;
    }
    public enum BallType
    {
        Orange,
        Blue,
        Green,
        Red,
        Purple,
        Pink,
        Yellow,
        DarkBlue,
        LightBlue,
        LightGreen,
        Brown,
        Gray
    }
}
