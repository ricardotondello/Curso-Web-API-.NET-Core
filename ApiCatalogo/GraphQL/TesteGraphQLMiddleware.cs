using ApiCatalogo.Repository;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiCatalogo.GraphQL
{
  public class TesteGraphQLMiddleware
  {
    private readonly RequestDelegate _next;

    public TesteGraphQLMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IUnitOfWork _context)
    {
      if (httpContext.Request.Path.StartsWithSegments("/graphql"))
      {
        using (var stream = new StreamReader(httpContext.Request.Body))
        {
          var query = await stream.ReadToEndAsync();

          if (!String.IsNullOrWhiteSpace(query))
          {
            var schema = new Schema
            {
              Query = new CategoriaQuery(_context)
            };

            var result = await new DocumentExecuter().ExecuteAsync(options =>
            {
              options.Schema = schema;
              options.Query = query;
            });
            await WriteResult(httpContext, result);
          }
        }
      }
      else
      {
        await _next(httpContext);
      }
    }

    private async Task WriteResult(HttpContext httpContext,
       ExecutionResult result)
    {
      var json = new DocumentWriter(indent: true).Write(result);
      httpContext.Response.StatusCode = 200;
      httpContext.Response.ContentType = "application/json";
      await httpContext.Response.WriteAsync(json);
    }

  }
}
