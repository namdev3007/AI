using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleGraphic : MonoBehaviour
{
    public int index;
    public BallGraphic[] ballGraphics;

    private void OnMouseUpAsButton()
    {
        Debug.Log("chon bottle" +  index);
    }

    public void SetGraphic(GameManager.BallType[] ballTypes)
    {
        for (int i = 0; i < ballGraphics.Length; i++)
        {
            if (i >= ballTypes.Length)
            {
                ballGraphics[i].SetColor(BallGraphicType.None); // Khi không có bóng, đặt None
            }
            else
            {
                BallGraphicType type = ConvertBallTypeToGraphicType(ballTypes[i]);
                ballGraphics[i].SetColor(type);  // Gán đúng sprite bóng
            }
        }
    }

    private BallGraphicType ConvertBallTypeToGraphicType(GameManager.BallType ballType)
    {
        // Ánh xạ từ BallType sang BallGraphicType
        switch (ballType)
        {
            case GameManager.BallType.Orange:
                return BallGraphicType.Orange;
            case GameManager.BallType.Blue:
                return BallGraphicType.Blue;
            case GameManager.BallType.Green:
                return BallGraphicType.Green;
            case GameManager.BallType.Red:
                return BallGraphicType.Red;
            case GameManager.BallType.Purple:
                return BallGraphicType.Purple;
            case GameManager.BallType.Pink:
                return BallGraphicType.Pink;
            case GameManager.BallType.Yellow:
                return BallGraphicType.Yellow;
            case GameManager.BallType.DarkBlue:
                return BallGraphicType.DarkBlue;
            case GameManager.BallType.LightBlue:
                return BallGraphicType.LightBlue;
            case GameManager.BallType.LightGreen:
                return BallGraphicType.LightGreen;
            case GameManager.BallType.Brown:
                return BallGraphicType.Brown;
            case GameManager.BallType.Gray:
                return BallGraphicType.Gray;
            default:
                return BallGraphicType.None; // Nếu không có bóng nào phù hợp, đặt None
        }
    }
}
