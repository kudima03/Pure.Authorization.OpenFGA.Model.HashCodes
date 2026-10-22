using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record DirectHashTests
{
    private static readonly byte[] TypePrefix =
    [
        48,
        242,
        194,
        99,
        44,
        133,
        192,
        28,
        218,
        181,
        145,
        184,
        39,
        42,
        112,
        220,
    ];

    private static readonly ISubjectForm User = new FakeTypeForm(new String("user"));

    private static readonly ISubjectForm GroupMember = new FakeSubjectSetForm(
        new String("group"),
        new String("member")
    );

    private static readonly ISubjectForm AnyUser = new FakeWildcardForm(
        new String("user")
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash([
                    new SubjectFormHash(User),
                    new SubjectFormHash(GroupMember),
                ]),
            ]),
            new DirectHash(new FakeDirect([User, GroupMember]))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeDirect direct = new FakeDirect([User, GroupMember]);

        Assert.Equal(new DirectHash(direct), new UntypedBytes(new DirectHash(direct)));
    }

    [Fact]
    public void EqualForEqualForms()
    {
        Assert.Equal<byte>(
            new DirectHash(new FakeDirect([User, GroupMember])),
            new DirectHash(
                new FakeDirect([
                    new FakeTypeForm(new String("user")),
                    new FakeSubjectSetForm(new String("group"), new String("member")),
                ])
            )
        );
    }

    [Fact]
    public void FormsOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new DirectHash(new FakeDirect([User, GroupMember, AnyUser])),
            new DirectHash(new FakeDirect([AnyUser, User, GroupMember]))
        );
    }

    [Fact]
    public void DiffersByForms()
    {
        Assert.NotEqual<byte>(
            new DirectHash(new FakeDirect([User, GroupMember])),
            new DirectHash(new FakeDirect([User, AnyUser]))
        );
    }

    [Fact]
    public void DiffersByFormCount()
    {
        Assert.NotEqual<byte>(
            new DirectHash(new FakeDirect([User])),
            new DirectHash(new FakeDirect([User, User]))
        );
    }

    [Fact]
    public void EmptyDiffersFromNonEmpty()
    {
        Assert.NotEqual<byte>(
            new DirectHash(new FakeDirect([])),
            new DirectHash(new FakeDirect([User]))
        );
    }
}
