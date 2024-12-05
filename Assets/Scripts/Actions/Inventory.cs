using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Inventory")]
public class Inventory : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        controller.interactables.DisplayInventory();
    }
}
