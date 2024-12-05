using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Use")]
public class Use : TextInputAction { 
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        controller.interactables.UseItem(seperatedInputWords);
    }
}
