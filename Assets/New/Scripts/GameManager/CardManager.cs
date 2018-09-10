using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡牌管理类
[DisallowMultipleComponent]
public class CardManager : MonoBehaviour
{

    //Inspector
    public GameObject PlayerCardListContentUI;
    public GameObject PickCardUI;
    public GameObject PickCardContentUI;
    public GameObject PlayerCardUIPrefab;

    //private
    private List<LevelCardData> LevelCardDatabase = new List<LevelCardData>();
    private List<CardData> RebornCardDatabase = new List<CardData>();
    private static List<Card> AllLevelCardsOnPlayer = new List<Card>();
    private static List<Card> AllRebornCardsOnPlayer = new List<Card>();

    //Components
    //private POEStatus S;

    void Start()
    {
        //初始化Statics
        if (POEStatics.CardManager == null) POEStatics.CardManager = this;

        //初始化数据库
        PickCardUI.SetActive(false);
        InitLevelCardDatabase();

        //测试代码
        //AddCardToPlayer(new CardData("攻速和移动速度", new List<StatusModifier>() {
        //    new StatusModifier(K.AttackSpeed, 0, 0.1f, 0),
        //    new StatusModifier(K.MoveSpeed, 0, 0.1f, 0)
        //}));
        //AddCardToPlayer(new CardData("最大MP", new List<StatusModifier>() {
        //    new StatusModifier(K.MaxMP, 0, 0.1f, 0),
        //}));
        //AddCardToPlayer(new CardData("最大HP", new List<StatusModifier>() {
        //    new StatusModifier(K.MaxHP, 50, 0.1f, 0),
        //}));
    }

    //等级卡牌数据表头
    private class LevelCardData
    {
        public int minlevel;
        public int maxlevel;
        public float weight;
        public CardData carddata;

        public LevelCardData(int minlevel, int maxlevel, float weight, CardData carddata)
        {
            this.minlevel = minlevel;
            this.maxlevel = maxlevel;
            this.weight = weight;
            this.carddata = carddata;
        }
    }

    //重生卡牌数据表头
    private class RebornCardData
    {
        public CardData carddata;
        public int CoinCost;

        public RebornCardData(int CoinCost, CardData carddata)
        {
            this.CoinCost = CoinCost;
            this.carddata = carddata;
        }
    }

