using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WebBaseServer.Interface
{
    [ServiceContract]
    public interface ISystemFunctionServer
    {
        [OperationContract]
        string GetUserFunction(int _nUserId);
    }
}
