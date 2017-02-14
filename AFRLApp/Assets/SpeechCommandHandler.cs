using UnityEngine;
using System.Collections;

public class VoiceCommandHandler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void onNextImageGlobal()
    {
        Debug.Log("inside onNextImageGlobal");
    }

    void onPreviousImageGlobal()
    {
        Debug.Log("inside onPreviousGlobal");
    }

    void onLatestImageGlobal()
    {
        Debug.Log("inside onLatestImageGlobal");
    }

    void onFollowGlobal()
    {
        Debug.Log("inside onFollowGlobal");
    }

    void onStopFollowingGlobal()
    {
        Debug.Log("inside onStopFollowingGlobal");
    }
}
