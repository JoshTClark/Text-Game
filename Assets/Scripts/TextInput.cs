using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TextInput : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    private GameController controller;

    void Start()
    {
        inputField.ActivateInputField();
        inputField.Select();

        controller = GetComponent<GameController>();
        inputField.onEndEdit.AddListener(AcceptStringInput);
        inputField.onDeselect.AddListener((string input) => { inputField.Select(); });
    }

    public void AcceptStringInput(string userInput)
    {
        userInput = userInput.ToLower();

        if (string.IsNullOrEmpty(userInput))
        {
            inputField.ActivateInputField();
            return;
        }

        controller.LogStringWithReturn("   >" + userInput);

        char[] delimiterCharacters = { ' ' };
        string[] seperatedInputWords = userInput.Split(delimiterCharacters);

        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            InputAction action = controller.inputActions[i];
            foreach (string s in action.keywords)
            {
                if (s == seperatedInputWords[0])
                {
                    action.RespondToInput(controller, seperatedInputWords);
                    break;
                }
            }
        }

        InputComplete();
    }

    public void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = string.Empty;
    }
}
