using UnityEngine;
using System.Collections;

public class CloseButtonScript : MonoBehaviour
{
    /// <summary>
    /// Simulates an air-tap on the CloseButton gameobject, which
    /// will delete and remove the currently focused
    /// gameobject's parent
    /// </summary>

    public void OnSelect()
    {
        Destroy(this.transform.root.gameObject);
    }
}