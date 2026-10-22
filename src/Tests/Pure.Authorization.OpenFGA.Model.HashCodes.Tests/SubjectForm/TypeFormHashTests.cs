using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.SubjectForm;

public sealed record TypeFormHashTests
{
    private static readonly byte[] TypePrefix =
    [
        223,
        29,
        170,
        233,
        25,
        44,
        54,
        197,
        190,
        237,
        229,
        93,
        20,
        231,
        13,
        42,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([.. TypePrefix, .. new DeterminedHash(new String("user"))]),
            new TypeFormHash(new FakeTypeForm(new String("user")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal(
            new TypeFormHash(new FakeTypeForm(new String("user"))),
            new UntypedBytes(new TypeFormHash(new FakeTypeForm(new String("user"))))
        );
    }

    [Fact]
    public void EqualForEqualForms()
    {
        Assert.Equal<byte>(
            new TypeFormHash(new FakeTypeForm(new String("user"))),
            new TypeFormHash(new FakeTypeForm(new String("user")))
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new TypeFormHash(new FakeTypeForm(new String("user"))),
            new TypeFormHash(new FakeTypeForm(new String("group")))
        );
    }
}
