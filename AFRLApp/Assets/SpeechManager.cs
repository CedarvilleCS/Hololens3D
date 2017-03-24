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

        GameObject ImageGallery      = GameObject.Find("ImageGallery");
        GameObject PlaceButton       = GameObject.Find("PlaceButton");
        GameObject ShowGalleryButton = GameObject.Find("ShowGalleryButton");

        keywords.Add("Show Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().showGalleryWindow();
            }
        });

        keywords.Add("Close Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        });

        keywords.Add("First Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImageGallery.GetComponent<ImageGalleryController>().OnFirstImage();
            }
        });

        keywords.Add("Latest Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImageGallery.GetComponent<ImageGalleryController>().OnSelectByIndex(0);
            }
        });

        keywords.Add("Next Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImageGallery.GetComponent<ImageGalleryController>().OnNextImage();
            }
        });

        keywords.Add("Previous Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImageGallery.GetComponent<ImageGalleryController>().OnPreviousImage();
            }
        });

        keywords.Add("Follow Me", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                bool CmdToFollow = true;
                PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
            }
        });

        keywords.Add("Stop Following", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                bool CmdToFollow = false;
                PlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(CmdToFollow);
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
