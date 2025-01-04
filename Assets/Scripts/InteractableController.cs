using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static Room;

public class InteractableController : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, Interactable> currentInteractableDictionary = new Dictionary<string, Interactable>();

    [HideInInspector]
    public List<Interactable> objectsInRoom = new List<Interactable>();

    [HideInInspector]
    public List<Interactable> objectsInInventory = new List<Interactable>();

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
                roomState.AddObject(i.interactableObject.objectName.ToLower(), isObjectActive);
            }

            roomDictionary.Add(currentRoom.name, roomState);
        }

        RoomInteractablesState currentState = roomDictionary[currentRoom.name];

        foreach (InteractableObjectRoomData i in currentRoom.interactableObjectsInRoom)
        {
            if (currentState.isObjectActive(i.interactableObject.objectName))
            {
                objectsInRoom.Add(i.interactableObject);
                foreach (string s in i.interactableObject.keyWords) 
                {
                    currentInteractableDictionary.Add(s.ToLower(), i.interactableObject);
                }
            }
        }

        foreach (Interactable i in objectsInInventory)
        {
            foreach (string s in i.keyWords)
            {
                currentInteractableDictionary.Add(s.ToLower(), i);
            }
        }
    }

    public bool IsObjectInInventory(Interactable interactableObject)
    {
        if (objectsInInventory.Contains(interactableObject))
        {
            return true;
        }

        return false;
    }

    public bool IsObjectActivated(Room room, string objectName)
    {
        return roomDictionary[room.name].isObjectActive(objectName);
    }

    public void SetObjectActive(string objectName, bool isObjectActive, Room room)
    {
        if (roomDictionary.ContainsKey(room.name))
        {
            roomDictionary[room.name].SetObjectActive(objectName, isObjectActive);
            controller.PrepareInteractables(controller.navigation.currentRoom);
        }
        else
        {
            Debug.Log("Cannot find room -" + room.name + "-");
        }
    }

    public void SetCharacterActive(string characterName, bool isCharacterActive, Room room)
    {
        if (roomDictionary.ContainsKey(room.name))
        {
            roomDictionary[room.name].SetCharacterActive(characterName, isCharacterActive);
            controller.PrepareInteractables(controller.navigation.currentRoom);
        }
        else
        {
            Debug.Log("Cannot find room -" + room.name + "-");
        }
    }

    public void DisplayInventory()
    {
        if (objectsInInventory.Count > 0)
        {
            controller.LogStringWithReturn("You look inside your suit's bag, inside you have: ");

            for (int i = 0; i < objectsInInventory.Count; i++)
            {
                controller.LogStringWithReturn(objectsInInventory[i].objectName);
            }
        }
        else
        {
            controller.LogStringWithReturn("Your suit's bag is empty.");
        }
    }

    public void ClearCollections()
    {
        objectsInRoom.Clear();
    }

    public void Take(string noun)
    {
        noun = noun.ToLower();

        for (int i = 0; i < objectsInRoom.Count; i++)
        {
            if (objectsInRoom[i].keyWords.Contains(noun))
            {
                objectsInInventory.Add(objectsInRoom[i]);
                SetObjectActive(noun, false, controller.navigation.currentRoom);
                return;
            }
        }
    }

    public void RemoveFromInventory(string noun)
    {
        for (int i = 0; i < objectsInInventory.Count; i++)
        {
            if (objectsInInventory[i].keyWords.Contains(noun))
            {
                objectsInInventory.RemoveAt(i);
                ClearCollections();
                controller.PrepareInteractables(controller.navigation.currentRoom);
            }
        }
    }
}
