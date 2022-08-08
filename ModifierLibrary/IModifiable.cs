namespace ModifierLibrary;

public interface IModifiable<TModel> where TModel : class
{
    Modifier<TModel> Modify();
}
