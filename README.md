# Pure.Authorization.OpenFGA.Model.HashCodes

Deterministic hash code implementations for OpenFGA authorization model abstractions in the **Pure** ecosystem.

[![.NET build & test](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.HashCodes/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.Authorization.OpenFGA.Model.HashCodes)](https://www.nuget.org/packages/Pure.Authorization.OpenFGA.Model.HashCodes)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> **Status:** early development. The public API is coming soon and no package has been published yet.

## Overview

`Pure.Authorization.OpenFGA.Model.HashCodes` will provide deterministic, byte-enumerable hash codes for every type in the [`Pure.Authorization.OpenFGA.Model.Abstractions`](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.Abstractions) layer. Following the approach of [`Pure.Chart.Model.HashCodes`](https://github.com/kudima03/Pure.Chart.Model.HashCodes), each type will wrap one abstraction interface and produce a stable byte sequence by prepending a fixed 16-byte type-discriminator prefix before hashing the object's fields with `Pure.HashCodes.DeterminedHash`. The resulting sequences enable structural equality checks and content-addressed storage of OpenFGA authorization models and relationship tuples.

## API

Coming soon. The planned surface is:

- **One hash per interface** — a `sealed record` implementing `IDeterminedHash` (which extends `IEnumerable<byte>`) for each interface in `Pure.Authorization.OpenFGA.Model.Abstractions`.
- **Dispatching hashes for variant base types** — `IRewrite`, `ISubject` and `ISubjectForm` have several concrete variants. Their hashes will dispatch to the hash of the specific variant, so two values of different variants never collide even when their fields match.

All hash types will live in the `Pure.Authorization.OpenFGA.Model.HashCodes` namespace.

## Dependencies

Planned:

- [`Pure.Authorization.OpenFGA.Model.Abstractions`](https://github.com/kudima03/Pure.Authorization.OpenFGA.Model.Abstractions) — OpenFGA authorization model interfaces
- [`Pure.HashCodes`](https://github.com/kudima03/Pure.HashCodes) — deterministic, byte-enumerable hash computation over Pure primitives

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

Coming soon.
