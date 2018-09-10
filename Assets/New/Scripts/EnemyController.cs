using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
    //public
    public Room myroom; //记录该怪物所处于的房间

    //private
    //private float OriginAngularSpeed;
    //private float OriginAccelration;
    private bool IsTouchingPlayer;

    //自己的组件
    private NavMeshAgent NavMeshAgent;
    private MeshRenderer MeshRender;
    private Color origincolor;
    private POEStatus MYS;
    private Rigidbody RGBD;

    //玩家的组件
    private POEStatus S;

    private void Awake()
    {
        //自己的组件
        NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        MeshRender = gameObject.GetComponent<MeshRenderer>();
        MYS = gameObject.GetComponent<POEStatus>();
        origincolor = MeshRender.material.color;
        RGBD = gameObject.GetComponent<Rigidbody>();

        //玩家组件
        S = POEStatics.PlayerPOEStatus;
    }

    void Start()
    {
        //其他
        Tools.COUNTIT(gameObject);
    }

    void Update()
    {
        if (NavMeshAgent != null && NavMeshAgent.isActiveAndEnabled == true && IsTouchingPlayer == false)
        {
            NavMeshAgent.SetDestination(POEStatics.Player.transform.position);
        }
        RGBD.velocity = Vector3.zero;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(POEStatics.Const.PlayerTag))
        {
            POEStatics.PlayerMeshRenderer.material.color = Color.red;
            MeshRender.material.color = Color.red;
            IsTouchingPlayer = true;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(POEStatics.Const.PlayerTag))
        {
            POEStatics.PlayerHPController.TakeDamage(MYS);
            POEStatics.PlayerMeshRenderer.material.color = Color.red;
            IsTouchingPlayer = true;
            if (NavMeshAgent != null && NavMeshAgent.isActiveAndEnabled == true)
            {
                NavMeshAgent.SetDestination(gameObject.transform.position);
            }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(POEStatics.Const.PlayerTag))
        {
            MeshRender.material.color = origincolor;
            POEStatics.PlayerMeshRenderer.material.color = POEStatics.Const.PlayerMeshRendererOriginColor;
            IsTouchingPlayer = false;
        }
    }

    private void OnDisable()
    {
        POEStatics.Player.SendMessage("RemoveTargetFromTargetlist", gameObject, SendMessageOptions.RequireReceiver);
        if (POEStatics.ISREBORN == false)
        {
            POEStatics.Player.SendMessage("ExpGain", MYS.S[K.ExpGainOnKill].Get(), SendMessageOptions.RequireReceiver);
        }
        myroom.MobKilled(gameObject);
        if (myroom.CheckCleared())
        {
            if (POEStatics.ISREBORN == false)
            {
                POEStatics.CardManager.ShowCardsToPick(S.S[K.PlayerLevelUpCardCount].GetInt());
                POEStatics.MobPopulation += 2f;
                POEStatics.MobLevel += 1;
            }
            myroom.CreateNextRoom();
        }
    }

    private void OnDestroy()
    {
        POEStatics.PlayerMeshRenderer.material.color = POEStatics.Const.PlayerMeshRendererOriginColor;  //重设玩家的颜色，防止由于怪物碰撞中的时候被打死导致玩家颜色无法恢复
    }
}
