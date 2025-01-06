using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomInteractablesState
{
    private Dictionary<string, bool> objectActivatedDictionary = new Dictionary<string, bool>();

    public RoomInteractablesState() { }

    public void AddObject(string objectName, bool isActive)
    {
        if (objectActivatedDictionary.ContainsKey(objectName.ToLower()))
        {
            return;
        }


        objectActivatedDictionary.Add(objectName.ToLower(), isActive);
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

    public void SetObjectActive(string objectName, bool isObjectActive)
    {
        if (objectActivatedDictionary.ContainsKey(objectName.ToLower()))
        {
            objectActivatedDictionary[objectName.ToLower()] = isObjectActive;
        }
        else
        {
            Debug.Log("Cannot find object -" + objectName + "-");
        }
    }
}
