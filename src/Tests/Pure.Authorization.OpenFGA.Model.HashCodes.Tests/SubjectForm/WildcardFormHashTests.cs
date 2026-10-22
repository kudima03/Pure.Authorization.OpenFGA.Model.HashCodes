using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.SubjectForm;

public sealed record WildcardFormHashTests
{
    private static readonly byte[] TypePrefix =
    [
        229,
        119,
        237,
        185,
        76,
        129,
        124,
        153,
        123,
        175,
        151,
        148,
        102,
        67,
        239,
        225,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([.. TypePrefix, .. new DeterminedHash(new String("user"))]),
            new WildcardFormHash(new FakeWildcardForm(new String("user")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal(
            new WildcardFormHash(new FakeWildcardForm(new String("user"))),
            new UntypedBytes(
                new WildcardFormHash(new FakeWildcardForm(new String("user")))
            )
        );
    }

    [Fact]
    public void EqualForEqualForms()
    {
        Assert.Equal<byte>(
            new WildcardFormHash(new FakeWildcardForm(new String("user"))),
            new WildcardFormHash(new FakeWildcardForm(new String("user")))
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new WildcardFormHash(new FakeWildcardForm(new String("user"))),
            new WildcardFormHash(new FakeWildcardForm(new String("group")))
        );
    }
}
