using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Room")]
public class Room : ScriptableObject
{
    public string descriptionKey;

    public List<Exit> exits = new List<Exit>();
    public List<InteractableObjectRoomData> interactableObjectsInRoom;
}
