// Custom constraint: **note that, it doesn't provide proper error message so it's not recommended to use

using System.Text.RegularExpressions;

namespace RoutingParameters.CustomConstraint
{
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext? httpContext, // http context
            IRouter? route,  // context request path
            string routeKey,  // route parameter name
            RouteValueDictionary values,  // route parameter value,
                                          // for example /sales-report/{year}/{month}, /sales-report/2021/apr,
                                          // values = [year][2021],
                                          //                [month] [apr]
            RouteDirection routeDirection) 
        {
            if (!values.ContainsKey(routeKey)) 
            {
                return false;
            }
            Regex regex = new Regex("^(apr|jul|oct|jan)$");
            string? month = Convert.ToString(values[routeKey]);
            // check if the month is valid
            if (regex.IsMatch(month))
            {
                return true;
            }
            return false;
        }
    }
}
