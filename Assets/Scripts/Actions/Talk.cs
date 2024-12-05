using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;

[CreateAssetMenu(menuName = "TextGame/InputActions/Talk")]
public class Talk : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            string verb = separatedInputWords[0];
            string noun = separatedInputWords[1];
            if (controller.TestVerbDictionaryWithNoun(controller.interactables.talkDictionary, verb, noun))
            {
                InteractionDataHolder data = controller.interactables.talkDictionary[noun];
                if (data.actionResponse != null)
                {
                    data.actionResponse.DoActionResponse(controller);
                }

                controller.LogStringWithReturn(data.interactionTextResponse);
            }
            else
            {
                controller.LogStringWithReturn("You can't " + verb + " " + noun);
            }
        }
    }
}
