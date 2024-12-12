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
    public List<CharacterInteractionData> possibleCharacterInteractionsInRoom;
}
