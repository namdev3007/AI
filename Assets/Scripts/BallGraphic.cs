using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallGraphicType
{
    None,
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

public class BallGraphic : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public List<Sprite> ballSprites;
    
    private Dictionary<BallGraphicType, Sprite> spriteDictionary;

    private void Awake()
    {
        InitializeSpriteDictionary();
    }

    private void InitializeSpriteDictionary()
    {
        // Khởi tạo từ điển và ánh xạ loại bóng với sprite tương ứng
        spriteDictionary = new Dictionary<BallGraphicType, Sprite>
        {
            { BallGraphicType.Orange, ballSprites[0] },
            { BallGraphicType.Blue, ballSprites[1] },
            { BallGraphicType.Green, ballSprites[2] },
            { BallGraphicType.Red, ballSprites[3] },
            { BallGraphicType.Purple, ballSprites[4] },
            { BallGraphicType.Pink, ballSprites[5] },
            { BallGraphicType.Yellow, ballSprites[6] },
            { BallGraphicType.DarkBlue, ballSprites[7] },
            { BallGraphicType.LightBlue, ballSprites[8] },
            { BallGraphicType.LightGreen, ballSprites[9] },
            { BallGraphicType.Brown, ballSprites[10] },
            { BallGraphicType.Gray, ballSprites[11] }
        };
    }

    public void SetColor(BallGraphicType type)
    {
        if (spriteDictionary.TryGetValue(type, out Sprite newSprite))
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    public static BallGraphicType ConvertFromGameType(GameManager.BallType ballType)
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

}
