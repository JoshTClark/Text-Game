using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactables/TJ")]
public class TJ : Interactable
{
    // Text keys and other variables
    public string commonsTalkKey1;
    public string commonsTalkKey2;

    // Getting commonly used values
    //string currentRoom = interactionData.controller.navigation.currentRoom.name;
    //GameController controller = interactionData.controller;

    public override bool Examine(InteractionData interactionData)
    {
        return false;
    }

    public override bool Give(InteractionData interactionData)
    {
        return false;
    }

    public override bool Open(InteractionData interactionData)
    {
        return false;
    }

    public override bool Take(InteractionData interactionData)
    {
        return false;
    }

    public override bool Talk(InteractionData interactionData)
    {
        string currentRoom = interactionData.controller.navigation.currentRoom.name;
        if (currentRoom == "Ship_Commons")
        {
            interactionData.controller.LogStringWithReturn(interactionData.controller.GetText(commonsTalkKey1));
            return true;
        }

        return false;
    }

    public override bool Use(InteractionData interactionData)
    {
        return false;
    }
}
