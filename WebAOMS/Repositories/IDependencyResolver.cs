using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebAOMS.Repositories;
using WebAOMS.Models;
public class MyDependencyResolver : IDependencyResolver
{

    public object GetService(Type serviceType)
    {
        if (serviceType == typeof(TargetParticipantServices))
        {
            // Replace fmisEntities with your actual entity context instantiation
            return new TargetParticipantServices(new fmisEntities());
        }

        // Handle other service types if needed

        return null;
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        // Implement if needed

        return new List<object>();
    }

    IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
    {
        throw new NotImplementedException();
    }
}
