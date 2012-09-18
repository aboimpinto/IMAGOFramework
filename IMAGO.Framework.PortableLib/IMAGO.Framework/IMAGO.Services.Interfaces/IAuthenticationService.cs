using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace IMAGO.Services.Interfaces
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        bool AuthenticateUser(string userName, string password);
    }
}
