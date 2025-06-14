public abstract class StatusModifier
{
    public string id;
    public string displayName;
    public string description;

    public abstract void Apply(PlayerCharacter player);
    public abstract void Remove(PlayerCharacter player);
}