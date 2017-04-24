using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventAppLogicLayer;
using Ninject;

namespace EventAppMVCPresentationLayer.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IEventManager>().To<EventManager>();
            _kernel.Bind<IUserManager>().To<UserManager>();
            _kernel.Bind<IGuestManager>().To<GuestManager>();
            _kernel.Bind<IRoomManager>().To<RoomManager>();
        }
    }
}