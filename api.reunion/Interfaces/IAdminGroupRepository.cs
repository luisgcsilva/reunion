using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// AdminGroups Interface
    /// </summary>
    public interface IAdminGroupRepository
    {
        ICollection<AdminGroup> GetAdminGroups();
        bool CriarAdminGroup(AdminGroup adminGroup);
        AdminGroup GetAdminGroup(int id);
        bool AtualizarAdminGroup(AdminGroup adminGroup);
        bool AdminGroupExists(int id);
        bool ApagarAdminGroup(AdminGroup adminGroup);
        bool Save();
    }
}
