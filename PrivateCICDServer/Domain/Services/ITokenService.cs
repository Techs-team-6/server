using Domain.Entities;

namespace Domain.Services;

public interface ITokenService
{
    /// <summary>
    /// Генерирует новый токен
    /// </summary>
    /// <param name="description">Описание токена</param>
    /// <returns>Возращает токен-строку</returns>
    string Generate(string description);
    
    /// <summary>
    /// Редактирование полей токена
    /// </summary>
    /// <param name="id">ID токена, который нужно изменить</param>
    /// <param name="description">Новое описание токена</param>
    void Edit(Guid id, string description);
    
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
    /// Находит токен по токен-строке
    /// </summary>
    /// <param name="tokenStr">Токен-строка</param>
    /// <returns>Возвращает токен, если он был найден, иначе null</returns>
    Token? FindByTokenString(string tokenStr);
    
    /// <summary>
    /// Показывает список токенов
    /// </summary>
    /// <returns>Список токенов</returns>
    List<Token> List();
}