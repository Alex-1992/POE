using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(POEStatus))]
public class Buff : MonoBehaviour
{

    private float Duration;
    private StatusModifier m;

    private POEStatus S;
    private static int counter = 0;

    // Use this for initialization
    private void Start()
    {
        S = gameObject.GetComponent<POEStatus>();
        StartCoroutine(Tools.LimitedLifeComponent(Duration, this));
        m.ApplyTo(S);
        counter++;
        //Debug.Log(counter.ToString() + " - BUFFON - " + attachedGameObject.name + ",Key:" + Key + ",AddChange" + AddChange + ",MultiplierChange" + MultiplierChange + ",MoreChange" + MoreChange + ",FINAL:" + S.S[Key].GetInt());
    }

    private void OnDisable()
    {
        m.RemoveFrom(S);
        //Debug.Log(counter.ToString() + " - BUFFOFF - " + attachedGameObject.name + ",Key:" + Key + ",AddChange" + AddChange + ",MultiplierChange" + MultiplierChange + ",MoreChange" + MoreChange + ",FINAL:" + S.S[Key].GetInt());
    }

    //对S所在的gameObject增加一个持续Duration的BUFF组件，其效果为modifier
    public static Buff AddBuff(POEStatus S, float duration, StatusModifier modifier) {
        if (S != null)
        {
            Buff newbuff = S.gameObject.AddComponent<Buff>();
            newbuff.Duration = duration;
            newbuff.m = modifier;
            return newbuff;
        }
        else
        {
            return null;
        }
    }
}
