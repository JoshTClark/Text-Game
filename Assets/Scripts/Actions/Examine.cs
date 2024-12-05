using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Examine")]
public class Examine : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        if (seperatedInputWords.Length > 1)
        {
            controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(controller.interactables.examineDictionary, seperatedInputWords[0], seperatedInputWords[1]));
        }
    }
}
