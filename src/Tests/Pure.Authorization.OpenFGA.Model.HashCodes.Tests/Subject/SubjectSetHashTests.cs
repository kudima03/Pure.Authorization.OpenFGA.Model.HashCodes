using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Subject;

public sealed record SubjectSetHashTests
{
    private static readonly byte[] TypePrefix =
    [
        128,
        202,
        156,
        201,
        228,
        107,
        177,
        88,
        136,
        132,
        202,
        19,
        153,
        241,
        68,
        238,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        FakeObject group = new FakeObject(new String("group"), new String("1"));

        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new ObjectHash(group),
                .. new DeterminedHash(new String("member")),
            ]),
            new SubjectSetHash(new FakeSubjectSet(group, new String("member")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeSubjectSet set = new FakeSubjectSet(
            new FakeObject(new String("group"), new String("1")),
            new String("member")
        );

        Assert.Equal(new SubjectSetHash(set), new UntypedBytes(new SubjectSetHash(set)));
    }

    [Fact]
    public void EqualForEqualSubjectSets()
    {
        Assert.Equal<byte>(
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("1")),
                    new String("member")
                )
            ),
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("1")),
                    new String("member")
                )
            )
        );
    }

    [Fact]
    public void DiffersByObject()
    {
        Assert.NotEqual<byte>(
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("1")),
                    new String("member")
                )
            ),
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("2")),
                    new String("member")
                )
            )
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("1")),
                    new String("member")
                )
            ),
            new SubjectSetHash(
                new FakeSubjectSet(
                    new FakeObject(new String("group"), new String("1")),
                    new String("owner")
                )
            )
        );
    }
}
