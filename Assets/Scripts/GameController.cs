using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayText;

    [HideInInspector]
    public List<string> interactionDescriptionsInRoom;
    [HideInInspector]
    public RoomNavigation navigation;
    [HideInInspector]
    public InteractableItems interactableItems;
    [HideInInspector]
    public TextInput textInput;

    public InputAction[] inputActions;

    private List<TextActionLog> actionLog = new List<TextActionLog>();
    private List<string> finalActionLog = new List<string>();

    // Text animation stuff
    [HideInInspector]
    public bool displayingText = false;
    public int lettersPerSecond;
    private float letterTimer = 0.0f;
    private int actionLogIndex = 0;
    private int textIndex = 0;


    private void Awake()
    {
        textInput = GetComponent<TextInput>();
        navigation = GetComponent<RoomNavigation>();
        interactableItems = GetComponent<InteractableItems>();
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
            if (letterTimer >= 1.0f / lettersPerSecond)
            {
                if (actionLogIndex < actionLog.Count)
                {
                    TextActionLog currentLog = actionLog[actionLogIndex];
                    if (actionLogIndex >= finalActionLog.Count)
                    {
                        finalActionLog.Add("");
                    }
                    if (currentLog.showInstant) 
                    {
                        finalActionLog[actionLogIndex] = currentLog.text;
                        actionLogIndex++;
                        textIndex = 0;
                    }
                    else if (textIndex < currentLog.text.Length)
                    {
                        //Debug.Log(currentLog.text[textIndex]);
                        //Debug.Log(actionLogIndex);
                        //Debug.Log(textIndex);
                        finalActionLog[actionLogIndex] += currentLog.text[textIndex];
                        textIndex++;
                    }
                    else
                    {
                        actionLogIndex++;
                        textIndex = 0;
                    }
                }
                else
                {
                    textInput.CanPlayerType(true);
                    displayingText = false;
                }
                letterTimer = 0.0f;
            }

            string logAsText = string.Join("\n", finalActionLog);

            displayText.text = logAsText;
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

        string joinedInteractionDescriptions = string.Join(" ", interactionDescriptionsInRoom);

        string combined = navigation.currentRoom.description + "\n" + joinedInteractionDescriptions;

        LogStringWithReturn(combined);
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
        PrepareObjects(navigation.currentRoom);
        navigation.UnpackExits();
    }

    public void PrepareObjects(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            string descriptionNotInInventory = interactableItems.GetObjectsNotInInventory(currentRoom, i);

            if (descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }

            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i].interactableObject;
            for (int j = 0; j < interactableInRoom.interactions.Count; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                for (int k = 0; k < interactableInRoom.keyWords.Count; k++)
                {
                    if (interaction.inputAction.keywords.Contains("examine"))
                    {
                        interactableItems.examineDictionary.Add(interactableInRoom.keyWords[k].ToLower(), interaction.textResponse);
                    }
                    if (interaction.inputAction.keywords.Contains("take"))
                    {
                        interactableItems.takeDictionary.Add(interactableInRoom.keyWords[k].ToLower(), interaction.textResponse);
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
        interactionDescriptionsInRoom.Clear();
        navigation.ClearExits();
        interactableItems.ClearCollections();
    }

    private class TextActionLog
    {
        public bool showInstant;
        public string text;
    }
}
