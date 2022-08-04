using System;
using System.Security.Claims;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal static class EndpointFactory
{
    /// <summary>
    /// It creates a fake endpoint with added services
    /// </summary>
    /// <param name="addServices">Function to add new Services, such as ILogger or any other dependency injected service</param>
    /// <param name="dependencies">Constructor parameters</param>
    /// <typeparam name="TEndpoint">Endpoint class</typeparam>
    /// <typeparam name="TRequest">Request clas</typeparam>
    /// <typeparam name="TResponse">Response class</typeparam>
    /// <returns></returns>
    public static TEndpoint CreateEndpoint<TEndpoint, TRequest, TResponse>(
        Action<ServiceCollection>? addServices = null,
        params object?[]? dependencies
    )
        where TEndpoint : Endpoint<TRequest, TResponse> where TRequest : notnull, new() where TResponse : notnull, new()
    {
        return Factory.Create<TEndpoint>(ctx =>
        {
            var services = new ServiceCollection();
            addServices?.Invoke(services);
            ctx.RequestServices = services.BuildServiceProvider();
        },dependencies);
    }

    /// <summary>
    /// It creates a fake endpoint without request with added services
    /// </summary>
    /// <param name="addServices">Function to add new Services, such as ILogger or any other dependency injected service</param>
    /// <typeparam name="TEndpoint">Endpoint class</typeparam>
    /// <typeparam name="TResponse">Response class</typeparam>
    /// <returns></returns>
    public static TEndpoint CreateEndpointWithoutRequest<TEndpoint, TResponse>(
        Action<ServiceCollection>? addServices = null)
        where TEndpoint : EndpointWithoutRequest<TResponse> where TResponse : notnull, new()
    {
        return Factory.Create<TEndpoint>(ctx =>
        {
            var services = new ServiceCollection();
            addServices?.Invoke(services);

            ctx.RequestServices = services.BuildServiceProvider();
        });
    }

    /// <summary>
    /// It creates a fake endpoint without response with added services
    /// </summary>
    /// <param name="addServices">Function to add new Services, such as ILogger or any other dependency injected service</param>
    /// <param name="user">User claims to mock</param>
    /// <param name="dependencies">Constructor parameters</param>
    /// <typeparam name="TEndpoint">Endpoint class</typeparam>
    /// <typeparam name="TRequest">Request clas</typeparam>
    public static TEndpoint CreateEndpointWithoutResponse<TEndpoint, TRequest>(
        Action<ServiceCollection>? addServices = null,
        ClaimsPrincipal? user = null,
        params object?[]? dependencies
    )
        where TEndpoint : Endpoint<TRequest> where TRequest : notnull, new()
    {
        return Factory.Create<TEndpoint>(ctx =>
        {
            if (user is not null) 
                ctx.HttpContext.User = user;
            
            var services = new ServiceCollection();
            addServices?.Invoke(services);

            ctx.RequestServices = services.BuildServiceProvider();
        },dependencies);
    }
}