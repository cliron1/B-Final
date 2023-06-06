using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace BezeqFinalProject.WebApi.Filters;

public class StopwatchFilter : IAsyncActionFilter {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        var watch = Stopwatch.StartNew();

        await next();

        var headers = context.HttpContext.Response.Headers;

        if(!watch.IsRunning)
            return;

        watch.Stop();
        if(!headers.ContainsKey("action-duration"))
            headers.Add("action-duration", $"{watch.ElapsedMilliseconds}ms");
    }
}
