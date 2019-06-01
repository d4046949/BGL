using System.Web.Mvc;

namespace GitHubMvc.CustomAttributes
{
    public class PageAttribute : ActionFilterAttribute
    {
        private readonly string _title;

        public PageAttribute(string title)
        {
            _title = title;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;

            viewBag.Title = _title;
        }
    }
}