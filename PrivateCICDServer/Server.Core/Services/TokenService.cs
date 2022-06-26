using Domain.Entities;
using Domain.Services;
using Domain.Tools;

namespace Server.Core.Services;

public class TokenService : ITokenService
{
    private readonly ServerDbContext _context;

    public TokenService(ServerDbContext context)
    {
        _context = context;
    }

    public string Generate(string description)
    {
        var token = new Token(GenerateTokenString(), description);
        _context.Tokens.Add(token);
        _context.SaveChanges();
        return token.TokenStr;
    }

    public void Edit(Guid id, string description)
    {
        var token = _context.Tokens.GetById(id);
        token.Description = description;
        _context.Update(token);
        _context.SaveChanges();
    }

    public void Refuse(Guid id)
    {
        var token = _context.Tokens.GetById(id);
        _context.Tokens.Remove(token);
        _context.SaveChanges();
    }

    public bool Check(string tokenStr)
    {
        return FindByTokenString(tokenStr) is not null;
    }

    public Token? FindByTokenString(string tokenStr)
    {
        return _context.Tokens.FirstOrDefault(t => t.TokenStr.Equals(tokenStr));
    }

    public List<Token> List()
    {
        return _context.Tokens.ToList();
    }

    private string GenerateTokenString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var random = new Random();
        var randomString = new string(Enumerable.Repeat(chars, 32)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return randomString;
    }
}