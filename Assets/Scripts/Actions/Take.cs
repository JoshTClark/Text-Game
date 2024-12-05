using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;

[CreateAssetMenu(menuName = "TextGame/InputActions/Take")]
public class Take : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        Dictionary<string, InteractionDataHolder> takeDictionary = controller.interactables.Take(separatedInputWords);

        if (separatedInputWords.Length > 1)
        {
            string verb = separatedInputWords[0];
            string noun = separatedInputWords[1];
            if (controller.TestVerbDictionaryWithNoun(takeDictionary, verb, noun))
            {
                InteractionDataHolder data = takeDictionary[noun];
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
