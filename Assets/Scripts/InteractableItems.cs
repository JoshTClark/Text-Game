using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    public List<InteractableObject> usableItemList = new List<InteractableObject>();

    [HideInInspector]
    public List<InteractableObject> objectsInRoom = new List<InteractableObject>();

    private List<InteractableObject> objectsInInventory = new List<InteractableObject>();
    private Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    private GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public string GetObjectsNotInInventory(Room currentRoom, int i)
    {
        InteractableObject interactableObject = currentRoom.interactableObjectsInRoom[i].interactableObject;

        if (!objectsInInventory.Contains(interactableObject))
        {
            objectsInRoom.Add(interactableObject);
            return currentRoom.interactableObjectsInRoom[i].roomDescription;
        }

        return null;
    }

    public void AddActionResponsesToUseDictionary()
    {
        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            string noun = objectsInInventory[i].objectName;

            InteractableObject interactableObjectInIventory = GetInteractableObjectFromUsableList(noun);
            if (interactableObjectInIventory == null)
            {
                continue;
            }
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

    public InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for (int i = 0; i < usableItemList.Count; i++)
        {
            if (usableItemList[i].keyWords.Contains(noun.ToLower()))
            {
                return usableItemList[i];
            }
        }
        return null;
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
        objectsInRoom.Clear();
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
