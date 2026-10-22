using System.Collections;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

/// <summary>
/// Reads a hash through its non-generic <see cref="IEnumerable"/> interface.
/// </summary>
internal sealed record UntypedBytes : IEnumerable<byte>
{
    private readonly IEnumerable _source;

    public UntypedBytes(IEnumerable source)
    {
        _source = source;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        foreach (object item in _source)
        {
            yield return (byte)item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
