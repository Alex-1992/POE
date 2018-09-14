using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{

    private Slider myslider;

    void Start()
    {
        myslider = GetComponentInChildren<Slider>();
    }

    public void SetBarValue(float value)
    {
        if (value > 1)
        {
            Debug.LogErrorFormat("【{0} - {1}】：传入的值不应该大于1！", this.GetType(), gameObject.name);
            return;
        }
        if (value < 0)
        {
            Debug.LogErrorFormat("【{0} - {1}】：传入的值不应该小于0！", this.GetType(), gameObject.name);
            return;
        }
        myslider.value = value;
    }
}
