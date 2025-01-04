using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableObjectRoomData
{
    public string dataName;
    public Interactable interactableObject;
    public bool activatedAtStart = true;
    public string roomDescriptionKey = "";
}
