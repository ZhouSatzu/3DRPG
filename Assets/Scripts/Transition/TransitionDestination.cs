using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        Enter,A,B,C
    }
    
    //�ô����������tag
    public DestinationTag destinationTag;
}
