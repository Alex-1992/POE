using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControllerPlayer : LevelControllerBase
{

    //inspector
    public BarController PlayerEXPBarController;
    public Text PlayerLevelText;

    protected override void Awake()
    {
        //初始化Statics
        if (POEStatics.PlayerLevelController == null && gameObject.CompareTag(POEStatics.Const.PlayerTag)) POEStatics.PlayerLevelController = this;
        S = POEStatics.PlayerPOEStatus;
        base.Awake();

        Level = new List<LevelData>()
        {
            new LevelData(){Level = 0, TotalExpToNextLevel = 0 , StatusModifiers = null },  //从0开始这样升序遍历的时候就不用再麻烦的+1 -1什么的了，Level的Index能够和CurrentLevel对的上（当InitialLevel == 1的时候）
            new LevelData(){Level = 1, TotalExpToNextLevel = 0 , StatusModifiers = null },
            new LevelData(){Level = 2, TotalExpToNextLevel = 50 , StatusModifiers = null },
            new LevelData(){Level = 3, TotalExpToNextLevel = 100 , StatusModifiers = null },
            new LevelData(){Level = 4, TotalExpToNextLevel = 200 , StatusModifiers = null },
            new LevelData(){Level = 5, TotalExpToNextLevel = 300 , StatusModifiers = null },
            new LevelData(){Level = 6, TotalExpToNextLevel = 500 , StatusModifiers = null },
            new LevelData(){Level = 7, TotalExpToNextLevel = 700 , StatusModifiers = null },
            new LevelData(){Level = 8, TotalExpToNextLevel = 1000 , StatusModifiers = null },
            new LevelData(){Level = 9, TotalExpToNextLevel = 1300 , StatusModifiers = null },
            new LevelData(){Level = 10, TotalExpToNextLevel = 1700 , StatusModifiers = null },
            new LevelData(){Level = 11, TotalExpToNextLevel = 2100 , StatusModifiers = null },
            new LevelData(){Level = 12, TotalExpToNextLevel = 2600 , StatusModifiers = null },
            new LevelData(){Level = 13, TotalExpToNextLevel = 3100 , StatusModifiers = null },
            new LevelData(){Level = 14, TotalExpToNextLevel = 3700 , StatusModifiers = null },
            new LevelData(){Level = 15, TotalExpToNextLevel = 4300 , StatusModifiers = null },
            new LevelData(){Level = 16, TotalExpToNextLevel = 5000 , StatusModifiers = null },
            new LevelData(){Level = 17, TotalExpToNextLevel = 6000 , StatusModifiers = null },
            new LevelData(){Level = 18, TotalExpToNextLevel = 7000 , StatusModifiers = null },
            new LevelData(){Level = 19, TotalExpToNextLevel = 8000 , StatusModifiers = null },
            new LevelData(){Level = 20, TotalExpToNextLevel = 9000 , StatusModifiers = null },
            new LevelData(){Level = 21, TotalExpToNextLevel = 10000 , StatusModifiers = null },
            new LevelData(){Level = 22, TotalExpToNextLevel = 11000 , StatusModifiers = null },
            new LevelData(){Level = 23, TotalExpToNextLevel = 12000 , StatusModifiers = null },
            new LevelData(){Level = 24, TotalExpToNextLevel = 13000 , StatusModifiers = null },
            new LevelData(){Level = 25, TotalExpToNextLevel = 14000 , StatusModifiers = null },
            new LevelData(){Level = 26, TotalExpToNextLevel = 15000 , StatusModifiers = null },
            new LevelData(){Level = 27, TotalExpToNextLevel = 16000 , StatusModifiers = null },
            new LevelData(){Level = 28, TotalExpToNextLevel = 17000 , StatusModifiers = null },
            new LevelData(){Level = 29, TotalExpToNextLevel = 18000 , StatusModifiers = null },
            new LevelData(){Level = 30, TotalExpToNextLevel = 19000 , StatusModifiers = null },
            new LevelData(){Level = 31, TotalExpToNextLevel = 20000 , StatusModifiers = null },
            new LevelData(){Level = 32, TotalExpToNextLevel = 21000 , StatusModifiers = null },
        };
    }

    private void Start()
    {
        PlayerEXPBarController.SetBarValue(0f);
        PlayerLevelText.text = POEStatics.Const.InitLevel.ToString();
    }

    public override void ExpGain(float expgain)
    {
        base.ExpGain(expgain);
        PlayerEXPBarController.SetBarValue(CurrentExp / GetExpToNextLevel());
    }

    protected override void LevelUp(int levleup)
    {
        base.LevelUp(levleup);
        POEStatics.Player.SendMessage("SetHPbyPercent", 1f, SendMessageOptions.RequireReceiver);  //玩家升级后要加满血，怪的话无所谓
        PlayerLevelText.text = GetCurrenLevel().ToString();
    }

    public void ResetPlayer()
    {
        CurrentExp = 0;
        CurrentLevel = POEStatics.Const.InitLevel;
        POEStatics.PlayerMeshRenderer.material.color = POEStatics.Const.PlayerMeshRendererOriginColor;
    }
}
