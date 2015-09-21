using System;
using System.Collections.Generic;

namespace IwAutoUpdater.DIMappings
{
    public static class Container
    {
        public static SimpleInjector.Container Init(IEnumerable<Action<SimpleInjector.Container>> additionalRegistrations)
        {
            return InitCore(additionalRegistrations);
        }

        public static SimpleInjector.Container Init()
        {
            return InitCore();
        }

        private static SimpleInjector.Container InitCore(IEnumerable<Action<SimpleInjector.Container>> additionalRegistrations = null)
        {
            SimpleInjector.Container container = new SimpleInjector.Container();

            InitCrossCutting(container);
            InitDAL(container);
            InitBLL(container);

            if (additionalRegistrations != null)
            {
                container.Options.AllowOverridingRegistrations = true;

                foreach (var additionalRegistration in additionalRegistrations)
                {
                    additionalRegistration(container);
                }
            }

            container.Verify();

            return container;
        }

        private static void InitCrossCutting(SimpleInjector.Container container)
        {
            (new CrossCutting.LoggingMappings() as IInitializeMapping).Init(container);
            (new CrossCutting.ConfigurationMappings() as IInitializeMapping).Init(container);
            (new CrossCutting.SFWMappings() as IInitializeMapping).Init(container);
        }

        private static void InitDAL(SimpleInjector.Container container)
        {
            (new DAL.LocalFilesMapping() as IInitializeMapping).Init(container);
            (new DAL.NotificationsMappings() as IInitializeMapping).Init(container);
            (new DAL.UpdatesMappings() as IInitializeMapping).Init(container);
        }

        private static void InitBLL(SimpleInjector.Container container)
        {
            (new BLL.AutoUpdaterMappings() as IInitializeMapping).Init(container);
            (new BLL.CommandPlannerMappings() as IInitializeMapping).Init(container);
        }
    }
}
