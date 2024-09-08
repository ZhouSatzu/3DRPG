using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        Enter,A,B,C
    }
    
    //该传送门自身的tag
    public DestinationTag destinationTag;
}