    //初始化等级卡牌数据库
    private void InitLevelCardDatabase()
    {
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("攻击速度", new List<StatusModifier>() { new StatusModifier(K.AttackSpeed, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("移动速度", new List<StatusModifier>() { new StatusModifier(K.MoveSpeed, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础物理伤害", new List<StatusModifier>() { new StatusModifier(K.BulletPhysicalDamage, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础火焰伤害", new List<StatusModifier>() { new StatusModifier(K.BulletFireDamage, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础冰霜伤害", new List<StatusModifier>() { new StatusModifier(K.BulletColdDamage, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础闪电伤害", new List<StatusModifier>() { new StatusModifier(K.BulletLightinglDamage, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("物理伤害", new List<StatusModifier>() { new StatusModifier(K.BulletPhysicalDamage, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础生命恢复", new List<StatusModifier>() { new StatusModifier(K.HPRegeneration, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("基础法力恢复", new List<StatusModifier>() { new StatusModifier(K.MPRegeneration, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("生命恢复", new List<StatusModifier>() { new StatusModifier(K.HPRegeneration, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("升级选牌数量", new List<StatusModifier>() { new StatusModifier(K.PlayerLevelUpCardCount, 1, 0, 0) })));
        LevelCardDatabase.Add(new LevelCardData(0, int.MaxValue, 1000f, new CardData("降低法力消耗", new List<StatusModifier>() { new StatusModifier(K.BulletMPCost, 0, -0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("疯狂射击", new List<StatusModifier>() { new StatusModifier(K.AttackSpeed, 0, 0, 0.3f), new StatusModifier(K.BulletMPCost, 0, 0, 0.3f) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("头脑风暴", new List<StatusModifier>() { new StatusModifier(K.MPRegeneration, 0, 0, 0.3f), new StatusModifier(K.MaxHP, 0, 0, -0.3f) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("攻速和移动速度", new List<StatusModifier>() { new StatusModifier(K.AttackSpeed, 0, 0.1f, 0), new StatusModifier(K.MoveSpeed, 0, 0.1f, 0), new StatusModifier(K.BulletMPCost, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("多重飞弹", new List<StatusModifier>() { new StatusModifier(K.BulletCount, 2, 0, 0), new StatusModifier(K.BulletDamageMultiplier, 0, 0, -0.2f) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("重型弹丸", new List<StatusModifier>() { new StatusModifier(K.BulletDamageMultiplier, 2, 0, 0), new StatusModifier(K.AttackSpeed, 0, 0, -0.2f), new StatusModifier(K.BulletMPCost, 0, 0, 0.2f) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("伤害与飞弹速度", new List<StatusModifier>() { new StatusModifier(K.BulletDamageMultiplier, 0, 0.1f, 0), new StatusModifier(K.BulletSpeed, 0, 0.1f, 0), new StatusModifier(K.BulletMPCost, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("最大生命与恢复", new List<StatusModifier>() { new StatusModifier(K.MaxHP, 0, 0.1f, 0), new StatusModifier(K.HPRegeneration, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("最大法力与恢复", new List<StatusModifier>() { new StatusModifier(K.MaxMP, 0, 0.1f, 0), new StatusModifier(K.MPRegeneration, 0, 0.1f, 0) })));
        LevelCardDatabase.Add(new LevelCardData(5, int.MaxValue, 1000f, new CardData("飞弹穿透", new List<StatusModifier>() { new StatusModifier(K.BulletDamageCount, 1, 0, 0), new StatusModifier(K.BulletMPCost, 0, 0.1f, 0) })));

        RebornCardDatabase.Add(new CardData("移动速度", new List<StatusModifier>() { new StatusModifier(K.MoveSpeed, 0, 0.2f, 0) }));
        RebornCardDatabase.Add(new CardData("疯狂射击", new List<StatusModifier>() { new StatusModifier(K.AttackSpeed, 0, 0, 0.3f), new StatusModifier(K.BulletMPCost, 0, 0, 0.3f) }));
        RebornCardDatabase.Add(new CardData("飞弹穿透", new List<StatusModifier>() { new StatusModifier(K.BulletDamageCount, 1, 0, 0), new StatusModifier(K.BulletMPCost, 0, 0.1f, 0) }));
        RebornCardDatabase.Add(new CardData("重型弹丸", new List<StatusModifier>() { new StatusModifier(K.BulletDamageMultiplier, 2, 0, 0), new StatusModifier(K.AttackSpeed, 0, 0, -0.2f), new StatusModifier(K.BulletMPCost, 0, 0, 0.2f) }));
        RebornCardDatabase.Add(new CardData("升级选牌数量", new List<StatusModifier>() { new StatusModifier(K.PlayerLevelUpCardCount, 1, 0, 0) }));
        RebornCardDatabase.Add(new CardData("头脑风暴", new List<StatusModifier>() { new StatusModifier(K.MPRegeneration, 0, 0, 0.3f), new StatusModifier(K.MaxHP, 0, 0, -0.3f) }));
        RebornCardDatabase.Add(new CardData("多重飞弹", new List<StatusModifier>() { new StatusModifier(K.BulletCount, 2, 0, 0), new StatusModifier(K.BulletDamageMultiplier, 0, 0, -0.2f) }));
        RebornCardDatabase.Add(new CardData("移动法力恢复", new List<StatusModifier>() { new StatusModifier(K.MPRegenerationMultiplierWhileMoving, 0, 0.3f, 0) }));
        RebornCardDatabase.Add(new CardData("基础物理伤害", new List<StatusModifier>() { new StatusModifier(K.BulletPhysicalDamage, 10, 0, 0) }));
        RebornCardDatabase.Add(new CardData("基础火焰伤害", new List<StatusModifier>() { new StatusModifier(K.BulletFireDamage, 10, 0, 0) }));
        RebornCardDatabase.Add(new CardData("基础冰霜伤害", new List<StatusModifier>() { new StatusModifier(K.BulletColdDamage, 10, 0, 0) }));
        RebornCardDatabase.Add(new CardData("基础闪电伤害", new List<StatusModifier>() { new StatusModifier(K.BulletLightinglDamage, 10, 0, 0) }));
        RebornCardDatabase.Add(new CardData("元素大师", new List<StatusModifier>() { new StatusModifier(K.BulletFireDamage, 0, 0.2f, 0), new StatusModifier(K.BulletColdDamage, 0, 0.2f, 0), new StatusModifier(K.BulletLightinglDamage, 0, 0.2f, 0), new StatusModifier(K.BulletPhysicalDamage, 0, 0, -0.5f) }));
    }

    private void ShowCardsToPick(List<CardData> CardDatas)
    {
        if (CardDatas.Count != 0)
        {
            PickCardUI.SetActive(true);
            foreach (var item in CardDatas)
            {
                GameObject cardUI = Instantiate(PlayerCardUIPrefab, PickCardContentUI.transform);
                CardController cc = cardUI.GetComponent<CardController>();
                cc.CardName.text = item.CardName;
                cc.CardDescription.text = item.GetCardDescription();
                cc.RemoveFromPlayerBtn.gameObject.SetActive(false);
                cc.PickBtn.gameObject.SetActive(true);
                //让CardContrller记下来自己是谁（也就是Card对象）这样好删除
                cc.Card = new Card() { CardData = item, CardController = cc };
            }
        }
        Time.timeScale = 0f;
    }

    //传入的参数是显示几张卡牌
    public void ShowCardsToPick(int count)
    {
        List<CardData> rtn = new List<CardData>();
        foreach (var item in LevelCardDatabase)
        {
            if (item.minlevel <= POEStatics.PlayerLevelController.GetCurrenLevel() && POEStatics.PlayerLevelController.GetCurrenLevel() < item.maxlevel)
            {
                rtn.Add(item.carddata);
            }
        }
        while (rtn.Count > count)
        {
            rtn.RemoveAt(Random.Range(0, rtn.Count));
        }
        ShowCardsToPick(rtn);
    }

    //为玩家增加一张卡牌的方法，这里省略了玩家POEStatus的参数，不用往里面传了
    public void AddCardToPlayer(CardData carddata)
    {
        carddata.CardApplyTo(POEStatics.PlayerPOEStatus);
        GameObject cardUI = Instantiate(PlayerCardUIPrefab, PlayerCardListContentUI.transform);
        CardController cc = cardUI.GetComponent<CardController>();
        cc.CardName.text = carddata.CardName;
        cc.CardDescription.text = carddata.GetCardDescription();
        cc.RemoveFromPlayerBtn.gameObject.SetActive(true);
        cc.PickBtn.gameObject.SetActive(false);
        //让CardContrller记下来自己是谁（也就是Card对象），这样好删除
        cc.Card = new Card() { CardData = carddata, CardController = cc };
        CardManager.AllLevelCardsOnPlayer.Add(cc.Card);
        //处理选卡界面
        PickCardUI.SetActive(false);
        for (int i = 0; i < PickCardContentUI.transform.childCount; i++)
        {
            Destroy(PickCardContentUI.transform.GetChild(i).gameObject);
        }
        Time.timeScale = 1f;
    }

    //为玩家增加一张转生牌的方法
    private void AddRebornCardToPlayer(CardData carddata)
    {
        carddata.CardApplyTo(POEStatics.PlayerPOEStatus);
        GameObject cardUI = Instantiate(PlayerCardUIPrefab, PlayerCardListContentUI.transform);
        CardController cc = cardUI.GetComponent<CardController>();
        cc.CardName.text = "转·" + carddata.CardName;
        cc.CardDescription.text = carddata.GetCardDescription();
        cc.RemoveFromPlayerBtn.gameObject.SetActive(true);
        cc.PickBtn.gameObject.SetActive(false);
        //让CardContrller记下来自己是谁（也就是Card对象），这样好删除
        cc.Card = new Card() { CardData = carddata, CardController = cc };
        CardManager.AllRebornCardsOnPlayer.Add(cc.Card);
    }

    //实际调用的为玩家增加转生牌的方法，传入的参数是张数
    public void AddRebornCardsToPlayer(int RebornCardsCount)
    {
        if (RebornCardsCount == 0)
        {
            Debug.LogFormat("【{0}】：你传入了0，所以什么都没发生", this.GetType());
            return;
        }
        if (RebornCardsCount < 0)
        {
            Debug.LogErrorFormat("【{0}】：不应该传入<0的参数！", this.GetType());
            return;
        }
        //Debug.Log(RebornCardsCount);
        List<CardData> rtn = RebornCardDatabase;
        while (rtn.Count > RebornCardsCount)
        {
            rtn.RemoveAt(Random.Range(0, rtn.Count));
        }
        foreach (var item in rtn)
        {
            AddRebornCardToPlayer(item);
        };
    }

    //为玩家移除一张卡牌的方法
    public void RemoveCardFromPlayer(Card Card)
    {
        Card.CardData.CardRemoveFrom(POEStatics.PlayerPOEStatus);
        Destroy(Card.CardController.gameObject);
        Card.CardData = null;
        Card = null;
    }

    //为玩家移除所有等级卡牌的方法
    public void RemoveAllLevelCardsFromPlayer()
    {
        if (AllLevelCardsOnPlayer.Count == 0)
        {
            return;
        }
        foreach (var item in CardManager.AllLevelCardsOnPlayer)
        {
            RemoveCardFromPlayer(item);
        }
        AllLevelCardsOnPlayer.Clear();
    }
}

//卡牌类
public class Card
{
    public CardData CardData;
    public CardController CardController;
}

//卡牌数据类
public class CardData
{
    public string CardName;
    public List<StatusModifier> StatusModifiers = new List<StatusModifier>();

    public CardData(string name, List<StatusModifier> StatusModifiers)
    {
        this.CardName = name;
        this.StatusModifiers = StatusModifiers;
    }

    public void CardApplyTo(POEStatus S)
    {
        foreach (var item in StatusModifiers)
        {
            item.ApplyTo(S);
        }
    }

    public void CardRemoveFrom(POEStatus S)
    {
        foreach (var item in StatusModifiers)
        {
            item.RemoveFrom(S);
        }
    }

    public string GetCardDescription()
    {
        string rtn = "";
        foreach (var item in StatusModifiers)
        {
            rtn += item.ToString();
        }
        rtn = rtn.Substring(0, rtn.Length - 1); //删掉最后多余的那个\n
        return rtn;
    }
}

/*
 * 玩家每次升级的时候会随机出来3张卡片，让玩家选择1张
 *   也就是说我们要有1个UI，能够随机生成卡片
 *   当玩家点击的时候，再把这个卡片的效果加到玩家身上
 * 玩家可以主动抛弃已经选过的卡片
 *   也就是说玩家可以随时查看现在已有的全部卡片列表，然后想删掉哪个就删掉哪个 
 * 当通过某些房间的时候，也可能会随机出来几张卡片，让玩家选择
 */
