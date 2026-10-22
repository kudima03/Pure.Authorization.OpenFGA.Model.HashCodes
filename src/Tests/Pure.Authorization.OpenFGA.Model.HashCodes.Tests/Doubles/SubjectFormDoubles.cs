using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeTypeForm(IString Type) : ITypeForm;

internal sealed record FakeSubjectSetForm(IString Type, IString Relation)
    : ISubjectSetForm;

internal sealed record FakeWildcardForm(IString Type) : IWildcardForm;

internal sealed record FakeConditionalForm(ISubjectForm Form, IString Condition)
    : IConditionalForm;

internal sealed record UnknownSubjectForm : ISubjectForm;
