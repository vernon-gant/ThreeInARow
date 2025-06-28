using FluentAssertions;
using OneOf;

namespace ThreeInARow.TestingUtilities;

public static class FluentAssertionsUtilities
{
    public static void ShouldBeOfTypeOneOf<T>(this object actual, string because = "")
    {
        if (actual is not IOneOf oneOf)
            throw new InvalidOperationException("The actual object is not a OneOf type.");

        oneOf.Value.Should().BeOfType<T>(because);
    }
}