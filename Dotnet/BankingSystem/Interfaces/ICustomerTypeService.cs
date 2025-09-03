using Model;

namespace interfaces
{
    public interface ICustomerTypeService
    {
        Task<List<MasterCustomerType>> GetAllCustomerType();
        Task<bool> AddCustomerType(MasterCustomerType customerType);
        Task<bool> DeleteCustomerType(string customerType);
    }
}
