using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private GameObject ImagePaneCollection;
    private GameObject ImageGallery;
    private GameObject PlaceButton;
    private GameObject MoreButton;
    private GameObject ShowGalleryButton;
    // Use this for initialization
    void Start()
    {
        keywords.Add("Show Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;

            // Ensure that the user is gazing at a given object rather than into empty space

            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ShowGalleryButton = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().showGalleryWindow();
            }
        });

        keywords.Add("Close Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ShowGalleryButton = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        });

        keywords.Add("First Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
                ImageGallery.GetComponent<ImageGalleryController>().OnFirstImage();
            }
        });

        keywords.Add("Latest Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
                ImageGallery.GetComponent<ImageGalleryController>().OnSelectByIndex(0);
            }
        });

        keywords.Add("Next Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
                ImageGallery.GetComponent<ImageGalleryController>().OnNextImage();
            }
        });

        keywords.Add("Previous Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
                ImageGallery.GetComponent<ImageGalleryController>().OnPreviousImage();
            }
        });

        keywords.Add("Follow Me", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Param indicates that this is a "follow me" command, not a 
                // "stop following" command
                ImagePaneCollection = focusObject.transform.root.gameObject;
                PlaceButton = ImagePaneCollection.transform.Find("PlaceButton").gameObject;
                bool CmdToFollow = true;
                PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
            }
        });

        keywords.Add("Stop Following", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Param used as described in "follow me" handler above
                ImagePaneCollection = focusObject.transform.root.gameObject;
                PlaceButton = ImagePaneCollection.transform.Find("PlaceButton").gameObject;
                bool CmdToFollow = false;
                PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
            }
        });

        keywords.Add("New Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                MoreButton = ImagePaneCollection.transform.Find("MoreButton").gameObject;
                MoreButton.GetComponent<MoreButtonScript>().OnSelect();
            }
        });

        keywords.Add("Close Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Fill in once handler function created
            }
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
