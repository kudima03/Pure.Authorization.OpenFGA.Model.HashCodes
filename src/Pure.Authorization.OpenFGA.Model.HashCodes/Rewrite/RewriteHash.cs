using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of any <see cref="IRewrite"/>. It dispatches to the hash of the
/// concrete variant and yields exactly the same bytes, so <c>new RewriteHash(union)</c>
/// equals <c>new UnionHash(union)</c>. Enumerating it throws
/// <see cref="ArgumentException"/> for an implementation outside the closed set of
/// variants.
/// </summary>
public sealed record RewriteHash : IDeterminedHash
{
    private readonly IRewrite _rewrite;

    public RewriteHash(IRewrite rewrite)
    {
        _rewrite = rewrite;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        IDeterminedHash hash = _rewrite switch
        {
            IDirect direct => new DirectHash(direct),
            IComputedRelation computedRelation => new ComputedRelationHash(
                computedRelation
            ),
            IRelationFrom relationFrom => new RelationFromHash(relationFrom),
            IUnion union => new UnionHash(union),
            IIntersection intersection => new IntersectionHash(intersection),
            IDifference difference => new DifferenceHash(difference),
            _ => throw new ArgumentException(
                $"{_rewrite.GetType()} is not a known IRewrite variant."
            ),
        };
        return hash.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
