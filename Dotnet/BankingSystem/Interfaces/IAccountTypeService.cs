using Model;

namespace interfaces;

public interface IAccountTypeService
{
    Task<List<MasterAccountTypeModel>?> GetAllAccountTypeAsync();
    Task<bool> AddAccountTypeAsync(MasterAccountTypeModel accountTypeModel);
    Task<bool> RemoveAccountTypeAsync(string AccountType);
}