using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Room")]
public class Room : ScriptableObject
{
    [TextArea(1,100)]
    public string description;

    public List<Exit> exits = new List<Exit>();
    public List<InteractableObjectRoomData> interactableObjectsInRoom;

    [System.Serializable]
    public class InteractableObjectRoomData 
    {
        public string replaceValue = "string to replace";
        public InteractableObject interactableObject;
        [TextArea]
        public string roomDescription = "Description in room";
    }
}
