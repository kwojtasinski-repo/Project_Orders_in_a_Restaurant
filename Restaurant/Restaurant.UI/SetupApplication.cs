using Castle.Windsor;

namespace Restaurant.UI
{
    public class SetupApplication
    {
        public static IWindsorContainer Create()
        {
            var container = new WindsorContainer();
            return container;
        }
    }
}
