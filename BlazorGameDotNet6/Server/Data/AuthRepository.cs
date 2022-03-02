﻿namespace BlazorGameDotNet6.Server.Data;

public class AuthRepository : IAuthRepository
{
    public DataContext _context;
    public IConfiguration _configuration;

    public AuthRepository(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
        if (user == null)
        {
            response.Success = false;
            response.Message = Constants.AuthMessages.UserNotFound;
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = Constants.AuthMessages.WrongPassword;
        }
        else
        {
            response.Success = true;
            response.Data = CreateToken(user);
        }
        return response;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password, int startUnitId)
    {
        if (await EmailExists(user.Email))
        {
            return new ServiceResponse<int> { Success = false, Message = Constants.AuthMessages.UserExistsEmail };
        }
        if (await UsernameExists(user.Username))
        {
            return new ServiceResponse<int> { Success = false, Message = Constants.AuthMessages.UserExistsUsername };
        }

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await AddStartingUnit(user, startUnitId);

        return new ServiceResponse<int> { Success = true, Data = user.Id, Message = Constants.AuthMessages.SuccessfulRegistration };
    }

    private async Task AddStartingUnit(User user, int startUnitId)
    {
        var unit = await _context.Units.FindAsync(startUnitId);
        _context.UserUnits.Add(new UserUnit
        {
            UnitId = unit!.Id, // 'units' table is handled by admin, therefore we can presume the record exists
            UserId = user.Id,
            HitPoints = unit.HitPoints,
        });

        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExists(string email) => await _context.Users.AnyAsync(s => s.Email.ToLower().Equals(email.ToLower()));

    public async Task<bool> UsernameExists(string username) => await _context.Users.AnyAsync(s => s.Username.ToLower().Equals(username.ToLower()));

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            // check if the passwords are equal on byte level
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }
        return true;
    }

    private string CreateToken(User user)
    {
        // security key from appsettings.json
        // create string json web token
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Appsettings:Token").Value)), 
                SecurityAlgorithms.HmacSha512Signature)));
    }
}
