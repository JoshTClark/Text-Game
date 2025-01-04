using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InteractionData
{
    public GameController controller;
    public OrganizedInputWordsData inputWords;

    public InteractionData(GameController controller, OrganizedInputWordsData inputWords)
    {
        this.controller = controller;
        this.inputWords = inputWords;
    }
}
