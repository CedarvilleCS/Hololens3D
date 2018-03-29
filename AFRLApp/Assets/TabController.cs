using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour {
    public TabSelector.TabState tabType;
    public TabSelector tabSelector;

	void OnSelect()
    {
        tabSelector.SetCurrentState(tabType);
    }
}
