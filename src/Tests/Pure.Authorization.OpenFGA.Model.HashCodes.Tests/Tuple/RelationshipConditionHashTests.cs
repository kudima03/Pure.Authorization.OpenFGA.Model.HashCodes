using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Tuple;

public sealed record RelationshipConditionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        70,
        122,
        123,
        228,
        28,
        240,
        28,
        112,
        185,
        202,
        96,
        68,
        106,
        39,
        125,
        181,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("non_expired")),
                .. SHA256.HashData(
                    Encoding.UTF8.GetBytes(
                        /*lang=json,strict*/
                        """{"duration":"1h","grant_time":"2026-01-01T00:00:00Z"}"""
                    )
                ),
            ]),
            Hash(
                "non_expired",
                /*lang=json,strict*/
                """{"grant_time":"2026-01-01T00:00:00Z","duration":"1h"}"""
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeRelationshipCondition condition = new FakeRelationshipCondition(
            new String("non_expired"),
            JsonNode.Parse("""{"duration":"1h"}""")!
        );

        Assert.Equal(
            new RelationshipConditionHash(condition),
            new UntypedBytes(new RelationshipConditionHash(condition))
        );
    }

    [Fact]
    public void EqualForEqualConditions()
    {
        Assert.Equal<byte>(
            Hash(
                "non_expired", /*lang=json,strict*/
                """{"duration":"1h"}"""
            ),
            Hash(
                "non_expired", /*lang=json,strict*/
                """{"duration":"1h"}"""
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            Hash(
                "non_expired", /*lang=json,strict*/
                """{"duration":"1h"}"""
            ),
            Hash(
                "in_office", /*lang=json,strict*/
                """{"duration":"1h"}"""
            )
        );
    }

    [Fact]
    public void DiffersByContext()
    {
        Assert.NotEqual<byte>(
            Hash(
                "non_expired", /*lang=json,strict*/
                """{"duration":"1h"}"""
            ),
            Hash(
                "non_expired", /*lang=json,strict*/
                """{"duration":"2h"}"""
            )
        );
    }

    [Fact]
    public void PropertyOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1,"b":{"x":[{"p":1,"q":2}],"y":2}}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                """{"b":{"y":2,"x":[{"q":2,"p":1}]},"a":1}"""
            )
        );
    }

    [Fact]
    public void WhitespaceDoesNotMatter()
    {
        Assert.Equal<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":[1,2]}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                "{ \"a\" : [ 1, 2 ] }"
            )
        );
    }

    [Fact]
    public void ArrayOrderMatters()
    {
        Assert.NotEqual<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":[1,2]}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                """{"a":[2,1]}"""
            )
        );
    }

    [Fact]
    public void NullValueDiffersFromAbsentProperty()
    {
        Assert.NotEqual<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1,"b":null}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1}"""
            )
        );
    }

    [Fact]
    public void NumbersCompareByJsonText()
    {
        Assert.NotEqual<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1.0}"""
            )
        );
    }

    [Fact]
    public void StringDiffersFromNumber()
    {
        Assert.NotEqual<byte>(
            Hash(
                "c", /*lang=json,strict*/
                """{"a":1}"""
            ),
            Hash(
                "c", /*lang=json,strict*/
                """{"a":"1"}"""
            )
        );
    }

    private static RelationshipConditionHash Hash(string name, string context)
    {
        return new RelationshipConditionHash(
            new FakeRelationshipCondition(new String(name), JsonNode.Parse(context)!)
        );
    }
}
