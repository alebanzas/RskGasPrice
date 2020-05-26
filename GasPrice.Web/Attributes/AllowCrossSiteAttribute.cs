using System.Configuration;
using System.Web.Mvc;

public class AllowCrossSiteAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if(filterContext.HttpContext == null || filterContext.HttpContext.Request.UrlReferrer == null)
        {
            filterContext.Result = new HttpNotFoundResult();
            return;
        }

        var originSegments = filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri.Split('/');
        var originUrl = string.Format("{0}//{1}", originSegments[0], originSegments[2]);

        var config = ConfigurationManager.AppSettings["API_CORS_DOMAINS"];
        if (!string.IsNullOrWhiteSpace(config))
        {
            var domains = config.Split(',');
            if(domains.Length > 0)
            {
                foreach (var d in domains)
                {
                    if (d.Equals(originUrl))
                    {
                        filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", d);
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                }

                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }
        }
        filterContext.Result = new HttpNotFoundResult();
    }
}