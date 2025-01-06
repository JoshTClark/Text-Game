using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactables/Allie")]
public class Allie : Interactable
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
        GameController controller = interactionData.controller;
        if (currentRoom == "Ship_Commons")
        {
            if (controller.GetFlag(GameData.GlobalFlag.HasInteractedWithCrate))
            {
                // After interacting with the vehicle crate
                controller.LogStringWithReturn(controller.GetText(commonsTalkKey2));
            }
            else
            {
                // Before interacting with the crate
                controller.LogStringWithReturn(controller.GetText(commonsTalkKey1));
            }
            return true;
        }

        return false;
    }

    public override bool Use(InteractionData interactionData)
    {
        return false;
    }
}
