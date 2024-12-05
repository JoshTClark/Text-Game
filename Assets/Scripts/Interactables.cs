using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using static Room;

public class Interactables : MonoBehaviour
{
    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> talkDictionary = new Dictionary<string, string>();

    [HideInInspector]
    public List<InteractableObject> objectsInRoom = new List<InteractableObject>();

    [HideInInspector]
    public List<CharacterRoomData> charactersInRoom = new List<CharacterRoomData>();

    private List<InteractableObject> objectsInInventory = new List<InteractableObject>();
    private Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    private Dictionary<string, bool> objectActivatedDictionary = new Dictionary<string, bool>();
    private Dictionary<string, bool> characterActivatedDictionary = new Dictionary<string, bool>();
    private GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void AddObjectToRoomObjectList(Room currentRoom, InteractableObject interactableObject)
    {
        if (!objectsInInventory.Contains(interactableObject))
        {
            objectsInRoom.Add(interactableObject);
        }
    }

    public void AddCharacterToRoomCharacterList(Room currentRoom, CharacterRoomData characterInRoom)
    {
        charactersInRoom.Add(characterInRoom);
        characterActivatedDictionary.Add(characterInRoom.characterDataName, characterInRoom.activatedAtStart);
    }

    public bool IsObjectInInventory(Room currentRoom, InteractableObject interactableObject)
    {
        if (objectsInInventory.Contains(interactableObject))
        {
            return true;
        }

        return false;
    }

    public void AddActionResponsesToUseDictionary()
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
                        useDictionary.Add(interactableObjectInIventory.keyWords[k], interaction.actionResponse);
                    }
                }
            }
        }
    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("You look inside your bagpack, inside you have: ");

        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            controller.LogStringWithReturn(objectsInInventory[i].objectName);
        }
    }

    public bool IsCharacterActivated(string characterDataName)
    {
        if (characterActivatedDictionary.ContainsKey(characterDataName))
        {
            return characterActivatedDictionary[characterDataName];
        }
        Debug.Log("characterDataName not found");
        return false;
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        objectsInRoom.Clear();
        characterActivatedDictionary.Clear();
    }

    public Dictionary<string, string> Take(string[] seperatedInputWords)
    {
        string noun = seperatedInputWords[1].ToLower();

        for (int i = 0; i < objectsInRoom.Count; i++)
        {
            if (objectsInRoom[i].keyWords.Contains(noun))
            {
                objectsInInventory.Add(objectsInRoom[i]);
                AddActionResponsesToUseDictionary();
                objectsInRoom.Remove(objectsInRoom[i]);
                return takeDictionary;
            }
        }

        controller.LogStringWithReturn("There is no " + noun + " to take.");
        return null;
    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];

        for (int i = 0; i < objectsInRoom.Count; i++)
        {
            if (objectsInInventory[i].keyWords.Contains(nounToUse))
            {
                if (useDictionary.ContainsKey(nounToUse))
                {
                    bool actionResult = useDictionary[nounToUse].DoActionResponse(controller);
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
