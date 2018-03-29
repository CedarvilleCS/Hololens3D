using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSheetController : MonoBehaviour
{
    public TabSelector tabSelector;
    public GameObject helpSheet;
    private Vector3 starterScale;

    // Use this for initialization
    void Start()
    {
        starterScale = this.transform.localScale;
    }

    void OnSelect()
    {
        if (tabSelector.CurrentTabState() != TabSelector.TabState.HelpSheet)
        {
            tabSelector.SetCurrentState(TabSelector.TabState.HelpSheet);
        }
    }

    internal void Hide()
    {
        helpSheet.transform.localScale = new Vector3(0, 0, 0);
    }

    internal void Show()
    {
        helpSheet.transform.localScale = starterScale;
    }
}
