using Domain.Entities;
using Server.Core.Services.Abstraction;
using Server.Core.Tools;

namespace Server.Core.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly ServerDBContext _context;
    
    public TokenService(ServerDBContext context)
    {
        _context = context;
    }
    
    public string Generate(string description)
    {
        var token = new Token
        {
            Description = description ?? throw new ArgumentNullException(nameof(description)),
            CreationTime = DateTime.Now,
            TokenStr = GenerateTokenString(),
        };
        _context.Tokens.Add(token);
        _context.SaveChanges();
        return token.TokenStr;
    }

    public void Refuse(Guid id)
    {
        var token = _context.Tokens.FirstOrDefault(t => t.Id == id);
        if (token is null)
            throw new ServiceException($"There is no token with such id: '{id}'");
        _context.Tokens.Remove(token);
        _context.SaveChanges();
    }

    public bool Check(string tokenStr)
    {
        return _context.Tokens.FirstOrDefault(t => t.TokenStr.Equals(tokenStr)) is not null;
    }

    public List<Token> List()
    {
        return _context.Tokens.ToList();
    }

    private string GenerateTokenString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
 
        var random       = new Random();
        var randomString = new string(Enumerable.Repeat(chars, 32)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return randomString;
    }
}