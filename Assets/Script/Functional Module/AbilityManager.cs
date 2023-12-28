using System;
using System.Collections.Generic;
using UnityEngine;
public class AbilityManager : MonoBehaviour
{
    private GameEntity owner;
    public bool isActivated{get{return gameObject.activeSelf;}}

    public GameEntity Owner { get{
        if(owner==null)
            owner=GetComponent<GameEntity>();
        return owner;
    }}

    public List<AbilityBase> abilities = new List<AbilityBase>();
    public AbilityBase this[string abilityName]
    {
        get
        {
            return abilities.Find((ability) => ability.AbilityName == abilityName);
        }
    }
    public T FindAbility<T>(string abilityName) where T:AbilityBase
    {
        return abilities.Find((ability) => ability.AbilityName == abilityName) as T;
    }
    public AbilityBase AddAbility(AbilityBase ability)
    {
        abilities.Add(ability);
        ability.owner = Owner;
        ability.StartAbility();
        return ability;
    }

    public void RemoveAbility(AbilityBase ability)
    {
        abilities.Remove(ability);
        ability.EndAbility();
    }

    public void RemoveAbility(string abilityName)
    {
        var t=abilities.Find((ability) => ability.AbilityName == abilityName);
        if(t!=null)
            RemoveAbility(t);
    }
    [Obsolete("disable")]
    public void RemoveAllAbility()
    {
        abilities.Clear();
    }
    [Obsolete("initialize")]
    public void StartAbility(string abilityName)
    {
        abilities.Find((ability) => ability.AbilityName == abilityName)?.StartAbility();
    }
    [Obsolete("disable")]
    public void EndAbility(string abilityName)
    {
        abilities.Find((ability) => ability.AbilityName == abilityName)?.EndAbility();
    }

    public void UpdateAbility()
    {   if (abilities.Count == 0)
            return;
        foreach (var ability in abilities)
        {
            ability.UpdateAbility();
        }
    }

    public void FixedUpdateAbility()
    {
        if (abilities.Count == 0)
            return;
        foreach (var ability in abilities)
        {
            ability.FixedUpdateAbility();
        }
    }

    public void Update()
    {
        if (!isActivated)
            return;
        UpdateAbility();
    }

    public void FixedUpdate()
    {   
        if (!isActivated)
            return;
        FixedUpdateAbility();
    }

}