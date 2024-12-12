using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Inventory")]
public class Inventory : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
            controller.interactables.DisplayInventory();
    }
}
