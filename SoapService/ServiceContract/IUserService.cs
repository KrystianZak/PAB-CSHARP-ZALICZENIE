using SoapService.Entity;
using System.ServiceModel;

namespace SoapService.ServiceContract
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string RegisterUser(User user);
    }
}
