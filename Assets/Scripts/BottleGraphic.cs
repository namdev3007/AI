using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleGraphic : MonoBehaviour
{
    public GameGraphic gameGraphic;

    public int index;

    public BallGraphic[] ballGraphics;

    public Transform bottleUpTransfrom;

    private void OnMouseUpAsButton()
    {
        gameGraphic.OnClickBottle(index);
    }

    public void SetGraphic(GameManager.BallType[] ballTypes)
    {
        for (int i = 0; i < ballGraphics.Length; i++)
        {
            if (i >= ballTypes.Length)
            {
                ballGraphics[i].SetColor(BallGraphicType.None); 
            }
            else
            {
                SetGraphic(i, ballTypes[i]);
            }
        }
    }

    private BallGraphicType ConvertBallTypeToGraphicType(GameManager.BallType ballType)
    {
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
                return BallGraphicType.None;
        }
    }
    public void SetGraphic(int index, GameManager.BallType ballType)
    {
        BallGraphicType colorType;

        switch (ballType)
        {
            case GameManager.BallType.Red:
                colorType = BallGraphicType.Red;
                break;
            case GameManager.BallType.Green:
                colorType = BallGraphicType.Green;
                break;
            case GameManager.BallType.Orange:
                colorType = BallGraphicType.Orange;
                break;
            case GameManager.BallType.Blue:
                colorType = BallGraphicType.Blue;
                break;
            case GameManager.BallType.Purple:
                colorType = BallGraphicType.Purple;
                break;
            case GameManager.BallType.Pink:
                colorType = BallGraphicType.Pink;
                break;
            case GameManager.BallType.Yellow:
                colorType = BallGraphicType.Yellow;
                break;
            case GameManager.BallType.DarkBlue:
                colorType = BallGraphicType.DarkBlue;
                break;
            case GameManager.BallType.LightBlue:
                colorType = BallGraphicType.LightBlue;
                break;
            case GameManager.BallType.LightGreen:
                colorType = BallGraphicType.LightGreen;
                break;
            case GameManager.BallType.Brown:
                colorType = BallGraphicType.Brown;
                break;
            case GameManager.BallType.Gray:
                colorType = BallGraphicType.Gray;
                break;
            default:
                colorType = BallGraphicType.None;
                break;
        }

        ballGraphics[index].SetColor(colorType);
    }

    public void SetGraphicNone(int index) 
    {
        ballGraphics[index].SetColor(BallGraphicType.None);
    }

    public Vector3 GetBallPosition(int index)
    {
        if (index < 0 || index >= ballGraphics.Length)
        {
            Debug.LogError($"Index {index} is out of range in ballGraphics array.");
            return Vector3.zero; // Trả về giá trị mặc định hoặc xử lý lỗi khác.
        }
        return ballGraphics[index].transform.position;
    }


    public Vector3 GetBottleUpPosition()
    {
        return bottleUpTransfrom.position;
    }
}
