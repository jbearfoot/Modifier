namespace ModifierLibrary;

public record ContentType(string Name, Guid Id, string Description) : IModifiable<ContentType>
{
    public Modifier<ContentType> Modify() => new Modifier<ContentType>(this);
}
