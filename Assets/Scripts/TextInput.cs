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

    private void Start()
    {
        inputField.DeactivateInputField();

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

        controller.LogStringWithReturn("   >" + userInput, true);

        OrganizedInputWordsData data = new OrganizedInputWordsData(userInput, controller);

        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            TextInputAction action = controller.inputActions[i];
            foreach (string s in action.keywords)
            {
                if (s == data.verb)
                {
                    action.RespondToInput(controller, data);
                    break;
                }
            }
        }

        InputComplete();
    }

    public void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.text = string.Empty;
    }

    public void CanPlayerType(bool canPlayerType)
    {
        if (canPlayerType)
        {
            inputField.ActivateInputField();
        }
        else
        {
            inputField.DeactivateInputField();
        }
    }
}
