using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task UpdateAsync(ApplicationUser user);
}