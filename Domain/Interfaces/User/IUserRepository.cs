using Domain.Entities.User;
using System.Threading.Tasks;

namespace Domain.Interfaces.User;

public interface IUserRepository
{
    global::System.Threading.Tasks.Task<Entities.User.User?> GetByIdAsync(Guid id);
    global::System.Threading.Tasks.Task<Entities.User.User?> GetByEmailAsync(string email);
    global::System.Threading.Tasks.Task<Entities.User.User?> GetByUsernameAsync(string username);
    global::System.Threading.Tasks.Task<bool> EmailExistsAsync(string email);
    global::System.Threading.Tasks.Task<bool> UsernameExistsAsync(string username);
    global::System.Threading.Tasks.Task AddAsync(Entities.User.User user);
    global::System.Threading.Tasks.Task UpdateAsync(Entities.User.User user);
    global::System.Threading.Tasks.Task DeleteAsync(Guid id);
}
