using UnityEngine;
using System.Collections;

public class VoiceCommandHandler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    public void OnGalleryOpenHandler()
    {
        Debug.Log("inside OnGalleryOpenGlobal");
        GameObject ShowGalleryButton = this.transform.Find("ShowGalleryButton").gameObject;
        ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().showGalleryWindow();
    }
    public void OnGalleryCloseHandler()
    {
        Debug.Log("inside OnGalleryOpenGlobal");
        GameObject ShowGalleryButton = this.transform.Find("ShowGalleryButton").gameObject;
        ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
    }
    public void OnFirstImageHandler()
    {
        Debug.Log("Inside OnFirstImageHandler()");
        GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
        Debug.Log("Still Inside OnFirstImageHandler()");
        ImageGallery.GetComponent<ImageGalleryController>().OnFirstImage();
        Debug.Log("End of OnFirstImageHandler()");
    }

    public void OnNextImageHandler()
    {
        GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
        ImageGallery.GetComponent<ImageGalleryController>().OnNextImage();
    }

    public void OnPreviousImageHandler()
    {
        GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
        ImageGallery.GetComponent<ImageGalleryController>().OnPreviousImage();
    }

    public void OnLatestImageHandler()
    {
        GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
        ImageGallery.GetComponent<ImageGalleryController>().OnSelectByIndex(0);
    }

    public void OnFollowHandler(bool CmdToFollow)
    {
        GameObject PlaceButton = this.transform.Find("PlaceButton").gameObject;
        PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
    }

    public void OnStopFollowingHandler()
    {
        GameObject PlaceButton = this.transform.Find("PlaceButton").gameObject;
        bool CmdToFollow = false;
        PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
    }

    public void OnNewWindowHandler()
    {
        GameObject MoreButton = this.transform.Find("MoreButton").gameObject;
        MoreButton.GetComponent<MoreButtonScript>().OnSelect();
    }

    public void OnCloseWindowHandler()
    {
        GameObject CloseButton = this.transform.Find("CloseButton").gameObject;
        CloseButton.GetComponent<CloseButtonScript>().OnSelect();
    }
}
