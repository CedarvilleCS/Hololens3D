using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;

    void Start()
    {
        HLNetwork.ObjectReceiver objr = new HLNetwork.ObjectReceiver();
        objr.JpegReceived += OnJpegReceived;
        System.Diagnostics.Debug.WriteLine("DummyCommand finished Start() funciton");
    }
    /*
    // Called by GazeGestureManager when the user performs a Select gesture

    // Currently, this pulls an image from the internet and assigns it to the floating black pane when the user performs a finger-tap on the floating black pane.
    // This was to prove that we could dynamically assign an image (that wasn't known by the Unity editor) to the image pane
    // Areas to change:
    // 1. This event should be called when an image is received, not when the user finger-taps on the screen
    // 2. The picture put on the image pane should be the one received from the Surface app, not a random imgur picture
    // 3. We need to figure out how to make the image not show up upside down when it is put on the image-pane
    IEnumerator OnSelect()
    {

        var url = "http://i.imgur.com/g3D5jNz.jpg";
        WWW www = new WWW(url);
        yield return www; //the yield return is to make the async web request work; it's irrelevant once we get images from the app instead of imgur.com
        var renderer = this.gameObject.GetComponent<Renderer>();
        // The way we to put an image on the pane (as far as we can tell) is to overwrite the mainTexture property of the pane's Renderer object
        renderer.material.mainTexture = www.texture; 
       
    }*/

    void Update()
    {
        if (_newImagePresent)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);
            var renderer = this.gameObject.GetComponent<Renderer>();
            renderer.material.mainTexture = tex;
            renderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }
}