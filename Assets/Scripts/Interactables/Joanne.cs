using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactables/Joanne")]
public class Joanne : Interactable
{
    // Text keys and other variables

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
        return false;
    }

    public override bool Use(InteractionData interactionData)
    {
        return false;
    }
}