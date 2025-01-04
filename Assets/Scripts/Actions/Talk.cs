using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Talk")]
public class Talk : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.hasNoun)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;
            if (controller.TestInputText(wordData))
            {
                noun = wordData.fullNoun;
                Interactable interactable = controller.interactables.currentInteractableDictionary[noun];
                interactable.Talk(new InteractionData(controller, wordData));
            }
            else
            {
                controller.LogStringWithReturn("You can't " + verb + " to " + noun);
            }
        }
    }
}
