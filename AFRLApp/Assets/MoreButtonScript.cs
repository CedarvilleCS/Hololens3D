using UnityEngine;
using System.Collections;

public class MoreButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	

    public void OnSelect ()
    {
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        Quaternion ImagePaneRotation = ImagePaneCollection.transform.localRotation;
        Vector3 ImagePanePosition = ImagePaneCollection.transform.localPosition;
        int numImages = ImagePaneCollection.GetComponent<ImageReceiver>().NumRcvdImages;

        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        Vector3 OldGalleryScale = ImageGallery.GetComponent<ImageGalleryController>().OrigScale;
        GameObject ImageQueue = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
        Vector3 OldQueueScale = ImageQueue.GetComponent<ImageQueueController>().OrigScale;
        GameObject AnnotatedImage = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
        Vector3 OldAnnotatedImageScale = AnnotatedImage.GetComponent<AnnotatedImageController>().OrigScale;
        Debug.Log("About to Instantiate: OldScale is " + OldGalleryScale);
        
        ImagePanePosition.x = ImagePanePosition.x + 2;
        GameObject NewCollection = (GameObject)Instantiate(ImagePaneCollection, ImagePanePosition, ImagePaneRotation);
        NewCollection.GetComponent<ImageReceiver>().FirstInstance = false;
        NewCollection.GetComponent<ImageReceiver>().ResetNumRcvdImages = numImages;

        GameObject NewGallery = NewCollection.transform.Find("ImageGallery").gameObject;
        NewGallery.GetComponent<ImageGalleryController>().ResetScale = OldGalleryScale;
        GameObject NewQueue = NewCollection.transform.Find("ImageQueue").gameObject;
        NewQueue.GetComponent<ImageQueueController>().ResetScale = OldQueueScale;
        GameObject NewAnnotatedImage = NewCollection.transform.Find("AnnotatedImage").gameObject;
        NewAnnotatedImage.GetComponent<AnnotatedImageController>().ResetScale = OldAnnotatedImageScale;
        Debug.Log("Scale is now changed.");

        GameObject OldPlaceButton = ImagePaneCollection.transform.Find("PlaceButton").gameObject;
        OldPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(false);
        GameObject NewPlaceButton = NewCollection.transform.Find("PlaceButton").gameObject;
        NewPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(true);
    }
}