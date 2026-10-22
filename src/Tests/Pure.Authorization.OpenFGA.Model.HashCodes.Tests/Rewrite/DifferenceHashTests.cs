using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record DifferenceHashTests
{
    private static readonly byte[] TypePrefix =
    [
        73,
        241,
        9,
        101,
        103,
        121,
        180,
        113,
        48,
        6,
        50,
        80,
        81,
        45,
        137,
        52,
    ];

    private static readonly IRewrite Viewer = new FakeComputedRelation(
        new String("viewer")
    );

    private static readonly IRewrite Blocked = new FakeComputedRelation(
        new String("blocked")
    );

    private static readonly IRewrite Owner = new FakeComputedRelation(
        new String("owner")
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new RewriteHash(Viewer),
                .. new RewriteHash(Blocked),
            ]),
            new DifferenceHash(new FakeDifference(Viewer, Blocked))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeDifference difference = new FakeDifference(Viewer, Blocked);

        Assert.Equal(
            new DifferenceHash(difference),
            new UntypedBytes(new DifferenceHash(difference))
        );
    }

    [Fact]
    public void EqualForEqualOperands()
    {
        Assert.Equal<byte>(
            new DifferenceHash(new FakeDifference(Viewer, Blocked)),
            new DifferenceHash(
                new FakeDifference(
                    new FakeComputedRelation(new String("viewer")),
                    new FakeComputedRelation(new String("blocked"))
                )
            )
        );
    }

    [Fact]
    public void OperandOrderMatters()
    {
        Assert.NotEqual<byte>(
            new DifferenceHash(new FakeDifference(Viewer, Blocked)),
            new DifferenceHash(new FakeDifference(Blocked, Viewer))
        );
    }

    [Fact]
    public void DiffersByBase()
    {
        Assert.NotEqual<byte>(
            new DifferenceHash(new FakeDifference(Viewer, Blocked)),
            new DifferenceHash(new FakeDifference(Owner, Blocked))
        );
    }

    [Fact]
    public void DiffersBySubtract()
    {
        Assert.NotEqual<byte>(
            new DifferenceHash(new FakeDifference(Viewer, Blocked)),
            new DifferenceHash(new FakeDifference(Viewer, Owner))
        );
    }
}
