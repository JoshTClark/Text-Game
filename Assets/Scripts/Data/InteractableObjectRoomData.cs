using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableObjectRoomData
{
    public string objectDataName = "";
    public InteractableObject interactableObject;
    public bool activatedAtStart = false;
    [TextArea]
    public string roomDescription = "Description in room";
}
