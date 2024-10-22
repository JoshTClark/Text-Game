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

    public InputAction[] inputActions;
    //public string[] ignoredWords;
    private List<string> actionLog = new List<string>();


    private void Awake()
    {
        navigation = GetComponent<RoomNavigation>();
        interactableItems = GetComponent<InteractableItems>();
    }

    void Start()
    {
        DisplayRoomText();
        DisplayLoggedText();
    }

    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog);

        displayText.text = logAsText;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();

        string joinedInteractionDescriptions = string.Join(" ", interactionDescriptionsInRoom);

        string combined = navigation.currentRoom.description + "\n" + joinedInteractionDescriptions;

        LogStringWithReturn(combined);
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

    public void LogString(string stringToAdd)
    {
        actionLog.Add(stringToAdd);
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

            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];
            for (int j = 0; j < interactableInRoom.interactions.Count; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                if (interaction.inputAction.keywords.Contains("examine"))
                {
                    interactableItems.examineDictionary.Add(interactableInRoom.noun.ToLower(), interaction.textResponse);
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
}
