using UnityEngine;
using System;
public static class DamageCommand
{

    public static EntityCommand Get(float damage, int priority = 0, GameEntity sender = null)
    {
        return EntityCommand.CreateCommand((command) =>
        {
            //command.Target.health.Value -= damage;
            command.Target.health.SendCommand( FloatChangeCommand.ChangeCommand(
                -damage, command.Target.health, priority
            ));
        }, priority, sender);
    }
}

public static class BuffCommand
{
    public static EntityCommand Get(Buff<GameEntity> buff, int priority = 0, GameEntity sender = null)
    {
        return EntityCommand.CreateCommand((command) =>
        {
            command.Target.AddBuff(buff);
        }, priority, sender);
    }
}