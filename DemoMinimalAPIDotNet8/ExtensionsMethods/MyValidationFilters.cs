using MiniValidation;

namespace DemoMinimalAPIDotNet8.ExtensionsMethods;

public static class MyRouteHandlerExtension
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        builder.AddEndpointFilter<MyValidationFilter<T>>();
        return builder;
    }

    public class MyValidationFilter<T> : IEndpointFilter where T : class
    {
        public async ValueTask<object?> InvokeAsync
            (EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var input = context.Arguments.SingleOrDefault(x => x is T) as T;
            if (input is null)
            {
                return TypedResults.BadRequest();
            }

            var (isValid, errors) = await MiniValidator.TryValidateAsync(input);
            if (isValid)
            {
                return await next(context);
            }
            else
            {
                return TypedResults.BadRequest(errors);
            }
        }
    }
}
