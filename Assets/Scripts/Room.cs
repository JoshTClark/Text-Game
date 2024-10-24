using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Room")]
public class Room : ScriptableObject
{
    [TextArea]
    public string description;
    public string roomName;

    public List<Exit> exits = new List<Exit>();
    public List<InteractableObjectRoomData> interactableObjectsInRoom;

    [System.Serializable]
    public class InteractableObjectRoomData 
    {
        public InteractableObject interactableObject;
        [TextArea]
        public string roomDescription = "Description in room";
    }
}
