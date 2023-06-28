using Livro.Identity.Entitys;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Livros.API.Middleware.Token
{
    public class ValidateTokenVersion
    {
        private readonly RequestDelegate requestDelegate;

        public ValidateTokenVersion(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<Usuario> userManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Usuario user = await userManager.FindByIdAsync(userId);

                Claim tokenVersionClaim = context.User.FindFirst("VersaoToken");

                if (tokenVersionClaim != null && int.TryParse(tokenVersionClaim.Value, out int tokenVersion))
                {
                    if (user.VersaoToken != tokenVersion)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";

                        var responseObj = new
                        {
                            mensagem = "Seu acesso expirou."
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(responseObj));

                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var responseObj = new
                    {
                        mensagem = "A declaração de versão do token está ausente ou não pôde ser analisada."
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(responseObj));

                    return;
                }
            }

            await requestDelegate(context);
        }
    }
}