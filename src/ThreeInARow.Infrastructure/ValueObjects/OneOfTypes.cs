using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ValueObjects;

[GenerateOneOf]
public partial class Optional<T> : OneOfBase<T, None>;