using System.Collections;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

/// <summary>
/// A fixed hash that counts how many times it has been enumerated.
/// </summary>
internal sealed class CountingHash(params byte[] bytes) : IDeterminedHash
{
    private readonly byte[] _bytes = bytes;

    public int Enumerations { get; private set; }

    public IEnumerator<byte> GetEnumerator()
    {
        Enumerations++;
        return _bytes.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
