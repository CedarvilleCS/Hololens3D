using UnityEngine;
using System.Collections;

public class VoiceCommandHandler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    public void OnGalleryOpenGlobal()
    {
        Debug.Log("inside OnGalleryOpenGlobal");
        this.gameObject.transform.GetChild(5).GetComponent<ShowGalleryButtonScript>().showGalleryWindow();
    }

    void OnNextImageGlobal()
    {
        Debug.Log("inside onNextImageGlobal");
    }

    void OnPreviousImageGlobal()
    {
        Debug.Log("inside onPreviousGlobal");
    }

    void OnLatestImageGlobal()
    {
        Debug.Log("inside onLatestImageGlobal");
    }

    void OnFollowGlobal()
    {
        Debug.Log("inside onFollowGlobal");
    }

    void OnStopFollowingGlobal()
    {
        Debug.Log("inside onStopFollowingGlobal");
    }
}
