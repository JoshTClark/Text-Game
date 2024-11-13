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
    public List<CharacterRoomData> possibleCharactersInRoom;

    [System.Serializable]
    public class InteractableObjectRoomData 
    {
        public string replaceValue = "string to replace";
        public InteractableObject interactableObject;
        [TextArea]
        public string roomDescription = "Description in room";
    }

    [System.Serializable]
    public class CharacterRoomData
    {
        public string characterDataName = "Name used to activate/deactivate character";
        public Character character;
        public bool activatedAtStart;
        [TextArea]
        public string roomDescription = "Description in room";
        public List<Interaction> interactions;
    }
}
