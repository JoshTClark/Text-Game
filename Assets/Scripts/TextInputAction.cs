using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static TextInput;

public abstract class TextInputAction : ScriptableObject
{
    public List<string> keywords;

    public abstract void RespondToInput(GameController controller, OrganizedInputWordsData wordData);
}
