using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Internals;

public sealed record MaterializedHashTests
{
    [Fact]
    public void YieldsSourceBytes()
    {
        Assert.Equal<byte>([3, 1, 2], new MaterializedHash(new CountingHash(3, 1, 2)));
    }

    [Fact]
    public void EnumeratesSourceOnce()
    {
        CountingHash source = new CountingHash(3, 1, 2);
        MaterializedHash hash = new MaterializedHash(source);

        _ = hash.ToArray();
        _ = hash.ToArray();

        Assert.Equal(1, source.Enumerations);
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal<byte>(
            [3, 1, 2],
            new UntypedBytes(new MaterializedHash(new CountingHash(3, 1, 2)))
        );
    }
}
