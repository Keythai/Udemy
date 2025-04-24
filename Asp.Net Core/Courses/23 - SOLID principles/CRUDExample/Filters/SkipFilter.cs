using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters
{
    //by creating this and adding it as attribute, then validate in top of PersonsAlwaysRunResultFilter to skip it
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
