using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(POEStatus))]
public class PlayerController : MonoBehaviour
{
    //inspectors
    public BarController PlayerMPBarController;

    //prefabs
    public GameObject m_BulletPrefab;
    public Camera m_MainCamara;

    //私有
    private bool m_IsMoving = false;
    private bool m_CanAttack = false;
    private float m_AttackCurrentCoolDown = 0f;
    private float m_AttackRange;
    private float m_TargetDistance;
    private List<GameObject> m_TargetList = new List<GameObject>(); //储存可以进攻的单位的列表

    //components
    private Rigidbody m_rgbd;
    private ScrollCircle m_JoyStick;
    private HPControllerPlayer m_HPControllerPlayer;
    private POEStatus S;

    //Debug
    private int counter = 0;
    private float lastrealtime = 0;
    //private RaycastHit rch;

    public bool GetIsMoving()
    {
        return m_IsMoving;
    }

    private void Awake()
    {
        //设置POEStatics
        if (POEStatics.Player == null) POEStatics.Player = gameObject;
        if (POEStatics.PlayerController == null) POEStatics.PlayerController = this;
        if (POEStatics.PlayerMeshRenderer == null) POEStatics.PlayerMeshRenderer = POEStatics.Player.GetComponent<MeshRenderer>();

        //获取组件
        m_rgbd = gameObject.GetComponent<Rigidbody>();
        m_HPControllerPlayer = gameObject.GetComponent<HPControllerPlayer>();
        S = gameObject.GetComponent<POEStatus>();
        m_JoyStick = GameObject.Find("JoystickBackGround").GetComponent<ScrollCircle>();
    }

    void Start()
    {
        //初始化私有变量
        m_IsMoving = false;
        m_CanAttack = true;
        m_AttackCurrentCoolDown = 0f;
    }

    private void FixedUpdatePrivates()
    {
        m_AttackCurrentCoolDown += Time.fixedDeltaTime;
        m_AttackRange = S.S[K.BulletSpeed].Get() * S.S[K.BulletLifeTime].Get();
        if (m_TargetList.Count != 0)
        {
            m_TargetDistance = Vector3.Distance(gameObject.transform.position, m_TargetList[0].transform.position);
        }
        else
        {
            m_TargetDistance = m_AttackRange;
        }
    }

    void FixedUpdate()
    {
        //更新属性
        FixedUpdatePrivates();
        //移动
        CheckMoving();
        //索敌
        Searching();
        //攻击
        Attacking();
    }
    
    void CheckMoving()
    {
        m_IsMoving = false;
        m_CanAttack = true;

        if (m_JoyStick.v2 != Vector2.zero)
        {
            m_rgbd.velocity = new Vector3(m_JoyStick.v2.x, 0, m_JoyStick.v2.y) * S.S[K.MoveSpeed].Get();
            //Debug.Log(m_rgbd.velocity);
            m_IsMoving = true;
            m_CanAttack = false;
        }
    }

