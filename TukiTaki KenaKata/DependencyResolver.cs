using Autofac;
using TukiTaki_KenaKata.persistant;
using TukiTaki_KenaKata.presentation;
using TukiTaki_KenaKata.service;

namespace TukiTaki_KenaKata
{
    class DependencyResolver
    {
        //private static DependencyResolver instance = null;
        private static IContainer Container = null;
        private static ContainerBuilder builder = null;
        private DependencyResolver() { }
        public static IContainer Instance()
        {
            if(Container == null)
            {
                //instance = new DependencyResolver();

                builder = new ContainerBuilder();
                RegisterModules();
                Container = builder.Build();
            }
            return Container;
        }
        private static void RegisterModules()
        {
            //builder.Register(c => new Component()).As<IComponent>();
            builder.Register(c => new Home()).As<IHome>();

            builder.Register(c => new ProductView()).As<IProductView>();
            builder.RegisterType <WishView> ().As<IWishView>();

            builder.RegisterType <WishService> ().As<IWishService>();
            builder.RegisterType <ProductService> ().As<IProductService>();

            //builder.RegisterType <DBRepository> ().As<IDBRepository>();
            builder.Register(c => CassandraDBRepository.Instance()).As<IDBRepository>();
        }
    }
}
