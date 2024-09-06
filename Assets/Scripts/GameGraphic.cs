using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGraphic : MonoBehaviour
{
    public int selectedBottleIndex = -1;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    public List<BottleGraphic> bottleGraphics;
    public void RefreshBottleGraphic(List<GameManager.Bottle> bottles)
    {
        for (int i = 0; i < bottles.Count; i++)
        {
            GameManager.Bottle gb = bottles[i];
            BottleGraphic bottleGraphic = bottleGraphics[i];

            List<GameManager.BallType> ballTypes = new List<GameManager.BallType>();

            foreach(var ball in gb.balls)
            {
                ballTypes.Add(ball.type);
            }
            bottleGraphic.SetGraphic(ballTypes.ToArray());
        }
    }

    public void OnClickBottle(int bottleIndex)
    {
        if (selectedBottleIndex == -1)
        {
            selectedBottleIndex = bottleIndex;
        }
        else 
        {
            gameManager.SwitchBall(selectedBottleIndex, bottleIndex);
            selectedBottleIndex = -1;
        }
    }    
}
