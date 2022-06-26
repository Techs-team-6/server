namespace Domain.Tools;

public static class GetByIdExtension
{
    public static T GetById<T>(this IEnumerable<T> collection, Guid id) where T : IHasId
    {
        return collection.FirstOrDefault(i => i.Id == id) ??
               throw new EntityNotFoundException(nameof(T), id);
    }
}