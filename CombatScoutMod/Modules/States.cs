using CombatScoutMod.SkillStates;
using CombatScoutMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace CombatScoutMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(BaseShootAttack));

            entityStates.Add(typeof(Slash));

            entityStates.Add(typeof(Roll));

            entityStates.Add(typeof(ThrowBomb));
        }
    }
}