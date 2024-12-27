using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomInteractablesState
{
    private Dictionary<string, bool> objectActivatedDictionary = new Dictionary<string, bool>();
    private Dictionary<string, bool> characterInteractionActivatedDictionary = new Dictionary<string, bool>();

    public RoomInteractablesState() { }

    public void AddObject(string objectName, bool isActive)
    {
        if (objectActivatedDictionary.ContainsKey(objectName.ToLower()))
        {
            return;
        }


        objectActivatedDictionary.Add(objectName.ToLower(), isActive);
    }

    public void AddCharacter(string characterName, bool isActive)
    {
        if (characterInteractionActivatedDictionary.ContainsKey(characterName.ToLower()))
        {
            return;
        }

        characterInteractionActivatedDictionary.Add(characterName.ToLower(), isActive);
    }

    public bool isObjectActive(string objectName)
    {
        if (objectActivatedDictionary.ContainsKey(objectName.ToLower()))
        {
            return objectActivatedDictionary[objectName.ToLower()];
        }

        Debug.Log("Couldn't find an object with name -" + objectName.ToLower() + "-");
        foreach (string key in objectActivatedDictionary.Keys)
        {
            Debug.Log(key);
        }
        return false;
    }

    public bool isCharacterActive(string characterName)
    {
        if (characterInteractionActivatedDictionary.ContainsKey(characterName.ToLower()))
        {
            return characterInteractionActivatedDictionary[characterName.ToLower()];
        }

        Debug.Log("Couldn't find an character with name -" + characterName.ToLower() + "-");
        return false;
    }

    public void SetObjectActive(string objectName, bool isObjectActive)
    {
        objectActivatedDictionary[objectName.ToLower()] = isObjectActive;
    }

    public void SetCharacterActive(string characterName, bool isCharacterActive)
    {
        characterInteractionActivatedDictionary[characterName.ToLower()] = isCharacterActive;
    }
}
