using CustomCADs.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADs.API.Helpers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken cancellationToken)
    {
        if (ex is CadNotFoundException or OrderNotFoundException or CategoryNotFoundException or UserNotFoundException or RoleNotFoundException)
        {
            context.Response.StatusCode = Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = "Resource Not Found", message = ex.Message }).ConfigureAwait(false);
        }
        else if (ex is OrderMissingCadException or DesignerNotAssociatedWithOrderException)
        {
            context.Response.StatusCode = Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid Operation", message = ex.Message }).ConfigureAwait(false);
        }
        else if (ex is DbUpdateConcurrencyException)
        {
            context.Response.StatusCode = Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { error = "Database Conflict Ocurred", message = ex.Message }).ConfigureAwait(false);
        }
        else if (ex is DbUpdateException)
        {
            context.Response.StatusCode = Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Database Error", message = ex.Message }).ConfigureAwait(false);
        }
        else
        {
            context.Response.StatusCode = Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error", message = ex.Message }).ConfigureAwait(false);
        }

        return true;
    }
}
