using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Object;

public sealed record ObjectHashTests
{
    private static readonly byte[] TypePrefix =
    [
        25,
        136,
        254,
        52,
        229,
        233,
        215,
        162,
        74,
        224,
        124,
        59,
        3,
        240,
        16,
        63,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("document")),
                .. new DeterminedHash(new String("1")),
            ]),
            new ObjectHash(new FakeObject(new String("document"), new String("1")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal(
            new ObjectHash(new FakeObject(new String("document"), new String("1"))),
            new UntypedBytes(
                new ObjectHash(new FakeObject(new String("document"), new String("1")))
            )
        );
    }

    [Fact]
    public void EqualForEqualObjects()
    {
        Assert.Equal<byte>(
            new ObjectHash(new FakeObject(new String("document"), new String("1"))),
            new ObjectHash(new FakeObject(new String("document"), new String("1")))
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new ObjectHash(new FakeObject(new String("document"), new String("1"))),
            new ObjectHash(new FakeObject(new String("folder"), new String("1")))
        );
    }

    [Fact]
    public void DiffersById()
    {
        Assert.NotEqual<byte>(
            new ObjectHash(new FakeObject(new String("document"), new String("1"))),
            new ObjectHash(new FakeObject(new String("document"), new String("2")))
        );
    }

    [Fact]
    public void DiffersWhenTypeAndIdSwap()
    {
        Assert.NotEqual<byte>(
            new ObjectHash(new FakeObject(new String("a"), new String("b"))),
            new ObjectHash(new FakeObject(new String("b"), new String("a")))
        );
    }
}
