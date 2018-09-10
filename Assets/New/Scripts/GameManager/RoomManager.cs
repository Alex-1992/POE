using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class RoomManager : MonoBehaviour
{
    //inspector
    public GameObject RoomPrefab;
    public GameObject MobPrefabNormal;
    public GameObject MobPrefabSmall;
    public GameObject MobPrefabLarge;

    private void Start()
    {
        //初始化Statics
        if (POEStatics.RoomManager == null) POEStatics.RoomManager = this;

        //创建初始房间
        Room.MobGroup mg = new Room.MobGroup(MobPrefabNormal, MobPrefabSmall, MobPrefabLarge, POEStatics.MobPopulation, new Vector3(5, 0, 5), MobGroupType.AllLarge);
        Room currentroom = new Room(0, RoomPrefab, Vector3.zero, mg);
        currentroom.SpawnMobs();
    }

    private void OnDisable()
    {
        Room.DestroyAll();
    }
}

public class Room
{
    private int roomid;
    public GameObject roomgo;
    public Direction directionToNextRoom;
    public Vector3 nextroomposition;
    private RoomController rc;
    private NavMeshSurface nms;
    private BoxCollider bc;
    private static GameObject StaticRoompPefab = null;
    private MobGroup mg = null;
    private bool isLeftOpen = false;
    private bool isRightOpen = false;
    private bool isUpOpen = false;
    private bool isDownOpen = false;
    private bool isCleared = false;

    //用于管理所有room对象的私有字典
    private static Dictionary<int, Room> roomdic = new Dictionary<int, Room>();

    //按ID查找Room实例
    public static Room FindRoomByID(int id)
    {
        return roomdic[id];
    }

    public int GetRoomID()
    {
        return this.roomid;
    }

    //场景关闭前防止泄露
    public static void DestroyAll()
    {
        //for (int i = 0; i < roomdic.Count; i++)
        //{
        //    roomdic.Remove(i);
        //    if (roomdic[i].roomgo != null)
        //    {
        //        GameObject.Destroy(roomdic[i].roomgo);
        //    }
        //}
        roomdic.Clear();
        //roomdic = null;
    }

    //构造函数，只有第一个房子用得到，后面用CreateNextRoom就行了
    public Room(int id, GameObject roomprefab, Vector3 p, MobGroup mobgroup)
    {
        if (StaticRoompPefab == null)
        {
            StaticRoompPefab = roomprefab;
        }
        this.mg = mobgroup;
        roomid = id;
        this.roomgo = GameObject.Instantiate(StaticRoompPefab, p, Quaternion.identity);  //第一次调用的时候，nextroomposition为Vector3.Zero，后面的房间才需要计算
        roomgo.name += " - " + roomid.ToString();
        rc = roomgo.GetComponent<RoomController>();
        nms = rc.Floor.GetComponent<NavMeshSurface>();
        bc = roomgo.GetComponent<BoxCollider>();
        bc.enabled = true;
        rc.thisroomid = id;
        this.CloseAll();
        nms.BuildNavMesh();
        nms.UpdateNavMesh(nms.navMeshData);
        this.directionToNextRoom = CalculateNextRoomDirection();
        this.nextroomposition = CalculateNextRoomPosition();
        roomdic.Add(this.roomid, this);
    }

    public void Destroy()
    {
        GameObject.Destroy(this.roomgo);
        roomdic.Remove(this.roomid);
    }

    //打开自己的BoxCollider，这个是RoomController销毁了老房间之后为新房间调用的方法
    public void SetBCEnable(bool b)
    {
        bc.enabled = b;
    }

    //创建下一个房间，返回房间的ID，并且要关闭下个房间的BoxCollider，防止玩家在房间接缝处来回跑会出bug，上个房间销毁后才会激活下个房间的BoxCollider（在RoomController中处理）
    public int CreateNextRoom()
    {
        MobGroup newmg = new MobGroup(mg.GetMobPrefabNormal(), mg.GetMobPrefabSmall(), mg.GetMobPrefabLarge(), POEStatics.MobPopulation, this.nextroomposition, Tools.GetRandomMobGroupType());
        Room nextroom = new Room(this.roomid + 1, StaticRoompPefab, this.nextroomposition, newmg);
        this.Open(this.directionToNextRoom, true);
        nextroom.Open(Tools.Opposite(this.directionToNextRoom), true);
        nextroom.bc.enabled = false;
        return nextroom.roomid;
    }

    //关闭房间所有门
    public void CloseAll()
    {
        Open(Direction.LEFT, false);
        Open(Direction.RIGHT, false);
        Open(Direction.UP, false);
        Open(Direction.DOWN, false);
    }

