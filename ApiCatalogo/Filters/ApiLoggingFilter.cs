using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ApiCatalogo.Filters
{
  public class ApiLoggingFilter : IActionFilter
  {
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
      _logger = logger;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
      _logger.LogInformation("Metodo: OnActionExecuted");
      _logger.LogInformation($"Data: {DateTime.Now.ToString()}");
      _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      _logger.LogInformation("Metodo: OnActionExecuting");
      _logger.LogInformation($"Data: {DateTime.Now.ToString()}");
      _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
    }
  }
}