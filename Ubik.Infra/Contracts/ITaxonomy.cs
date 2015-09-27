namespace Ubik.Infra.Contracts
{
    public interface ITaxonomy<out TKey> : ITaxonomy
    {
        TKey Id { get; }

        TKey ParentId { get; }
    }

    public interface ITaxonomy
    {
        int Depth { get; }
    }
}