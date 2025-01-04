using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteractableController;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Examine")]
public class Examine : TextInputAction
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
                interactable.Examine(new InteractionData(controller, wordData));
            }
            else
            {
                controller.LogStringWithReturn("You can't " + verb + " " + noun);
            }
        }
    }
}
