using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeCondition(
    IString Name,
    IString Expression,
    IEnumerable<IConditionParameter> Parameters
) : ICondition;

internal sealed record FakeConditionParameter(IString Name, IConditionParameterType Type)
    : IConditionParameter;

internal sealed record FakeConditionParameterType(
    IString Name,
    IEnumerable<IConditionParameterType> GenericTypes
) : IConditionParameterType;
