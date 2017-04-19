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

    // Use this for initialization
    void Start()
    {
        keywords.Add("Open Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;

            // Ensure that the user is gazing at a given object rather than into empty space

            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnGalleryOpenHandler();
            }
        });

        keywords.Add("Close Gallery", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnGalleryCloseHandler();
            }
        });

        keywords.Add("First Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnFirstImageHandler();
            }
        });

        keywords.Add("Latest Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnLatestImageHandler();
            }
        });

        keywords.Add("Next Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnNextImageHandler();
            }
        });

        keywords.Add("Previous Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnPreviousImageHandler();
            }
        });

        keywords.Add("Unlock Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnUnlockWindowHandler();
            }
        });

        keywords.Add("Lock Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Param used as described in "unlock window" handler above
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnLockWindowHandler();

            }
        });

        keywords.Add("New Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnNewWindowHandler();
            }
        });

        keywords.Add("Close Window", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                ImagePaneCollection = focusObject.transform.root.gameObject;
                ImagePaneCollection.GetComponent<VoiceCommandHandler>().OnCloseWindowHandler();
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
