using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeDirect(IEnumerable<ISubjectForm> Forms) : IDirect;

internal sealed record FakeComputedRelation(IString Relation) : IComputedRelation;

internal sealed record FakeRelationFrom(IString Relation, IString Tupleset)
    : IRelationFrom;

internal sealed record FakeUnion(IEnumerable<IRewrite> Children) : IUnion;

internal sealed record FakeIntersection(IEnumerable<IRewrite> Children) : IIntersection;

internal sealed record FakeDifference(IRewrite Base, IRewrite Subtract) : IDifference;

internal sealed record UnknownRewrite : IRewrite;
