using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        //同场景或异场景传送
        SameScene,DiffentScene
    }

    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    //要传送的目标点的tag
    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    private void Update()
    {
        if(canTrans && Input.GetKeyDown(KeyCode.E))
        {
            //执行传送
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
