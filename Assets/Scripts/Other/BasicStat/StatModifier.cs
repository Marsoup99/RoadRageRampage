public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

[System.Serializable]
public class StatModifier
{
    public float Value;
    public StatModType Type = StatModType.PercentAdd;

    public StatModifier(float value, StatModType type)
    {
        Value = value;
        Type = type;
    }

    public void ChangeValue(float value)
    {
        Value = value;
    }
}
