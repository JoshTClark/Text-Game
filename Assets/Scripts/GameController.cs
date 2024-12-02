using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using static Room;
using Unity.VisualScripting;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayText;

    [HideInInspector]
    public RoomNavigation navigation;
    [HideInInspector]
    public Interactables interactables;
    [HideInInspector]
    public TextInput textInput;

    public InputAction[] inputActions;

    private List<TextActionLog> actionLog = new List<TextActionLog>();

    private GameData gameData;

    // Text animation stuff
    [HideInInspector]
    public bool displayingText = false;
    public int lettersPerSecond;
    private float letterTimer = 0.0f;
    private int actionLogIndex = 0;
    private int visibleCharacters = 0;
    private int textIndex = 0;


    private void Awake()
    {
        textInput = GetComponent<TextInput>();
        navigation = GetComponent<RoomNavigation>();
        interactables = GetComponent<Interactables>();

        gameData = new GameData();
    }

    private void Start()
    {
        DisplayRoomText();
        DisplayLoggedText();
    }

    private void Update()
    {
        if (displayingText)
        {
            textInput.CanPlayerType(false);
            letterTimer += Time.deltaTime;
            string logAsText = string.Join("\n", actionLog);
            displayText.text = logAsText;
            if (letterTimer >= 1.0f / lettersPerSecond)
            {
                if (actionLogIndex < actionLog.Count)
                {
                    TextActionLog currentLog = actionLog[actionLogIndex];
                    if (currentLog.showInstant)
                    {
                        actionLogIndex++;
                        visibleCharacters += currentLog.text.Length;
                        textIndex = 0;
                    }
                    else if (textIndex < currentLog.text.Length)
                    {
                        visibleCharacters++;
                        textIndex++;
                    }
                    else
                    {
                        actionLogIndex++;
                        textIndex = 0;
                    }

                    letterTimer = 0.0f;
                }
                else
                {
                    displayingText = false;
                    textInput.CanPlayerType(true);
                    textIndex = 0;
                }
            }
            displayText.maxVisibleCharacters = visibleCharacters + logAsText.Length - string.Join("", actionLog).Length; ;
        }
    }

    public void DisplayLoggedText()
    {
        displayingText = true;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();
        Room currentRoom = navigation.currentRoom;

        string finalDescription = currentRoom.description;

        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            InteractableObjectRoomData interactableObjectData = currentRoom.interactableObjectsInRoom[i];

            string replaceText = "<" + interactableObjectData.replaceValue + ">";

            if (finalDescription.Contains(replaceText))
            {
                if (!interactables.IsObjectInInventory(currentRoom, interactableObjectData.interactableObject))
                {
                    finalDescription = finalDescription.Replace(replaceText, interactableObjectData.roomDescription);
                }
                else
                {
                    finalDescription = finalDescription.Replace(replaceText, "");
                }
            }
        }

        for (int i = 0; i < currentRoom.possibleCharactersInRoom.Count; i++)
        {
            CharacterRoomData characterRoomData = currentRoom.possibleCharactersInRoom[i];

            string replaceText = "<" + characterRoomData.characterDataName + ">";

            if (finalDescription.Contains(replaceText))
            {
                if (interactables.IsCharacterActivated(characterRoomData.characterDataName))
                {
                    finalDescription = finalDescription.Replace(replaceText, characterRoomData.roomDescription);
                }
                else
                {
                    finalDescription = finalDescription.Replace(replaceText, "");
                }
            }
        }

        LogStringWithReturn(finalDescription);
    }

    public void LogStringWithReturn(string stringToAdd, bool showInstant = false)
    {
        LogString(stringToAdd + "\n", showInstant);
    }

    public void LogString(string stringToAdd, bool showInstant = false)
    {
        TextActionLog log = new TextActionLog();
        log.text = stringToAdd;
        log.showInstant = showInstant;

        actionLog.Add(log);
    }

    public void UnpackRoom()
    {
        PrepareInteractables(navigation.currentRoom);
        navigation.UnpackExits();
    }

    public void PrepareInteractables(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i].interactableObject;
            interactables.AddObjectToRoomObjectList(currentRoom, interactableInRoom);

            for (int j = 0; j < interactableInRoom.interactions.Count; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                for (int k = 0; k < interactableInRoom.keyWords.Count; k++)
                {
                    if (interaction.inputAction.keywords.Contains("examine"))
                    {
                        interactables.examineDictionary.Add(interactableInRoom.keyWords[k].ToLower(), interaction.textResponse);
                    }
                    if (interaction.inputAction.keywords.Contains("take"))
                    {
                        interactables.takeDictionary.Add(interactableInRoom.keyWords[k].ToLower(), interaction.textResponse);
                    }
                }
            }
        }

        for (int i = 0; i < currentRoom.possibleCharactersInRoom.Count; i++)
        {
            CharacterRoomData characterInRoom = currentRoom.possibleCharactersInRoom[i];
            interactables.AddCharacterToRoomCharacterList(currentRoom, characterInRoom);

            for (int j = 0; j < characterInRoom.interactions.Count; j++)
            {
                Interaction interaction = characterInRoom.interactions[j];
                for (int k = 0; k < characterInRoom.character.keyWords.Count; k++)
                {
                    if (interaction.inputAction.keywords.Contains("talk") && interactables.IsCharacterActivated(characterInRoom.characterDataName))
                    {
                        interactables.talkDictionary.Add(characterInRoom.character.keyWords[k].ToLower(), interaction.textResponse);
                    }
                }
            }
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string, string> verbDictionary, string verb, string noun)
    {
        noun = noun.ToLower();
        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        return "You can't " + verb + " " + noun;
    }

    public void ClearCollectionsForNewRoom()
    {
        navigation.ClearExits();
        interactables.ClearCollections();
    }

    private class TextActionLog
    {
        public bool showInstant;
        public string text;

        public override string ToString()
        {
            return text;
        }
    }
}
