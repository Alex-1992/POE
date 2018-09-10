using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BulletController : MonoBehaviour
{
    public GameObject SourceUnit;

    private List<GameObject> DamagedTargetList = new List<GameObject>();    //已经打过了的敌人的列表
    private POEStatus S;
    private MeshRenderer M;
    private int DamageCount; //可以伤害不同敌人的总次数

    //debug
    private bool IsDebug = false;

    void Start()
    {
        S = SourceUnit.GetComponent<POEStatus>();
        M = gameObject.GetComponent<MeshRenderer>();

        DamageCount = S.S[K.BulletDamageCount].GetInt();
        StartCoroutine(Tools.LimitedLife(S.S[K.BulletLifeTime].Get(), gameObject));

        ChangeColorByDamage();
        Tools.COUNTIT(gameObject);
    }

    private void ChangeColorByDamage() {
        float total = S.S[K.BulletPhysicalDamage].Get() + S.S[K.BulletFireDamage].Get() + S.S[K.BulletColdDamage].Get() + S.S[K.BulletLightinglDamage].Get();
        float r = S.S[K.BulletFireDamage].Get() / total;
        float g = S.S[K.BulletColdDamage].Get() / total;
        float b = S.S[K.BulletLightinglDamage].Get() / total;
        M.material.color = new Color(r, g, b);
        if(IsDebug) Debug.LogFormat("{0}", M.material.color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(POEStatics.Const.EnemyTag) && DamagedTargetList.Contains(other.gameObject) == false)
        {
            DamagedTargetList.Add(other.gameObject);
            other.gameObject.GetComponent<HPControllerEnemy>().TakeDamage(POEStatics.PlayerPOEStatus);
            DamageCount -= 1;
            if (DamageCount <= 0) { Destroy(gameObject); }
        }
        //撞到墙就摧毁
        if (other.gameObject.CompareTag(POEStatics.Const.WallTag))
        {
            if (IsDebug) Debug.Log(gameObject.name + " - HitWall");
            Destroy(gameObject);
        }
    }
}
