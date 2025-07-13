using OneOf;
using OneOf.Types;

namespace ThreeInARow.ValueObjects;

[GenerateOneOf]
public partial class Optional<T> : OneOfBase<T, None>;