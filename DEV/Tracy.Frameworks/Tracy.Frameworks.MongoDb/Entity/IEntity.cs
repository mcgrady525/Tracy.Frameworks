namespace Tracy.Frameworks.MongoDb
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}