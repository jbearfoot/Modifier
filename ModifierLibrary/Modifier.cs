using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace ModifierLibrary;

public class Modifier<T> where T : class
{
    private T _model;
    private Dictionary<string, object> _modifications = new();
    private static ConcurrentDictionary<Type, ConstructorInfo> _constructionInfo = new();

    public Modifier(T model)
    {
        _model = model;
    }

    public Modifier<T> Set<TValue>(Expression<Func<T, TValue>> expression, TValue? newValue)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            _modifications[memberExpression.Member.Name] = newValue!;
        }
        else
        {
            throw new NotSupportedException("Only member expressions are supported");
        }
        return this;
    }

    public T Apply()
    {
        var modelType = _model.GetType();
        var constructor = _constructionInfo.GetOrAdd(modelType, type => type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First());
        var values = new List<object?>();
        foreach (var parameter in constructor.GetParameters())
        {
            if (_modifications.TryGetValue(parameter.Name!, out var value))
            {
                values.Add(value);
            }
            else
            {
                values.Add(modelType.GetProperty(parameter.Name!)?.GetValue(_model));
            }
        }

        return (T)constructor.Invoke(values.ToArray());
    }
}

