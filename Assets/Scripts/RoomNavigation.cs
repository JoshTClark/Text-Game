using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;

    private GameController gameController;
    private Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    private void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void UnpackExits()
    {
        for (int i = 0; i < currentRoom.exits.Count; i++)
        {
            foreach (string s in currentRoom.exits[i].keyStrings)
            {
                exitDictionary.Add(s, currentRoom.exits[i].valueRoom);
            }
        }
    }

    public void AttemptToChangeRooms(string input)
    {
        if (exitDictionary.ContainsKey(input))
        {
            currentRoom = exitDictionary[input];
            gameController.DisplayRoomText();
        }
        else
        {
            gameController.LogStringWithReturn("You can not go " + input);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
