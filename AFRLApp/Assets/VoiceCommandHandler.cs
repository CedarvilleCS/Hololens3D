using UnityEngine;
using System.Collections;

public class VoiceCommandHandler : MonoBehaviour
{
    public void OnGalleryOpenHandler()
    {
        GameObject ShowGalleryButton = this.transform.Find("ShowGalleryButton").gameObject;
        ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().ShowGalleryWindow();
    }
    public void OnGalleryCloseHandler()
    {
        GameObject ShowGalleryButton = this.transform.Find("ShowGalleryButton").gameObject;
        ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().HideGalleryWindow();
    }
    public void OnFirstImageHandler()
    {
        GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
        ImageGallery.GetComponent<ImageGalleryController>().OnFirstImage();
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

    public void OnUnlockWindowHandler()
    {
        GameObject PlaceButton = this.transform.Find("PlaceButton").gameObject;
        bool CmdToUnlock = true;
        PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToUnlock);
    }

    public void OnLockWindowHandler()
    {
        GameObject PlaceButton = this.transform.Find("PlaceButton").gameObject;
        bool CmdToUnlock = false;
        PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToUnlock);
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
