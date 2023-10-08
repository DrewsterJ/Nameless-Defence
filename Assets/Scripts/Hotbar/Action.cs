using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public enum ActionType
    {
        Empty,
        Build
    }

    public ActionType actionType;
}
