using ModifierLibrary;
using System;
using Xunit;

namespace Modifier.Test;

public class ModifierTest
{
    private ContentType _existing = new ContentType("StandardPage", Guid.NewGuid(), "Description");

    [Fact]
    public void Apply_WhenApplied_ShouldApplyModifications()
    {
        var updatedValue = "UpdatedName";
        var modified = _existing
            .Modify()
            .Set(c => c.Name, updatedValue)
            .Apply();

        Assert.Equal(updatedValue, modified.Name);
    }

    [Fact]
    public void Apply_WhenApplied_ShouldCreateNewInstances()
    {
        var updatedValue = "UpdatedName";
        var modified = _existing
            .Modify()
            .Set(c => c.Name, updatedValue)
            .Apply();

        Assert.NotSame(modified, _existing);
    }

    [Fact]
    public void Apply_WhenSeveralModifications_ShouldApplyAll()
    {
        var updatedValue = "UpdatedName";
        var updatedId = Guid.NewGuid();
        var modified = _existing
            .Modify()
            .Set(c => c.Name, updatedValue)
            .Set(c => c.Id, updatedId)
            .Apply();

        Assert.Equal(updatedValue, modified.Name);
        Assert.Equal(updatedId, modified.Id);
    }

    [Fact]
    public void Apply_WhenPropertyIsNotModified_ShouldKeepOriginalValue()
    {
        var updatedValue = "UpdatedName";
        var modified = _existing
            .Modify()
            .Set(c => c.Name, updatedValue)
            .Apply();

        Assert.Equal(_existing.Id, modified.Id);
    }
}
