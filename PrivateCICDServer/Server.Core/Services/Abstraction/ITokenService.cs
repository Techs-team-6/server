using Domain.Entities;

namespace Server.Core.Services.Abstraction;

public interface ITokenService
{
    /// <summary>
    /// Генерирует новый токен
    /// </summary>
    /// <param name="description">Описание токена</param>
    /// <returns>Возращает токен-строку</returns>
    string Generate(string description);
    
    /// <summary>
    /// Отзывает токен
    /// </summary>
    /// <param name="id">ID токена, который нужно отозвать</param>
    void Refuse(Guid id);
    
    /// <summary>
    /// Проверяет существование токена
    /// </summary>
    /// <param name="tokenStr">Токен-строка</param>
    /// <returns>Возвращает true, если токен существует</returns>
    bool Check(string tokenStr);
    
    /// <summary>
    /// Показывает список токенов
    /// </summary>
    /// <returns>Список токенов</returns>
    List<Token> List();
}