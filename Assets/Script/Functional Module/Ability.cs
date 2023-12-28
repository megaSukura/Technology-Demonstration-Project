using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase
{
    public readonly string AbilityName;
    public GameEntity owner;
    public bool isAbilityActive{get;private set;}

    #region AbilityAction (次要功能)
    public Action<GameEntity> OnAbilityStart;
    public Action<GameEntity> OnAbilityEnd;

    public Action<GameEntity> OnAbilityUpdate;

    public Action<GameEntity> OnAbilityFixedUpdate;
    #endregion
    public AbilityBase( GameEntity owner, string abilityName)
    {
        AbilityName = abilityName;
        this.owner = owner;
    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void StartAbility()
    {
        isAbilityActive = true;
        OnAbilityStart?.Invoke(owner);
    }
    /// <summary>
    /// 析构
    /// </summary>
    public virtual void EndAbility()
    {   
        if(isAbilityActive)
            OnAbilityEnd?.Invoke(owner);
        isAbilityActive = false;
    }

    public virtual void UpdateAbility()
    {
        if (isAbilityActive)
            OnAbilityUpdate?.Invoke(owner);
    }
    public virtual void FixedUpdateAbility()
    {
        if (isAbilityActive)
            OnAbilityFixedUpdate?.Invoke(owner);
    }

    #if UNITY_EDITOR
    public virtual void OndrawInspector()
    {
    }
    #endif

}



