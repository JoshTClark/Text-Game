using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InputAction : ScriptableObject
{
    public List<string> keywords;

    public abstract void RespondToInput(GameController controller, string[] seperatedInputWords);
}
