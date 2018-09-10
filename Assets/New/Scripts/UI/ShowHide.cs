using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ShowHide : MonoBehaviour {

    private bool IsShow = true;
    private CanvasGroup CG;

	// Use this for initialization
	void Start () {
        CG = GetComponent<CanvasGroup>();
    }

    public void ShowandHide() {

        if (IsShow)
        {
            //隐藏
            CG.alpha = 0;
            CG.interactable = false;
            CG.blocksRaycasts = false;
            IsShow = false;
        }
        else
        {
            //显示
            CG.alpha = 1;
            CG.interactable = true;
            CG.blocksRaycasts = true;
            IsShow = true;
        }
    }
}
