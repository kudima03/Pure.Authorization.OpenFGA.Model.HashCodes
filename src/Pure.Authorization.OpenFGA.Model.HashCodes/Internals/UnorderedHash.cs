using System.Collections;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Internals;

/// <summary>
/// The order-insensitive hash of a set of element hashes, combined by the sorting
/// aggregate of <see cref="DeterminedHash"/>. Each element is computed exactly once,
/// before sorting: the sort then compares ready bytes instead of recomputing the
/// elements on every comparison, and an exception raised while hashing an element
/// reaches the caller as is rather than wrapped by the sort.
/// </summary>
internal sealed record UnorderedHash : IDeterminedHash
{
    private readonly IEnumerable<IDeterminedHash> _hashes;

    public UnorderedHash(IEnumerable<IDeterminedHash> hashes)
    {
        _hashes = hashes;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            _hashes.Select(x => new MaterializedHash(x))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
