using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    GameObject ImagePaneCollection;

    // Use this for initialization
    void Start()
    {

        Debug.Log("Inside SpeechManager's Start function");

        keywords.Add("Show Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                GameObject ShowGalleryButton = GameObject.Find("ShowGalleryButton");
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().showGalleryWindow();
            }
        });

        keywords.Add("Close Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                GameObject ShowGalleryButton = GameObject.Find("ShowGalleryButton");
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        });

        // This is redundant with the select command, just here for testing; remove 
        // once testing is finished

        keywords.Add("Swap Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnSelect");
            }
        });

        keywords.Add("Next Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add actual OnNextImage function

                GameObject ImageGallery = GameObject.Find("ImageGallery");
                Debug.Log("Found the Image Gallery: " + ImageGallery);


                ImageGallery.GetComponent<ImageGalleryController>().OnNextImage();
            }
        });

        keywords.Add("Previous Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add actual OnNextImage function

                focusObject.SendMessageUpwards("OnPreviousImageGlobal");
            }
        });

        keywords.Add("Start Following", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add actual OnFollow function

                this.SendMessageUpwards("OnFollowGlobal");
            }
        });

        keywords.Add("Stop Following", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add OnStopFollowing function

                this.SendMessageUpwards("OnStopFollowingGlobal");
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
