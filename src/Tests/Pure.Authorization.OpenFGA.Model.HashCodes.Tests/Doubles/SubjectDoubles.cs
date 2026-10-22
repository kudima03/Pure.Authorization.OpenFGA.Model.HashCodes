using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeObject(IString Type, IString Id) : IObject;

internal sealed record FakeSubjectSet(IObject Object, IString Relation) : ISubjectSet;

internal sealed record FakeTypedWildcard(IString Type) : ITypedWildcard;

internal sealed record UnknownSubject : ISubject;
