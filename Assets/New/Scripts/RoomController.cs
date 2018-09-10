using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class RoomController : MonoBehaviour
{
    //Inspector
    public GameObject Floor;
    public GameObject LeftOpen;
    public GameObject LeftClose;
    public GameObject RightOpen;
    public GameObject RightClose;
    public GameObject UpOpen;
    public GameObject UpClose;
    public GameObject DownOpen;
    public GameObject DownClose;

    public int thisroomid;

    private bool isMobSpawned = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //新的房间关门，刷怪
            Room nextroom = Room.FindRoomByID(thisroomid + 1);
            Room thisroom = Room.FindRoomByID(thisroomid);
            nextroom.Open(Tools.Opposite(thisroom.directionToNextRoom), false);
            if (isMobSpawned == false)
            {
                isMobSpawned = true;
                nextroom.SpawnMobs();
            }

            //老的房间销毁，析构函数里面会摧毁thisroom.roomgo
            thisroom.Destroy();

            //新的房间打开BoxCollider
            nextroom.SetBCEnable(true);
        }
    }
}