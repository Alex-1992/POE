using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPControllerPlayer : HPControllerBase
{

    //inspector
    public GameObject RebornPanel;
    public Text RebornText;
    public BarController PlayerHPBarController;
    public BarController PlayerMPBarController;
    public BarController PlayerEXPBarController;
    public Text PlayerLevelText;

    // Use this for initialization
    new void Start()
    {
        //初始化Statics
        if (gameObject.CompareTag("Player") && POEStatics.PlayerHPController == null) POEStatics.PlayerHPController = this;
        //父类的初始化
        base.Start();
    }

    new public void TakeDamage(POEStatus Source)
    {
        base.TakeDamage(Source);
        if (CurrentHp <= 0)
        {
            this.Kill();
        }
    }

    private void Kill()
    {
        //重生流程

        //打开重生界面
        POEStatics.ISREBORN = true;
        RebornPanel.SetActive(true);
        //摧毁所有怪
        GameObject[] moblist = GameObject.FindGameObjectsWithTag(POEStatics.Const.EnemyTag);
        foreach (var item in moblist)
        {
            Destroy(item);
        }
        POEStatics.CardManager.RemoveAllLevelCardsFromPlayer();
        //获得重生卡牌
        int temp = 0;
        if (POEStatics.PlayerLevelController.GetCurrenLevel() == POEStatics.Const.InitLevel)
        {
            temp = 0;
        }
        else
        {
            float temp2 = POEStatics.MobLevel + POEStatics.PlayerLevelController.GetCurrenLevel();
            temp2 -= 2 * POEStatics.Const.InitLevel;
            temp = Mathf.RoundToInt(temp2 / 2);
        }
        RebornText.text = string.Format(RebornText.text, temp);
        POEStatics.CardManager.AddRebornCardsToPlayer(temp);
        //重置玩家属性和各种常量，玩家+钱
        POEStatics.PlayerCoins += 10;
        POEStatics.Player.SendMessage("ResetPlayer", SendMessageOptions.RequireReceiver);
        SetHPbyPercent(1f);
        SetMPbyPercent(1f);
        POEStatics.MobLevel = POEStatics.Const.InitLevel;
        POEStatics.MobPopulation = POEStatics.Const.InitMobPopulation;
        POEStatics.ISREBORN = false;
    }
}
