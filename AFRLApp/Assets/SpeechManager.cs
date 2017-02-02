using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("Open Gallery", () =>
        {
            // Call the OnReset method on every descendant object.

            // Add correct function here, not OnGallerySelect
            this.BroadcastMessage("OnGallerySelect");
        });

        keywords.Add("Close Gallery", () =>
        {
            // Call the OnReset method on every descendant object.

            // Add correct function here, not OnGallerySelect
            this.BroadcastMessage("OnGallerySelect");
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

                focusObject.SendMessage("OnNextImage");
            }
        });

        keywords.Add("Previous Image", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add actual OnNextImage function

                focusObject.SendMessage("OnPreviousImage");
            }
        });

        keywords.Add("Follow Me", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add actual OnFollow function
                
                this.BroadcastMessage("OnFollow");
            }
        });

        keywords.Add("Stop Following", () =>
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Add OnStopFollowing function
                
                this.BroadcastMessage("OnStopFollowing");
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
