using UnityEngine;
using System.Collections;

public class ImageQueueController : MonoBehaviour
{
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    public GameObject[] queueImagePanes { get; private set; }
    public Renderer[] queueImageRenderers { get; private set; }

    // Use this for initialization
    void Start()
    {
        Debug.Log("ImageQueueController start - current scale: " + this.transform.localScale);
        OrigScale = this.transform.localScale;   
        int numQueuePanes = this.transform.childCount;
        queueImagePanes = new GameObject[numQueuePanes];
        queueImageRenderers = new Renderer[numQueuePanes];
        Debug.Log("num of queue panes is " + numQueuePanes);
        for (int i = 0; i < queueImagePanes.Length; i++)
        {
            queueImagePanes[i] = this.transform.GetChild(i).gameObject;
            queueImagePanes[i].GetComponent<QueueImageSwapper>().ImageId = i;
            queueImageRenderers[i] = queueImagePanes[i].GetComponent<Renderer>();
            queueImageRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }

        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        bool IsFirstInstance = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;

        Debug.Log("ImageReceiver.InstanceNum is " + IsFirstInstance);

        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
        }
        showWindow();
    }

    public void updateCurrViewedQueueIndex(int NextGalleryIndex)
   { 
        Debug.Log("Inside ImageQueueController.updateCurrViewedQueuePane");

        GameObject ImagePaneCollection = this.transform.root.gameObject;
        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        ImageGallery.GetComponent<ImageGalleryController>().OnSelectByIndex(NextGalleryIndex);
    }

    public void RcvNewImage(Texture2D ImageTexture, int numRcvdImages)
    {
        Debug.Log("Inside ImageQueueController.RcvdNewImage");
        if (numRcvdImages > 1)
        {
            // shift the image queue to the right

            int queueSize = queueImagePanes.Length - 1;
            for (int i = queueSize; i > 0; i--)
            {
                Renderer prevObjRenderer = queueImageRenderers[i - 1];
                Renderer currObjRenderer = queueImageRenderers[i];
                Texture prevObjTexture = prevObjRenderer.material.mainTexture;
                currObjRenderer.material.mainTexture = prevObjTexture;
            }

            // Load new image

            Renderer queueRenderer = queueImageRenderers[0];
            queueRenderer.material.mainTexture = ImageTexture;
        }
        else
        {
            // Load image, but do not shift (first image rcv'd, so nothing to shift)

            Debug.Log("Inside ImageQueueController.RcvdNewImage: Loading first texture");

            Renderer queueRenderer = queueImageRenderers[0];
            queueRenderer.material.mainTexture = ImageTexture;
        }
    }
    public void hideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void showWindow()
    {
        this.transform.localScale = OrigScale;
    }
}
