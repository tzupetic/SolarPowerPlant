using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.Data;

namespace SolarPowerPlant.Users;

public class UserService
{
    private readonly PowerPlantContext _context;

    public UserService(PowerPlantContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        var newUser = _context.Users.Add(user).Entity;
        await _context.SaveChangesAsync();

        return newUser;
    }
}