    //刷怪，刷怪点由mg的p决定，在mg的构造函数中赋值
    public void SpawnMobs()
    {
        mg.Spawn(this.roomid);
    }

    //怪物被击杀时的回调
    public void MobKilled(GameObject mobinstance)
    {
        if (mg.moblist.Contains(mobinstance))
        {
            mg.moblist.Remove(mobinstance);
        }
    }

    //检查是否还有剩余怪物
    public bool CheckCleared()
    {
        if (mg.moblist.Count == 0)
        {
            isCleared = true;
        }
        else
        {
            isCleared = false;
        }
        return isCleared;
    }

    //计算通往下个房间的方向
    private Direction CalculateNextRoomDirection()
    {
        Direction rtn = Direction.NULL;
        List<Direction> rndpool = new List<Direction>() { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
        if (this.roomid <= 0)
        {
            rtn = rndpool[UnityEngine.Random.Range(0, rndpool.Count)];
        }
        else
        {
            Direction remove = FindRoomByID(this.roomid - 1).directionToNextRoom;
            remove = Tools.Opposite(remove);
            rndpool.Remove(remove);
        }
        rtn = rndpool[UnityEngine.Random.Range(0, rndpool.Count)];
        this.directionToNextRoom = rtn;
        return this.directionToNextRoom;
    }

    //打开某个方向的墙
    public void Open(Direction direction, bool isopen)
    {
        if (rc == null)
        {
            rc = roomgo.GetComponent<RoomController>();
        }
        switch (direction)
        {
            case Direction.LEFT:
                rc.LeftOpen.SetActive(isopen);
                rc.LeftClose.SetActive(!isopen);
                isLeftOpen = isopen;
                break;
            case Direction.RIGHT:
                rc.RightOpen.SetActive(isopen);
                rc.RightClose.SetActive(!isopen);
                isRightOpen = isopen;
                break;
            case Direction.UP:
                rc.UpOpen.SetActive(isopen);
                rc.UpClose.SetActive(!isopen);
                isUpOpen = isopen;
                break;
            case Direction.DOWN:
                rc.DownOpen.SetActive(isopen);
                rc.DownClose.SetActive(!isopen);
                isDownOpen = isopen;
                break;
            default:
                break;
        }
    }

    //计算下个房间的位置
    private Vector3 CalculateNextRoomPosition()
    {
        Vector3 rtn = roomgo.transform.position;
        float x = rc.Floor.transform.lossyScale.x * 10; //乘10是因为roomgo用的是Plane，用别的就不行了，下一行也是相同道理
        float y = rc.Floor.transform.lossyScale.z * 10;
        switch (directionToNextRoom)
        {
            case Direction.LEFT:
                rtn += new Vector3(-x, 0, 0);
                break;
            case Direction.RIGHT:
                rtn += new Vector3(x, 0, 0);
                break;
            case Direction.UP:
                rtn += new Vector3(0, 0, y);
                break;
            case Direction.DOWN:
                rtn += new Vector3(0, 0, -y);
                break;
        }
        this.nextroomposition = rtn;
        return rtn;
    }

    //怪物组的类
    public class MobGroup
    {
        //参数
        private GameObject MobPrefabNormal;
        private GameObject MobPrefabSmall;
        private GameObject MobPrefabLarge;
        private float MaxMobPopulation;
        private MobGroupType m_MobGroupType;
        private Vector3 p;

        //privates
        private float MobNormalPopulation;
        private float MobSmallPopulation;
        private float MobLargelPopulation;
        private float CurrentMobPopulation;
        public List<GameObject> moblist = new List<GameObject>();
        private bool CanSpawnNormalMob;
        private bool CanSpawnSmallMob;
        private bool CanSpawnLargeMob;
        private float MinimalMobPopulation;

        public GameObject GetMobPrefabNormal()
        {
            return MobPrefabNormal;
        }
        public GameObject GetMobPrefabLarge()
        {
            return MobPrefabLarge;
        }
        public GameObject GetMobPrefabSmall()
        {
            return MobPrefabSmall;
        }
        public float GetMobPopulation()
        {
            return MaxMobPopulation;
        }

        //构造函数
        public MobGroup(GameObject MobPrefabNormal, GameObject MobPrefabSmall, GameObject MobPrefabLarge, float MobPopulation, Vector3 p, MobGroupType mobgrouptype)
        {
            this.MobPrefabNormal = MobPrefabNormal;
            this.MobPrefabSmall = MobPrefabSmall;
            this.MobPrefabLarge = MobPrefabLarge;
            this.MaxMobPopulation = MobPopulation;
            this.p = p;
            this.m_MobGroupType = mobgrouptype;

            //initialize privates
            this.CurrentMobPopulation = 0f;
            this.MobNormalPopulation = 1f;
            this.MobSmallPopulation = 0.5f;
            this.MobLargelPopulation = 2f;

            switch (this.m_MobGroupType)
            {
                case MobGroupType.Standard:
                    CanSpawnNormalMob = true;
                    CanSpawnSmallMob = true;
                    CanSpawnLargeMob = true;
                    MinimalMobPopulation = MobSmallPopulation;
                    break;
                case MobGroupType.AllSmall:
                    CanSpawnNormalMob = false;
                    CanSpawnSmallMob = true;
                    CanSpawnLargeMob = false;
                    MinimalMobPopulation = MobSmallPopulation;
                    break;
                case MobGroupType.AllNormal:
                    CanSpawnNormalMob = true;
                    CanSpawnSmallMob = false;
                    CanSpawnLargeMob = false;
                    MinimalMobPopulation = MobNormalPopulation;
                    break;
                case MobGroupType.AllLarge:
                    CanSpawnNormalMob = false;
                    CanSpawnSmallMob = false;
                    CanSpawnLargeMob = true;
                    MinimalMobPopulation = MobLargelPopulation;
                    break;
                case MobGroupType.SmallandNormal:
                    CanSpawnNormalMob = true;
                    CanSpawnSmallMob = true;
                    CanSpawnLargeMob = false;
                    MinimalMobPopulation = MobSmallPopulation;
                    break;
                case MobGroupType.SmallandLarge:
                    CanSpawnNormalMob = false;
                    CanSpawnSmallMob = true;
                    CanSpawnLargeMob = true;
                    MinimalMobPopulation = MobSmallPopulation;
                    break;
                case MobGroupType.NormalandLarge:
                    CanSpawnNormalMob = true;
                    CanSpawnSmallMob = false;
                    CanSpawnLargeMob = true;
                    MinimalMobPopulation = MobNormalPopulation;
                    break;
                default:
                    CanSpawnNormalMob = true;
                    CanSpawnSmallMob = true;
                    CanSpawnLargeMob = true;
                    MinimalMobPopulation = MobSmallPopulation;
                    break;
            }
        }

        //刷怪逻辑
        public void Spawn(int RoomID)
        {
            while (CurrentMobPopulation < MaxMobPopulation && (MaxMobPopulation - CurrentMobPopulation) >= MinimalMobPopulation)    //如果不判断剩余可用人口数是否还能够刷出来至少一个怪的话，就有可能死循环，导致unity卡死！！！
            {
                SpawnRandomMobType(RoomID);
            }
        }

        //随机某个类型的怪，并记录其人口
        public bool SpawnRandomMobType(int RoomID)
        {
            int i = UnityEngine.Random.Range(0, 3);
            switch (i)
            {
                case 0: //Normal Mob
                    if (CanSpawnNormalMob == false) return false;
                    if (CurrentMobPopulation + MobNormalPopulation > MaxMobPopulation) return false;
                    GameObject newmob = GameObject.Instantiate(MobPrefabNormal, p, Quaternion.identity);
                    moblist.Add(newmob);
                    newmob.GetComponent<EnemyController>().myroom = FindRoomByID(RoomID);
                    CurrentMobPopulation += MobNormalPopulation;
                    return true;
                case 1: //Small Mob
                    if (CanSpawnSmallMob == false) return false;
                    if (CurrentMobPopulation + MobSmallPopulation > MaxMobPopulation) return false;
                    GameObject newmob2 = GameObject.Instantiate(MobPrefabSmall, p, Quaternion.identity);
                    moblist.Add(newmob2);
                    newmob2.GetComponent<EnemyController>().myroom = FindRoomByID(RoomID);
                    CurrentMobPopulation += MobSmallPopulation;
                    return true;
                case 2: //Large Mob
                    if (CanSpawnLargeMob == false) return false;
                    if (CurrentMobPopulation + MobLargelPopulation > MaxMobPopulation) return false;
                    GameObject newmob3 = GameObject.Instantiate(MobPrefabLarge, p, Quaternion.identity);
                    moblist.Add(newmob3);
                    newmob3.GetComponent<EnemyController>().myroom = FindRoomByID(RoomID);
                    CurrentMobPopulation += MobLargelPopulation;
                    return true;
                default:
                    return false;
            }
        }
    }
}