using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverController : MonoBehaviour
{
    [Serializable]
    public class ButtonTagPair
    {
        public Button button;
        public string tag;
    }

    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private List<ButtonTagPair> menuButtonTagPairs;
    private ButtonTagPair selectedButtonTagPair;
    public bool isGameOverOpen;


    private void Update()
    {
        if (!isGameOverOpen)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateMenu(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateMenu(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter key
        {
            SelectButton(selectedButtonTagPair);
        }
    }
    [Button]
    public void OpenGameOverPanel()
    {
        isGameOverOpen = true;
        menuCanvasGroup.DOFade(1, 0.25f).SetUpdate(true);
        selectedButtonTagPair = menuButtonTagPairs[0];
        ScaleButton(selectedButtonTagPair.button, 1.2f);
    }


    private void NavigateMenu(int direction)
    {
        int currentIndex = menuButtonTagPairs.IndexOf(selectedButtonTagPair);
        int nextIndex = (currentIndex + direction + menuButtonTagPairs.Count) % menuButtonTagPairs.Count;
        ButtonTagPair nextButtonTagPair = menuButtonTagPairs[nextIndex];

        // Scale down the previously selected button
        ScaleButton(selectedButtonTagPair.button, 1f);

        // Scale up the next selected button
        selectedButtonTagPair = nextButtonTagPair;
        ScaleButton(selectedButtonTagPair.button, 1.2f);
    }

    private void ScaleButton(Button button, float scale)
    {
        button.transform.DOScale(new Vector3(scale, scale, 1f), 0.25f).SetUpdate(true);
    }

    private void SelectButton(ButtonTagPair buttonTagPair)
    {
        switch (buttonTagPair.tag)
        {
            case "reset":
                GameManager.Instance.ResetGame();
                break;
            case "exit":
                Application.Quit();
                break;
            // Add more cases as needed based on your tags
            default:
                Debug.LogWarning("Unknown button tag: " + buttonTagPair.tag);
                break;
        }
    }


}