    //索敌方法
    void Searching()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in temp)
        {
            float tempdistance = Vector3.Distance(gameObject.transform.position, item.transform.position);
            //如果目标列表中不包括目标单位，且距离在射程内，加入列表
            if (m_TargetList.Contains(item) == false && tempdistance <= m_AttackRange)
            {
                m_TargetList.Add(item);
            }
            //如果目标里表中包括目标单位，但距离太远，移出列表
            else if (m_TargetList.Contains(item) == true && tempdistance > m_AttackRange)
            {
                m_TargetList.Remove(item);
            }
        }
        //对目标单位列表进行排序
        if (m_TargetList.Count > 0)
        {
            m_TargetList.Sort(SortEnemyListByDistanceFromPlayer);
        }
    }

    //用于为索敌列表进行排序的方法
    private int SortEnemyListByDistanceFromPlayer(GameObject a, GameObject b)
    {
        float distancea = Vector3.Distance(gameObject.transform.position, a.transform.position);
        float distanceb = Vector3.Distance(gameObject.transform.position, b.transform.position);
        if (distancea > distanceb) //这边的比较可以是任意的类型，只要是你可以比较的东西，比如student类中的年龄age stu1.age > stu2.age
        {
            return 1;
        }
        else if (distancea < distanceb)
        {
            return -1;
        }
        return 0;
    }

    //攻击方法
    void Attacking()
    {
        //所有判断都通过了，可以攻击
        if (AttackCheck())
        {
            //根据距离重新计算arc，距离越远arc越小，且整个子弹扇面不会超过180度
            float temparc = S.S[K.BulletArc].Get() * 5f / m_TargetDistance;
            if (temparc * (S.S[K.BulletCount].GetInt() - 1) > 180f)
            {
                temparc = 180f / (S.S[K.BulletCount].GetInt() - 1);
            }
            //Debug.LogFormat("temparc:{0} , m_TargetDistance:{1}", temparc, m_TargetDistance); //修正一过房间怪物还没刷出来的时候就尝试攻击会报错的bug的时候加的log，后来发现是因为m_TargetDistance作为被除数，当没有怪物的时候值为0导致的bug
            //发射飞弹
            for (int i = 0; i < S.S[K.BulletCount].GetInt(); i++)
            {
                GameObject _newbullet = GameObject.Instantiate(m_BulletPrefab, gameObject.transform.position, Quaternion.identity);
                _newbullet.transform.LookAt(m_TargetList[0].transform);
                _newbullet.transform.Rotate(0, -(S.S[K.BulletCount].GetInt() - 1) / 2 * temparc, 0);
                _newbullet.transform.Rotate(0, i * temparc, 0);
                _newbullet.GetComponent<Rigidbody>().velocity = _newbullet.transform.forward * S.S[K.BulletSpeed].Get();
                _newbullet.GetComponent<BulletController>().SourceUnit = gameObject;
            }
            //CD清零，扣蓝
            m_AttackCurrentCoolDown = 0f;
            m_HPControllerPlayer.LoseMP(S.S[K.BulletMPCost].Get());

            //更新UI
            PlayerMPBarController.SetBarValue(m_HPControllerPlayer.GetCurrentMP() / S.S[K.MaxMP].Get());

            //测试Buff
            //Buff.AddBuff(S, 3f, new StatusModifier(K.BulletCount, 1, 0, 0));
        }
    }

    //攻击前判断是否可以攻击的方法，返回true则可以攻击
    private bool AttackCheck()
    {
        //1 判断由于移动等其他原因导致的是否可以攻击
        if (m_CanAttack == false) return false;
        //2 判断列表敌人有没有敌人
        if (m_TargetList.Count == 0) return false;
        //3 判断剩余法力够不够
        if (m_HPControllerPlayer.GetCurrentMP() < S.S[K.BulletMPCost].Get()) return false;
        //4 判断CD是不是已经清空了
        if (m_AttackCurrentCoolDown < 1f / S.S[K.AttackSpeed].Get()) { return false; }
        //5 判断射程够不够
        if (m_TargetDistance > m_AttackRange) return false;
        //6 判断目标是否在视野中
        //Vector3 p1 = gameObject.transform.position;
        //Vector3 direction = m_TargetList[0].transform.position - gameObject.transform.position;
        //Debug.DrawRay(p1, direction, Color.red);
        //Physics.Raycast(p1, direction, out rch, m_AttackRange);
        //if (rch.collider.gameObject != m_TargetList[0]) return false; //如果看到的不是要攻击的单位

        //所有判断都通过，返回true
        //Debug.Log("Counter:" + counter.ToString() + ",当前冷却：" + m_AttackCurrentCoolDown.ToString() + ",攻击间隔：" + 1f / S.S[K.AttackSpeed].Get() + ",上次攻击后过了：" + (Time.realtimeSinceStartup - lastrealtime).ToString() + ",目标距离：" + m_TargetDistance + ",射程：" + m_AttackRange);
        lastrealtime = Time.realtimeSinceStartup;
        counter++;
        return true;
    }

    //其他单位回调的删除单位的方法
    public bool RemoveTargetFromTargetlist(GameObject targetinstance)
    {
        if (m_TargetList.Contains(targetinstance))
        {
            m_TargetList.Remove(targetinstance);
            return true;
        }
        else
        {
            return false;
        }
    }
}