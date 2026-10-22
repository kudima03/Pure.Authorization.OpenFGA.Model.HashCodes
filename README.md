# Pure.Authorization.OpenFGA.Model.HashCodes

Deterministic hash code implementations for OpenFGA authorization model abstractions in the **Pure** ecosystem.

[![.NET build & test](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.Authorization.OpenFGA.Model.HashCodes)](https://www.nuget.org/packages/Pure.Authorization.OpenFGA.Model.HashCodes)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> **Status:** early development. The API below is implemented, but no package has been published yet.

## Overview

`Pure.Authorization.OpenFGA.Model.HashCodes` provides deterministic, byte-enumerable hash codes for every type in the [`Pure.Authorization.OpenFGA.Model.Abstractions`](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.Abstractions) layer. Following the approach of [`Pure.Chart.Model.HashCodes`](https://github.com/kudima03/Pure.Chart.Model.HashCodes), each type wraps one abstraction interface and produces a stable byte sequence by prepending a fixed 16-byte type-discriminator prefix before hashing the object's fields with `Pure.HashCodes.DeterminedHash`. The resulting sequences enable structural equality checks and content-addressed storage of OpenFGA authorization models and relationship tuples.

## API

All types are `sealed record` and implement `IDeterminedHash` (which extends `IEnumerable<byte>`). Hashes are computed lazily, when the bytes are enumerated. The namespaces mirror the folders of the abstractions: a hash of an interface from `Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite` lives in `Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite`, and so on.

### One hash per interface

Each of these hashes has its own 16-byte type prefix, so two values of different types never collide even when their fields match.

| Namespace | Type | Wraps | Hashed fields |
|---|---|---|---|
| `Model` | `AuthorizationModelHash` | `IAuthorizationModel` | SchemaVersion, Types *(set)*, Conditions *(set)* |
| `Model` | `TypeDefinitionHash` | `ITypeDefinition` | Name, Relations *(set)* |
| `Model` | `RelationDefinitionHash` | `IRelationDefinition` | Name, Rewrite |
| `Rewrite` | `DirectHash` | `IDirect` | Forms *(set)* |
| `Rewrite` | `ComputedRelationHash` | `IComputedRelation` | Relation |
| `Rewrite` | `RelationFromHash` | `IRelationFrom` | Relation, Tupleset |
| `Rewrite` | `UnionHash` | `IUnion` | Children *(set)* |
| `Rewrite` | `IntersectionHash` | `IIntersection` | Children *(set)* |
| `Rewrite` | `DifferenceHash` | `IDifference` | Base, Subtract *(ordered)* |
| `SubjectForm` | `TypeFormHash` | `ITypeForm` | Type |
| `SubjectForm` | `SubjectSetFormHash` | `ISubjectSetForm` | Type, Relation |
| `SubjectForm` | `WildcardFormHash` | `IWildcardForm` | Type |
| `SubjectForm` | `ConditionalFormHash` | `IConditionalForm` | Form, Condition |
| `Condition` | `ConditionHash` | `ICondition` | Name, Expression, Parameters *(set)* |
| `Condition` | `ConditionParameterHash` | `IConditionParameter` | Name, Type |
| `Condition` | `ConditionParameterTypeHash` | `IConditionParameterType` | Name, GenericTypes *(ordered)* |
| `Object` | `ObjectHash` | `IObject` | Type, Id |
| `Subject` | `SubjectSetHash` | `ISubjectSet` | Object, Relation |
| `Subject` | `TypedWildcardHash` | `ITypedWildcard` | Type |
| `Tuple` | `RelationshipConditionHash` | `IRelationshipCondition` | Name, Context *(canonical JSON)* |
| `Tuple` | `ConditionalTupleHash` | `IConditionalTuple` | Subject, Relation, Object, Condition |

### Dispatching hashes

`IRewrite`, `ISubjectForm` and `ISubject` are closed sets of variants, and `ITuple` may or may not carry a condition. Their hashes pick the hash of the concrete variant and yield exactly the same bytes, so `new SubjectHash(obj)` equals `new ObjectHash(obj)`.

| Namespace | Type | Wraps | Dispatches to |
|---|---|---|---|
| `Rewrite` | `RewriteHash` | `IRewrite` | `DirectHash`, `ComputedRelationHash`, `RelationFromHash`, `UnionHash`, `IntersectionHash`, `DifferenceHash` |
| `SubjectForm` | `SubjectFormHash` | `ISubjectForm` | `TypeFormHash`, `SubjectSetFormHash`, `WildcardFormHash`, `ConditionalFormHash` |
| `Subject` | `SubjectHash` | `ISubject` | `ObjectHash`, `SubjectSetHash`, `TypedWildcardHash` |
| `Tuple` | `TupleHash` | `ITuple` | `ConditionalTupleHash` for an `IConditionalTuple`. A plain tuple hashes its Subject, Relation and Object under the `TupleHash` prefix |

Enumerating `RewriteHash`, `SubjectFormHash` or `SubjectHash` over an implementation outside its closed set throws `ArgumentException`. This also holds when the unknown variant is nested anywhere inside a larger hash, including inside a set.

## Hashing rules

### Collection order

Collections that OpenFGA treats as sets are hashed **order-insensitively**. Each element is hashed once, then the element hashes are combined by `Pure.HashCodes`' sorting aggregate (`new DeterminedHash(IEnumerable<IDeterminedHash>)`):

- `IAuthorizationModel.Types` and `IAuthorizationModel.Conditions`
- `ITypeDefinition.Relations`
- `IDirect.Forms`
- `IUnion.Children` and `IIntersection.Children`
- `ICondition.Parameters`

Duplicates are not collapsed: `[a, a]` and `[a]` hash differently.

Order is **significant** where it carries meaning:

- `IDifference`: `a but not b` and `b but not a` hash differently. Base and Subtract are hashed in that order.
- `IConditionParameterType.GenericTypes`: type arguments keep their positions. The element hashes are concatenated in order, then hashed.

### Relationship condition context

`IRelationshipCondition.Context` is a `JsonNode`. It is hashed through a canonical JSON text, so semantically equal JSON hashes equally:

- Object properties are sorted by ordinal (`StringComparer.Ordinal`) key, recursively at every depth.
- Arrays keep their order.
- Primitives use their JSON text, so `1` and `1.0` hash differently, as do `1` and `"1"`.
- Strings are re-escaped uniformly, so `"\u00e9"` and `"é"` hash equally.
- Whitespace is dropped.
- A property with a JSON `null` value differs from an absent property.

The canonical form is produced with `JsonDocument` and `Utf8JsonWriter` only, so it does not use reflection-based serialization and stays AOT-safe.

## Dependencies

- [`Pure.Authorization.OpenFGA.Model.Abstractions`](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.Abstractions/tree/0.1.0-preview.0.1.0) — OpenFGA authorization model interfaces
- [`Pure.HashCodes`](https://github.com/kudima03/Pure.HashCodes/tree/2.1.0) — deterministic, byte-enumerable hash computation over Pure primitives

## Target Frameworks

- .NET 7
- .NET 8
- .NET 9
- .NET 10

## Installation

Not yet published. Once released:

```
dotnet add package Pure.Authorization.OpenFGA.Model.HashCodes
```

## Usage

```csharp
using Pure.Authorization.OpenFGA.Model.HashCodes.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;

IAuthorizationModel model = ...; // any IAuthorizationModel implementation
ITuple tuple = ...;              // any ITuple, with or without a condition

// Enumerate the hash bytes directly
byte[] modelHash = new AuthorizationModelHash(model).ToArray();

// Compare two models structurally, regardless of the order of their types
bool same = new AuthorizationModelHash(modelA).SequenceEqual(
    new AuthorizationModelHash(modelB)
);

// Hash a tuple; a conditional tuple includes its condition and context
byte[] tupleHash = new TupleHash(tuple).ToArray();
```
