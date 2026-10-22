using System.Collections;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Internals;

/// <summary>
/// A hash whose bytes are computed once, when it is created, and replayed on every
/// enumeration.
/// </summary>
internal sealed record MaterializedHash : IDeterminedHash
{
    private readonly byte[] _bytes;

    public MaterializedHash(IDeterminedHash hash)
    {
        _bytes = [.. hash];
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return _bytes.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
