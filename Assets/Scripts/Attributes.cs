using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]

public class Attributes
{
    public float Base_Value;

    public float Value
    {
        get
        {
            if (Is_Dirty || Base_Value != Last_Base_Value)
            {
                Last_Base_Value = Base_Value;
                _Value_ = CalculateFinalValue();
                Is_Dirty = false;
            }
            return _Value_;
        }
    }

    protected bool Is_Dirty = true;
    protected float _Value_;
    protected float Last_Base_Value = float.MinValue;

    protected readonly List<StatsModifier> Stat_Modifiers;
    public readonly ReadOnlyCollection<StatsModifier> Public_Stat_Modifiers;

    public Attributes()
    {
        Stat_Modifiers = new List<StatsModifier>();
        Public_Stat_Modifiers = Stat_Modifiers.AsReadOnly();
    }

    
    public Attributes(float base_value) : this()
    {
        Base_Value = base_value;
    }

    public virtual void AddModifier(StatsModifier mod)
    {
        Is_Dirty = true;
        Stat_Modifiers.Add(mod);
        Stat_Modifiers.Sort(CompareModifierOrder);
    }

    protected virtual int CompareModifierOrder(StatsModifier a, StatsModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        }
        else if (a.Order > b.Order)
        {
            return 1;
        }
        return 0;
    }

    public virtual bool RemoveModifier(StatsModifier mod)
    {
        if (Stat_Modifiers.Remove(mod))
        {
            Is_Dirty = true;
            return true;
        }
        return false;
    }

    protected virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool Removed = false;

        for (int i = (Stat_Modifiers.Count - 1); i >=0; i--)
        {
            if (Stat_Modifiers[i].Source == source)
            {
                Is_Dirty = true;
                Removed = true;
                Stat_Modifiers.RemoveAt(i);
            }
        }

        return Removed;
    }

    protected virtual float CalculateFinalValue()
    {
        float Final_Value = Base_Value;
        float Sum_Of_Percent_Add = 0;

        for (int i = 0; i < Stat_Modifiers.Count; i++)
        {
            StatsModifier mod = Stat_Modifiers[i];

            if (mod.Type == Stat_Modifier_Type.Flat)
            {
                Final_Value += Stat_Modifiers[i].Value;
            }
            else if (mod.Type == Stat_Modifier_Type.Percent_Add)
            {
                Sum_Of_Percent_Add += mod.Value;

                if (i + 1 >= Stat_Modifiers.Count || Stat_Modifiers[i + 1].Type != Stat_Modifier_Type.Percent_Add)
                {
                    Final_Value *= (1 + Sum_Of_Percent_Add);
                    Sum_Of_Percent_Add = 0;
                }
            }
            else if (mod.Type == Stat_Modifier_Type.Percent_Multiply)
            {
                Final_Value *= (1 + mod.Value);
            }
        }

        return (float)Math.Round(Final_Value, 4);
    }
}
