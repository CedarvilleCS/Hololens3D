using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    private Renderer[] queueImageRenderers;
    private Renderer[] galleryImageRenderers;
    private int numRcvdImages;

    void Start()
    {
        HLNetwork.ObjectReceiver objr = new HLNetwork.ObjectReceiver();
        objr.JpegReceived += OnJpegReceived;
        numRcvdImages = 0;

        // Store image renderers for texture-application use later

        int numQueueImages = this.gameObject.transform.GetChild(0).childCount;
        int numGalleryImages = this.gameObject.transform.GetChild(1).childCount;
        queueImageRenderers = new Renderer[numQueueImages];
        galleryImageRenderers = new Renderer[numGalleryImages];

        // Textures are applied to objects mirrored by default, so scale main textures appropriately

        for (int i = 0; i < queueImageRenderers.Length; i++)
        {
            var queueImgObj = this.gameObject.transform.GetChild(0).GetChild(i);
            queueImageRenderers[i] = queueImgObj.gameObject.GetComponent<Renderer>();
            queueImageRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }

        for (int i = 0; i < galleryImageRenderers.Length; i++)
        {
            var galleryImgObj = this.gameObject.transform.GetChild(1).GetChild(i);
            galleryImageRenderers[i] = galleryImgObj.gameObject.GetComponent<Renderer>();
            galleryImageRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }

        var mainImageRenderer = this.gameObject.GetComponent<Renderer>();
        mainImageRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
    }


    void Update()
    {
        if (_newImagePresent)
        {
            numRcvdImages++;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);

            // After first image rcv'd, shift queue/gallery before loading the image

            if (numRcvdImages > 1)
            {
                ShiftImages(numRcvdImages);
                var queueRenderer = queueImageRenderers[0];
                queueRenderer.material.mainTexture = tex;
                var galleryRenderer = galleryImageRenderers[0];
                galleryRenderer.material.mainTexture = tex;
            }
            else
            {
                // Load image, but do not shift (first image rcv'd, so nothing to shift)

                var renderer = this.gameObject.GetComponent<Renderer>();
                renderer.material.mainTexture = tex;
                var queueRenderer = queueImageRenderers[0];
                queueRenderer.material.mainTexture = tex;
                var galleryRenderer = galleryImageRenderers[0];
                galleryRenderer.material.mainTexture = tex;
            }

            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }

    void ShiftImages(int numRcvdImages)
    {
        // Determine minimum images to shift to avoid unnecesary operations

        var queueSize = queueImageRenderers.Length - 1;
        var gallerySize = galleryImageRenderers.Length - 1;

        if (numRcvdImages < queueImageRenderers.Length)
        {
            queueSize = numRcvdImages;
        }
        if (numRcvdImages < gallerySize)
        {
            gallerySize = numRcvdImages;
        }

        // shift the image queue to the right

        for (int i = queueSize; i > 0; i--)
        {
            var prevObjRenderer = queueImageRenderers[i - 1];
            var currObjRenderer = queueImageRenderers[i];

            var prevObjTexture = prevObjRenderer.material.mainTexture;
            var currObjTexture = currObjRenderer.material.mainTexture;

            currObjRenderer.material.mainTexture = prevObjTexture;
        }

        // shift image gallery to the right

        for (int i = gallerySize - 1; i > 0; i--)
        {
            var prevObjRenderer = galleryImageRenderers[i - 1];
            var currObjRenderer = galleryImageRenderers[i];

            var prevObjTexture = prevObjRenderer.material.mainTexture;
            currObjRenderer.material.mainTexture = prevObjTexture;
        }
    }
}