using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static Room;

public class Interactables : MonoBehaviour
{
    public Dictionary<string, InteractionDataHolder> examineDictionary = new Dictionary<string, InteractionDataHolder>();
    public Dictionary<string, InteractionDataHolder> takeDictionary = new Dictionary<string, InteractionDataHolder>();
    public Dictionary<string, InteractionDataHolder> talkDictionary = new Dictionary<string, InteractionDataHolder>();
    public Dictionary<string, InteractionDataHolder> useDictionary = new Dictionary<string, InteractionDataHolder>();
    public Dictionary<string, InteractionDataHolder> openDictionary = new Dictionary<string, InteractionDataHolder>();

    [HideInInspector]
    public List<InteractableObject> objectsInRoom = new List<InteractableObject>();

    [HideInInspector]
    public List<CharacterInteractionData> characterInteractionsInRoom = new List<CharacterInteractionData>();

    [HideInInspector]
    public List<InteractableObject> objectsInInventory = new List<InteractableObject>();

    private Dictionary<string, RoomInteractablesState> roomDictionary = new Dictionary<string, RoomInteractablesState>();
    private GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void SetUpInteractablesInRoom(Room currentRoom)
    {
        if (!roomDictionary.ContainsKey(currentRoom.name)) 
        {
            RoomInteractablesState roomState = new RoomInteractablesState();
            foreach (InteractableObjectRoomData i in currentRoom.interactableObjectsInRoom)
            {
                bool isObjectActive = i.activatedAtStart && !IsObjectInInventory(i.interactableObject);
                roomState.AddObject(i.objectDataName, isObjectActive);
            }

            foreach (CharacterInteractionData i in currentRoom.possibleCharacterInteractionsInRoom)
            {
                bool isCharacterActive = i.activatedAtStart;
                roomState.AddCharacter(i.characterDataName, isCharacterActive);
            }

            roomDictionary.Add(currentRoom.name, roomState);
        }

        RoomInteractablesState currentState = roomDictionary[currentRoom.name];

        foreach (InteractableObjectRoomData i in currentRoom.interactableObjectsInRoom)
        {
            if (currentState.isObjectActive(i.objectDataName)) 
            {
                objectsInRoom.Add(i.interactableObject);
            }
        }
        foreach (CharacterInteractionData i in currentRoom.possibleCharacterInteractionsInRoom)
        {
            if (currentState.isCharacterActive(i.characterDataName))
            {
                characterInteractionsInRoom.Add(i);
            }
        }

        AddIventoryToUseDictionary();
    }

    public bool IsObjectInInventory(InteractableObject interactableObject)
    {
        if (objectsInInventory.Contains(interactableObject))
        {
            return true;
        }

        return false;
    }

    public bool IsCharacterActivated(Room room, string characterName)
    {
        return roomDictionary[room.name].isCharacterActive(characterName);
    }

    public bool IsObjectActivated(Room room, string objectName)
    {
        return roomDictionary[room.name].isObjectActive(objectName);
    }

    public void AddIventoryToUseDictionary()
    {
        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            InteractableObject interactableObjectInIventory = objectsInInventory[i];
            for (int j = 0; j < interactableObjectInIventory.interactions.Count; j++)
            {
                Interaction interaction = interactableObjectInIventory.interactions[j];
                if (interaction.actionResponse == null)
                {
                    continue;
                }

                for (int k = 0; k < interactableObjectInIventory.keyWords.Count; k++)
                {
                    if (!useDictionary.ContainsKey(interactableObjectInIventory.keyWords[k]))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        useDictionary.Add(interactableObjectInIventory.keyWords[k], holder);
                    }
                }
            }
        }
    }

    public void SetObjectActive(string objectName, bool isObjectActive, Room room)
    {
        roomDictionary[room.name].SetObjectActive(objectName, isObjectActive);
    }

    public void SetCharacterActive(string characterName, bool isCharacterActive, Room room)
    {
        roomDictionary[room.name].SetCharacterActive(characterName, isCharacterActive);
    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("You look inside your bagpack, inside you have: ");

        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            controller.LogStringWithReturn(objectsInInventory[i].objectName);
        }
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        talkDictionary.Clear();
        useDictionary.Clear();
        openDictionary.Clear();
        objectsInRoom.Clear();
        characterInteractionsInRoom.Clear();
    }

    public bool Take(string noun)
    {
        noun = noun.ToLower();

        for (int i = 0; i < objectsInRoom.Count; i++)
        {
            if (objectsInRoom[i].keyWords.Contains(noun))
            {
                objectsInInventory.Add(objectsInRoom[i]);
                objectsInRoom.Remove(objectsInRoom[i]);
                ClearCollections();
                controller.PrepareInteractables(controller.navigation.currentRoom);
                return true;
            }
        }

        return false;
    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];

        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            if (objectsInInventory[i].keyWords.Contains(nounToUse))
            {
                if (useDictionary.ContainsKey(nounToUse))
                {
                    bool actionResult = useDictionary[nounToUse].actionResponse.DoActionResponse(controller);
                    if (!actionResult)
                    {
                        controller.LogStringWithReturn("Nothing happens.");
                        return;
                    }
                }
                else
                {
                    controller.LogStringWithReturn("You can't use the " + nounToUse + ".");
                    return;
                }
            }
        }
        controller.LogStringWithReturn("There is no " + nounToUse + " in your inventory to use.");
        return;
    }
}
