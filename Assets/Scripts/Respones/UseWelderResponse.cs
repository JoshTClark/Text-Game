using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Use Plasma Welder")]
public class UseWelderResponse : ActionResponse
{
    public override bool DoActionResponse(GameController controller)
    {
        RoomNavigation nav = controller.navigation;
        Interactables inter= controller.interactables;
        if (nav.currentRoom.name == "Ship_Storage" && inter.IsObjectActivated(nav.currentRoom, "VehicleCrate"))
        {
            controller.LogStringWithReturn("Using a plasma welder you don't know how to use to open a box with something you want inside isn't the best idea.");
        }
        else
        {
            controller.LogStringWithReturn("You can't think of anything worthwhile to weld in here.");
        }
        return true;
    }
}
