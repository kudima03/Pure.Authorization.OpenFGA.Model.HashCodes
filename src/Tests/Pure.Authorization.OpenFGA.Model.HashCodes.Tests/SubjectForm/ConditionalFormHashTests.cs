using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.SubjectForm;

public sealed record ConditionalFormHashTests
{
    private static readonly byte[] TypePrefix =
    [
        33,
        254,
        23,
        91,
        80,
        84,
        50,
        139,
        201,
        85,
        77,
        70,
        153,
        41,
        217,
        93,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        FakeTypeForm user = new FakeTypeForm(new String("user"));

        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new SubjectFormHash(user),
                .. new DeterminedHash(new String("non_expired")),
            ]),
            new ConditionalFormHash(
                new FakeConditionalForm(user, new String("non_expired"))
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeConditionalForm form = new FakeConditionalForm(
            new FakeTypeForm(new String("user")),
            new String("non_expired")
        );

        Assert.Equal(
            new ConditionalFormHash(form),
            new UntypedBytes(new ConditionalFormHash(form))
        );
    }

    [Fact]
    public void EqualForEqualForms()
    {
        Assert.Equal<byte>(
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeTypeForm(new String("user")),
                    new String("non_expired")
                )
            ),
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeTypeForm(new String("user")),
                    new String("non_expired")
                )
            )
        );
    }

    [Fact]
    public void DiffersByForm()
    {
        Assert.NotEqual<byte>(
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeTypeForm(new String("user")),
                    new String("non_expired")
                )
            ),
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeWildcardForm(new String("user")),
                    new String("non_expired")
                )
            )
        );
    }

    [Fact]
    public void DiffersByCondition()
    {
        Assert.NotEqual<byte>(
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeTypeForm(new String("user")),
                    new String("non_expired")
                )
            ),
            new ConditionalFormHash(
                new FakeConditionalForm(
                    new FakeTypeForm(new String("user")),
                    new String("in_office")
                )
            )
        );
    }

    [Fact]
    public void DiffersFromUnconditionedForm()
    {
        FakeTypeForm user = new FakeTypeForm(new String("user"));

        Assert.NotEqual(
            new SubjectFormHash(user),
            new ConditionalFormHash(
                new FakeConditionalForm(user, new String("non_expired"))
            )
        );
    }
}
