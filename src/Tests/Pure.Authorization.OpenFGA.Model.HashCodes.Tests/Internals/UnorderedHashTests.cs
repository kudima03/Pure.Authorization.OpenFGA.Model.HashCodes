using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Internals;

public sealed record UnorderedHashTests
{
    [Fact]
    public void MatchesSortingAggregate()
    {
        Assert.Equal(
            new DeterminedHash([new CountingHash(2, 0), new CountingHash(1, 9)]),
            new UnorderedHash([new CountingHash(2, 0), new CountingHash(1, 9)])
        );
    }

    [Fact]
    public void OrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new UnorderedHash([
                new CountingHash(2, 0),
                new CountingHash(1, 9),
                new CountingHash(5),
            ]),
            new UnorderedHash([
                new CountingHash(5),
                new CountingHash(2, 0),
                new CountingHash(1, 9),
            ])
        );
    }

    [Fact]
    public void EnumeratesEachElementOnce()
    {
        IReadOnlyCollection<CountingHash> elements =
        [
            new CountingHash(4),
            new CountingHash(2),
            new CountingHash(3),
            new CountingHash(1),
        ];

        _ = new UnorderedHash(elements).ToArray();

        Assert.All(elements, x => Assert.Equal(1, x.Enumerations));
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal(
            new UnorderedHash([new CountingHash(2, 0), new CountingHash(1, 9)]),
            new UntypedBytes(
                new UnorderedHash([new CountingHash(2, 0), new CountingHash(1, 9)])
            )
        );
    }
}
