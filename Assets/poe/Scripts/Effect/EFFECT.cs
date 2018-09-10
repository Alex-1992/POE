using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EFFECT {
    public static BaseEffect Create(string type, BaseUnit caster, BaseUnit source, BaseUnit target)
    {
        BaseEffect effect = null;
        switch (type)
        {
            case EffectType.EffectProjectile:
                effect = new EffectProjectile();
                break;
            case EffectType.EffectDamage:
                effect = new EffectDamage();
                break;
            default:
                effect = new BaseEffect();
                break;
        }
        return effect;
    }
}
