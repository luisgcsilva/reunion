namespace api.reunion.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdminGroupService
    {
        string GetUserRole(string username);
        string GetGroup(string role);
        string VerifyUserRole(string username, int selectedLocal);

    }
}
