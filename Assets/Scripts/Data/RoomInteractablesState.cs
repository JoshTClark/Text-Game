using System.Collections;
using System.Collections.Generic;
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
        return objectActivatedDictionary[objectName.ToLower()];
    }

    public bool isCharacterActive(string characterName)
    {
        return characterInteractionActivatedDictionary[characterName.ToLower()];
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
