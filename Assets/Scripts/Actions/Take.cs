using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Take")]
public class Take : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.isValid)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;

            if (controller.TestVerbDictionaryWithNoun(controller.interactables.takeDictionary, wordData))
            {
                noun = wordData.fullNoun;
                if (controller.interactables.Take(noun))
                {
                    InteractionDataHolder data = controller.interactables.takeDictionary[noun];
                    if (data.actionResponse != null)
                    {
                        data.actionResponse.DoActionResponse(controller);
                    }

                    controller.LogStringWithReturn(data.interactionTextResponse);
                }
                else 
                {
                    controller.LogStringWithReturn("You can't " + verb + " the " + noun);
                }
            }
            else
            {
                controller.LogStringWithReturn("There is no " + noun + " to " + verb);
            }
        }
    }
}
