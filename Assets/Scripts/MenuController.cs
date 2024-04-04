using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
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
    private bool isMenuOpen;

    private void Awake()
    {
        GameManager.OnGamePaused += OpenMenu;
        GameManager.OnGameResumed += CloseMenu;

        // Initialize selected button
        selectedButtonTagPair = menuButtonTagPairs[0];

        // Scale up the initially selected button
        ScaleButton(selectedButtonTagPair.button, 1.2f);
    }
    private void OnDisable()
    {
        GameManager.OnGamePaused -= OpenMenu;
        GameManager.OnGameResumed -= CloseMenu;
    }
    private void OnDestroy()
    {
        GameManager.OnGamePaused -= OpenMenu;
        GameManager.OnGameResumed -= CloseMenu;
    }

    private void OpenMenu()
    {
        isMenuOpen = true;
        menuCanvasGroup.DOFade(1, 0.25f).SetUpdate(true);
        selectedButtonTagPair = menuButtonTagPairs[0];
        ScaleButton(selectedButtonTagPair.button, 1.2f);
    }

    private void CloseMenu()
    {
        isMenuOpen = false;
        menuCanvasGroup.DOFade(0, 0.25f).SetUpdate(true);
    }

    private void Update()
    {
        if (!isMenuOpen)
        {
            return;
        }
        if (InputController.Instance.Player1Actions.menuUpAction.WasPressed || InputController.Instance.Player2Actions.menuUpAction.WasPressed)
        {
            NavigateMenu(-1);
        }
        if (InputController.Instance.Player1Actions.menuDownAction.WasPressed || InputController.Instance.Player2Actions.menuDownAction.WasPressed)
        {
            NavigateMenu(1);
        }
        if (InputController.Instance.Player1Actions.menuEnterAction.WasPressed || InputController.Instance.Player2Actions.menuEnterAction.WasPressed)
        {
            SelectButton(selectedButtonTagPair);
        }
    }

    private void NavigateMenu(int direction)
    {
        Debug.Log("Navigate menu" + direction + ")");
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
            case "resume":
                GameManager.Instance.ResumeGame();
                break;
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