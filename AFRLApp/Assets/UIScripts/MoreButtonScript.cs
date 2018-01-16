using UnityEngine;
using System.Collections;

public class MoreButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	/// <summary>
    /// Simulates an air-tap on the MoreButton gameobject, which creates a 
    /// new Image Pane Collection window
    /// </summary>

    public void OnSelect ()
    {

        // Before initilizing new Image Pane Collection window, acquire
        // NumRcvdImages as well as the original, default scale of all
        // gameObjects that will be/could be hidden so that these values
        // can be passed into the new Image Pane Collection object; without
        // this step, original scale information could be lost once the new
        // window is created

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
        GameObject OldCloseButton = ImagePaneCollection.transform.Find("CloseButton").gameObject;
        Vector3 OldCloseButtonScale = OldCloseButton.GetComponent<CloseButtonScript>().OrigScale;
        
        // Initialize a new Image Pane Collection window, setting the necessary default scale variables
        // and otherwise from the previous Image Pane Collection window to preserve initial state

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
        GameObject NewCloseButton = NewCollection.transform.Find("CloseButton").gameObject;
        NewCloseButton.GetComponent<CloseButtonScript>().ResetScale = OldCloseButtonScale;

        // Whenever a new window is spawned, "Place" the old window and "Unplace" the new

        GameObject OldPlaceButton = ImagePaneCollection.transform.Find("PlaceButton").gameObject;
        OldPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(false);
        GameObject NewPlaceButton = NewCollection.transform.Find("PlaceButton").gameObject;
        NewPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(true);
    }
}