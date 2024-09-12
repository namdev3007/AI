using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelReader : MonoBehaviour
{
    public GameManager gameManager;


    public void LoadLevel(TextAsset textAsset)
    {
        if (textAsset == null)
        {
            Debug.LogError("TextAsset is null, cannot load level.");
            return;
        }

        string[] lines = textAsset.text.Split(new string[] { "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);

        Debug.Log($"Lines count: {lines.Length}");

        int bottleCount = 0;
        int ballPerBottle = 0;

        List<int[]> bottleArray = new List<int[]>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            Debug.Log($"Line {i}: {line}");

            if (i == 0)
            {
                string[] line0Splits = line.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                bottleCount = int.Parse(line0Splits[0]);
                ballPerBottle = int.Parse(line0Splits[1]);
            }
            else
            {
                int[] convertArray = new int[ballPerBottle];

                for (int j = 0; j < convertArray.Length; j++)
                {
                    convertArray[j] = CharacterToInt(line[j]);
                }

                bottleArray.Add(convertArray);
            }
        }

        gameManager.LoadLevel(bottleArray); // Gọi phương thức này từ GameManager để tải level lên.
    }

    private int CharacterToInt(char c)
    {
        switch (c)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            case 'A': case 'a': return 10;
            case 'B': case 'b': return 11;
            case 'C': case 'c': return 12;
            case 'D': case 'd': return 13;
            case 'E': case 'e': return 14;
            case 'F': case 'f': return 15;
            default: return 0;
        }
    }
}
