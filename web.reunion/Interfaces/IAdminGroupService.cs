using web.reunion.Models;

namespace web.reunion.Interfaces
{
    public interface IAdminGroupService
    {
        Task<List<AdminGroup>> GetAdminGroupsAsync();
        Task<AdminGroup> GetAdminGroupAsync(int id);
        Task PostAdminGroupAsync(AdminGroup adminGroup);
        Task PutAdminGroupAsync(AdminGroup adminGroup);
        Task DeleteAdminGroupAsync(int id);
    }
}
