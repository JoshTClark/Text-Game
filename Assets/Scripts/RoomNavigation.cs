using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;

    private GameController controller;
    private Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    private void Awake()
    {
        controller = GetComponent<GameController>();
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

    public void AttemptToChangeRooms(OrganizedInputWordsData wordData)
    {
        if (controller.TestInputText(wordData, exitDictionary.Keys.ToList<string>()))
        {
            currentRoom = exitDictionary[wordData.fullNoun];
            controller.DisplayRoomText();
        }
        else
        {
            controller.LogStringWithReturn("You can not go " + wordData.nounFirstWord);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
