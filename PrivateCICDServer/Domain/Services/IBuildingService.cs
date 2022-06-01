namespace Domain.Services;

public interface IBuildingService
{
    /// <summary>
    /// Создаёт репозиторий для проекта 
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    /// <param name="projectName">Имя проекта</param>
    /// <returns>Возвращает ссылку на репозиторий</returns>
    string CreateProject(Guid projectId, string projectName);
}