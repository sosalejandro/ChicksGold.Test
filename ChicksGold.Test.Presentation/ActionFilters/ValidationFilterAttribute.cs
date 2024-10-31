using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChicksGold.Test.Presentation.ActionFilters;

/// <summary>  
/// Attribute to validate the action parameters before executing the action.  
/// </summary>  
public class ValidationFilterAttribute : IActionFilter
{
    /// <summary>  
    /// Initializes a new instance of the <see cref="ValidationFilterAttribute"/> class.  
    /// </summary>  
    public ValidationFilterAttribute()
    {
    }

    /// <summary>  
    /// Called after the action method is executed.  
    /// </summary>  
    /// <param name="context">The context for the action.</param>  
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    /// <summary>  
    /// Called before the action method is executed.  
    /// </summary>  
    /// <param name="context">The context for the action.</param>  
    public void OnActionExecuting(ActionExecutingContext context)
    {
        //var action = context.RouteData.Values["action"];
        //var controller = context.RouteData.Values["controller"];

        //var param = context.ActionArguments
        //    .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
        //if (param is null)
        //{
        //    context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
        //    return;
        //}

        if (!context.ModelState.IsValid)
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }
}