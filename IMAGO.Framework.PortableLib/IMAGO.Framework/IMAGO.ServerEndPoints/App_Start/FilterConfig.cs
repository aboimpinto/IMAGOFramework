using System.Web;
using System.Web.Mvc;

namespace IMAGO.ServerEndPoints
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}