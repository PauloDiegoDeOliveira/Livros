using Microsoft.AspNetCore.Authorization;

namespace Livro.Identity.Policies.HorarioComercial
{
    public class HorarioComercialRequirement : IAuthorizationRequirement
    {
        public HorarioComercialRequirement()
        { }
    }
}