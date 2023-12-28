using System.Collections.Generic;
using System;
public abstract class EntityLogicSub : LogicSub
{
    public GameEntity entity;

    public List<Type> requiredAbilityTypes = new List<Type>();

    protected EntityLogicSub(GameEntity entity)
    {
        this.entity = entity;
    }

    public virtual void Init()
    {
    }
    protected void Require(Type type)
    {
        requiredAbilityTypes.Add(type);
    }
    protected void Require<T>() where T : AbilityBase
    {
        Require(typeof(T));
    }

    

}
