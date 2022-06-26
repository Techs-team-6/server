namespace Domain.Tools;

public class EntityNotFoundException : ServiceException
{
    public EntityNotFoundException(string entityName, Guid id)
        : base($"There is no {entityName} with id '{id}'")
    {
    }
}