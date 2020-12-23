using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Constants;
using Utilities.MenuSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    Button _exitButton;

    void Awake() => MenuManager.Initialize();

    void Start() => _exitButton.onClick.AddListener(HandleExitButtonClicked);

    void HandleExitButtonClicked() => MenuManager.ExitMenuAndLoadScene("MainMenu");
}
