using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.SubjectForm;

public sealed record SubjectFormHashTests
{
    [Fact]
    public void DispatchesTypeForm()
    {
        FakeTypeForm form = new FakeTypeForm(new String("user"));

        Assert.Equal(new TypeFormHash(form), new SubjectFormHash(form));
    }

    [Fact]
    public void DispatchesSubjectSetForm()
    {
        FakeSubjectSetForm form = new FakeSubjectSetForm(
            new String("group"),
            new String("member")
        );

        Assert.Equal(new SubjectSetFormHash(form), new SubjectFormHash(form));
    }

    [Fact]
    public void DispatchesWildcardForm()
    {
        FakeWildcardForm form = new FakeWildcardForm(new String("user"));

        Assert.Equal(new WildcardFormHash(form), new SubjectFormHash(form));
    }

    [Fact]
    public void DispatchesConditionalForm()
    {
        FakeConditionalForm form = new FakeConditionalForm(
            new FakeSubjectSetForm(new String("group"), new String("member")),
            new String("non_expired")
        );

        Assert.Equal(new ConditionalFormHash(form), new SubjectFormHash(form));
    }

    [Fact]
    public void ThrowsForUnknownVariant()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            new SubjectFormHash(new UnknownSubjectForm()).ToArray()
        );

        Assert.Contains(
            typeof(UnknownSubjectForm).FullName!,
            exception.Message,
            StringComparison.Ordinal
        );
        Assert.Contains("ISubjectForm", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeTypeForm form = new FakeTypeForm(new String("user"));

        Assert.Equal(new TypeFormHash(form), new UntypedBytes(new SubjectFormHash(form)));
    }
}
