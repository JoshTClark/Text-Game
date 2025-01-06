using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactables/Levi")]
public class Levi : Interactable
{
    // Text keys and other variables
    public string helmTalkKey;

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
        // Talking to levi in the helm
        if (currentRoom == "Ship_Helm")
        {
            controller.LogStringWithReturn(controller.GetText(helmTalkKey));
            return true;
        }

        return false;
    }

    public override bool Use(InteractionData interactionData)
    {
        return false;
    }
}
