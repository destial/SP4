public enum Stat_Modifier_Type
{
    Flat,
    Percent_Add,
    Percent_Multiply,
}

public class StatsModifier
{
    public readonly float Value;
    public readonly Stat_Modifier_Type Type;
    public readonly int Order;
    public readonly object Source;

    public StatsModifier(float value, Stat_Modifier_Type type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    public StatsModifier(float value, Stat_Modifier_Type type) : this (value, type, (int)type, null) { }
    public StatsModifier(float value, Stat_Modifier_Type type, int order) : this(value, type, order, null) { }
    public StatsModifier(float value, Stat_Modifier_Type type, object source) : this(value, type, (int)type, source) { }
}
