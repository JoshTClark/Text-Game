using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction
{
    public InputAction inputAction;
    [TextArea(1, 100)]
    public string textResponse;
    public ActionResponse actionResponse;
}
