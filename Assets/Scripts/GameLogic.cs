using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public List<Bottle> bottles;

    private void Start()
    {
        bottles = new List<Bottle>();

        bottles.Add(new Bottle()
        {
            balls = new List<Ball> { new Ball { type = BallType.Red }, new Ball { type = BallType.Red } }
        });

        bottles.Add(new Bottle()
        {
            balls = new List<Ball>()
        });


        PrintBottles();

        SwitchBall(bottles[0], bottles[1]);

        PrintBottles();
    }

    public void PrintBottles()
    {
        Debug.Log("Bottles =====");
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < bottles.Count; i++)
        {

            Bottle b = bottles[i];
            sb.Append("Bottle " + (i + 1) + ":");
            foreach (Ball ball in b.balls)
            {
                sb.Append(" " + ball.type);
                sb.Append(", ");
            }
            Debug.Log(sb.ToString());
            sb.Clear();
        }

    }

    public void SwitchBall(Bottle bottle_1, Bottle bottle_2)
    {
        List<Ball> bottle_1Balls = bottle_1.balls;
        List<Ball> bottle_2Balls = bottle_2.balls;

        if (bottle_1Balls.Count == 0)
            return;
        if (bottle_1Balls.Count == 4)
            return;

        int index = bottle_1Balls.Count - 1;
        Ball b = bottle_1Balls[index];

        var type = b.type;

        if(bottle_2Balls.Count > 0 && bottle_2Balls[bottle_2Balls.Count -1].type !=type)
        {
            return;
        }

        for (int i = index; i >= 0; i--) 
        {
            Ball ball = bottle_1Balls[i];
            if(ball.type == type)
            {
                bottle_1Balls.RemoveAt(i);
                bottle_2Balls.Add(ball);
            }
            else
            {
                break;
            }
        }
    }
    
    public bool CheckWincodition()
    {
        bool winFlag = true;
        foreach (Bottle bottle in bottles) 
        {
            if (bottle.balls.Count == 0)
                continue;

            if(bottle.balls.Count < 4)
            {
                winFlag = false;
                break;
            }

            bool sameTypeFlag = true;
            BallType type = bottle.balls[0].type;
            foreach(Ball ball in bottle.balls)
            {
                if(ball.type != type)
                {
                    sameTypeFlag = false;
                    break;
                }
            }
            if(!sameTypeFlag)
            {
                winFlag = false;
                break;
            }

        }
        return winFlag;
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
