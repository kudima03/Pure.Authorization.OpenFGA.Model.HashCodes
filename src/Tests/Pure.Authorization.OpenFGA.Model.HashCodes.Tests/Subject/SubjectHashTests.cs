using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Subject;

public sealed record SubjectHashTests
{
    [Fact]
    public void DispatchesObject()
    {
        FakeObject user = new FakeObject(new String("user"), new String("anne"));

        Assert.Equal(new ObjectHash(user), new SubjectHash(user));
    }

    [Fact]
    public void DispatchesSubjectSet()
    {
        FakeSubjectSet set = new FakeSubjectSet(
            new FakeObject(new String("group"), new String("1")),
            new String("member")
        );

        Assert.Equal(new SubjectSetHash(set), new SubjectHash(set));
    }

    [Fact]
    public void DispatchesTypedWildcard()
    {
        FakeTypedWildcard wildcard = new FakeTypedWildcard(new String("user"));

        Assert.Equal(new TypedWildcardHash(wildcard), new SubjectHash(wildcard));
    }

    [Fact]
    public void ThrowsForUnknownVariant()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            new SubjectHash(new UnknownSubject()).ToArray()
        );

        Assert.Contains(
            typeof(UnknownSubject).FullName!,
            exception.Message,
            StringComparison.Ordinal
        );
        Assert.Contains("ISubject", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeObject user = new FakeObject(new String("user"), new String("anne"));

        Assert.Equal(new ObjectHash(user), new UntypedBytes(new SubjectHash(user)));
    }
}
