using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.SubjectForm;

public sealed record SubjectSetFormHashTests
{
    private static readonly byte[] TypePrefix =
    [
        246,
        251,
        109,
        104,
        6,
        0,
        112,
        235,
        254,
        253,
        123,
        35,
        245,
        221,
        9,
        132,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("group")),
                .. new DeterminedHash(new String("member")),
            ]),
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("member"))
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeSubjectSetForm form = new FakeSubjectSetForm(
            new String("group"),
            new String("member")
        );

        Assert.Equal(
            new SubjectSetFormHash(form),
            new UntypedBytes(new SubjectSetFormHash(form))
        );
    }

    [Fact]
    public void EqualForEqualForms()
    {
        Assert.Equal<byte>(
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("member"))
            ),
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("member"))
            )
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("member"))
            ),
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("team"), new String("member"))
            )
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("member"))
            ),
            new SubjectSetFormHash(
                new FakeSubjectSetForm(new String("group"), new String("owner"))
            )
        );
    }
}
