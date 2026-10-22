# Changelog

All notable changes to Pure.Authorization.OpenFGA.Model.HashCodes are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [Unreleased]

### Added
- **One deterministic hash per abstraction interface**, each a `sealed record`
  implementing `IDeterminedHash` with its own 16-byte type prefix:
  `AuthorizationModelHash`, `TypeDefinitionHash`, `RelationDefinitionHash`,
  `DirectHash`, `ComputedRelationHash`, `RelationFromHash`, `UnionHash`,
  `IntersectionHash`, `DifferenceHash`, `TypeFormHash`, `SubjectSetFormHash`,
  `WildcardFormHash`, `ConditionalFormHash`, `ConditionHash`,
  `ConditionParameterHash`, `ConditionParameterTypeHash`, `ObjectHash`,
  `SubjectSetHash`, `TypedWildcardHash`, `RelationshipConditionHash` and
  `ConditionalTupleHash`.
- **Dispatching hashes** `RewriteHash`, `SubjectFormHash` and `SubjectHash`,
  which yield exactly the bytes of the concrete variant's hash and throw
  `ArgumentException` for an unknown variant.
- **`TupleHash`**: an `IConditionalTuple` hashes as `ConditionalTupleHash`, and
  a plain `ITuple` hashes without a condition.
- Order-insensitive hashing of set-like collections (model types and
  conditions, type relations, direct forms, union and intersection children,
  condition parameters). Difference operands and generic type arguments stay
  ordered.
- Canonical JSON hashing of `IRelationshipCondition.Context`: keys are sorted
  by ordinal, arrays keep their order, and a `null` value differs from an
  absent property.
