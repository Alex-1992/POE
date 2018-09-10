using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//效果: 发射投射物
public class EffectProjectile : BaseEffect
{

    //================属性配置
    public int _projectilesQuantity;    //投射物数量
    public int _penetrationRate;   //穿透率
    public int _projectileRate;   //投射物速率
    public int _chainNumber;   //连锁量
    public int _splitNumber;   //分裂量
    public bool _boomerang;    //是否返回
    public List<string> _firstCollisionEffect = new List<string>();   //首次碰撞时效果ID
    public List<string> _collisionEffect = new List<string>();   //后续碰撞效果ID
    public List<string> _endEffect = new List<string>();   //投射物消失效果ID
    //================局部变量


    //================私有方法
    private void OnCollisionTrigger()       //当碰撞时触发
    {

        //如果是无目标效果,碰撞到任何unit触发,并设置target,如果是有目标效果,碰撞的unit与target相同时触发

        //处理碰撞效果
        HandleCllisionEffect();
        //处理穿透
        if (HandlePierceThrought() == false) {
            //处理分裂
            if (HandleSplit() == false) {
                //处理连锁
                if (HandleChain() == false) {
                    //处理返回
                    if (HandleBoomerang() == false) {
                        //处理消失
                        HandleEndEffect();
                    }
                }
            }
        }
    }
    private void HandleCllisionEffect()     //处理碰撞效果
    {
        List<string> effectList = null;
        BaseEffect collisionEffect = null;
        if (_generation == 0) {
            effectList = _firstCollisionEffect;
        } else {
            effectList = _collisionEffect;
        }
        for (int i = 0; i < effectList.Count; i++) {
            collisionEffect = EFFECT.Create(effectList[i], _caster, _source, _target);
            collisionEffect.StartUp();
        }
    }
    private bool HandlePierceThrought()     //处理穿透,返回true表示穿透成功,不处理之后的分裂等逻辑
    {
        float penetrationRate = Random.Range(0, 100);
        if (penetrationRate < _penetrationRate) {
            //代数增加
            _generation++;
            return true;
        }
        return false;
    }
    private bool HandleSplit()    //处理分裂
    {
        return false;
    }
    private bool HandleChain()    //处理连锁
    {
        return false;
    }
    private bool HandleBoomerang()    //处理返回
    {
        return false;
    }
    private void HandleEndEffect()  //处理消失效果
    {

    }
    //================公有方法
    public new void StartUp()
    {

    }
    public EffectProjectile() //初始化时代数增加
    {
        if (_sourceEffect != null) {
            _generation = _sourceEffect.Teneration + 1;
        }
    }
}
