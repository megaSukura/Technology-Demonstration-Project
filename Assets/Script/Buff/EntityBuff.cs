using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBuff : Buff<GameEntity>
{
    public int buffStrength;

    public EntityBuff(int buffStrength, float duration, EntityBuffEffect effect) 
    {
        this.buffStrength = buffStrength;
        this.Duration = duration;
        effect.buffStrength = buffStrength;
        this.Effect = effect;
    }
}
public abstract class EntityBuffEffect : BuffEffect<GameEntity>
{
    public int buffStrength;
}

//concrete buff and buffEffect

public class EntityBuffEffect_Slow : EntityBuffEffect
{
    private const float V = 0.1f;

    public EntityBuffEffect_Slow()
    {
        this.Name = "SlowBuff";
    }
    private AttributeFloat targetSpeed;
    public override void Apply(Buff<GameEntity> buff, GameEntity target)
    {
        targetSpeed = target.speed;
        targetSpeed.panleFramework["Base"].PercentagePanle=-Mathf.Clamp01(V * buffStrength);
    }

    public override void OnStack(Buff<GameEntity> buff, GameEntity target, Buff<GameEntity> newbuff, List<Buff<GameEntity>> buffs, BuffManager<GameEntity> buffManager = null)
    {
        if(newbuff.Effect is EntityBuffEffect_Slow newef)
        {
            if(newef.buffStrength<buffStrength)
            {
                newef.buffStrength= buffStrength ;
            }
            buffManager.RemoveBuff(buff, target);
                buffManager.AddBuff(newbuff, target);
        }
    }

    public override void Remove(Buff<GameEntity> buff, GameEntity target)
    {
        targetSpeed.panleFramework["Base"].PercentagePanle=0f;
    }

    public override void Update(Buff<GameEntity> buff, GameEntity target, float timePassed, float percentage)
    {
        targetSpeed.panleFramework["Base"].PercentagePanle=-Mathf.Clamp01(V * buffStrength);
    }
}



