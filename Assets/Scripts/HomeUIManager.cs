    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class HomeUIManager : MonoBehaviour
    {
        public TextMeshProUGUI text_level;
        public TextMeshProUGUI addBottleCount;

        public GameLevelReader levelReader;
        public GameManager gameManager;
        public GameLevelManager gameLevelManager;
        public GameGraphic gameGraphic;

        public System.Action OnClickButtonSetting;
        public System.Action OnClickButtonReload;
        public System.Action OnClickButtonHint;
        public System.Action OnClickButtonUndo;
        public System.Action OnClickButtonAddBottle;

        private int addBottleClickCount = 2; 

        void Start()
        {
            OnClickButtonUndo = gameManager.Undo;
            OnClickButtonAddBottle = gameGraphic.AddNewBottle;
            OnClickButtonReload = gameLevelManager.ReloadLevel;

            UpdateAddBottleCountText(); 
        }

        public void SetLevelText(int level)
        {
            text_level.text = "Level " + level.ToString();
        }

        public void Click_ButtonUndo()
        {
            OnClickButtonUndo?.Invoke();
        }

        public void Click_ButtonReload()
        {
            OnClickButtonReload?.Invoke();
        }    

        public void Click_ButtonAddBottle()
        {
            if (addBottleClickCount > 0)
            {
                OnClickButtonAddBottle?.Invoke();
                addBottleClickCount--;
                UpdateAddBottleCountText();
            }
        }

        private void UpdateAddBottleCountText()
        {
            addBottleCount.text =  addBottleClickCount.ToString();
        }

    public void Click_ButtonHint()
    {
        if (gameManager != null)
        {
            bool moveMade = gameManager.ExecuteHint();

            if (moveMade && gameGraphic.selectedBottleIndex != -1)
            {
                StartCoroutine(gameGraphic.MoveBallDown(gameGraphic.selectedBottleIndex));
            }
        }
    }



}
