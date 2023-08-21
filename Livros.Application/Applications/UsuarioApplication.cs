using AutoMapper;
using Livro.Identity.Extensions;
using Livros.Application.Dtos.Smtp;
using Livros.Application.Dtos.Usuario;
using Livros.Application.Dtos.Usuario.Autenticacao;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;

namespace Livros.Application.Applications
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly SignInManager<Usuario> signInManager;
        private readonly UserManager<Usuario> userManager;
        private readonly JwtOptions jwtOptions;
        private readonly IEmailApplication emailApplication;
        private readonly IUsuarioService usuarioService;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUser user;
        private readonly INotifier notifier;
        private readonly EAmbiente actualEnvironment;

        public UsuarioApplication(SignInManager<Usuario> signInManager,
                                  UserManager<Usuario> userManager,
                                  IOptions<JwtOptions> jwtOptions,
                                  IEmailApplication emailApplication,
                                  IUsuarioService usuarioService,
                                  IMapper mapper,
                                  IConfiguration configuration,
                                  IUser user,
                                  INotifier notifier,
                                  IWebHostEnvironment environment)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.jwtOptions = jwtOptions.Value;
            this.emailApplication = emailApplication;
            this.usuarioService = usuarioService;
            this.mapper = mapper;
            this.configuration = configuration;
            this.user = user;
            this.notifier = notifier;
            actualEnvironment = environment.IsDevelopment() ? EAmbiente.Desenvolvimento : environment.IsStaging() ?
                    EAmbiente.Homologacao : EAmbiente.Producao;
        }

        public async Task<ViewPaginacaoUsuarioDto> GetPaginationAsync(ParametersUsuario parametersUsuario)
        {
            PagedList<Usuario> usuarios = await usuarioService.GetPaginationAsync(parametersUsuario);
            if (usuarios == null)
            {
                return null;
            }

            return new ViewPaginacaoUsuarioDto(usuarios, mapper.Map<List<ViewUsuarioDto>>(usuarios));
        }

        public async Task<ViewUsuarioDto> GetByIdAsync(Guid id)
        {
            Usuario usuario = await userManager.FindByIdAsync(id.ToString());
            if (usuario is null)
            {
                AddNotification("Nenhum usuário foi encontrado com o id informado.");
                return null;
            }

            return mapper.Map<ViewUsuarioDto>(usuario);
        }

        public async Task<bool> CadastrarUsuario(PostCadastroUsuarioDto postCadastroUsuarioDto)
        {
            Usuario usuario = new()
            {
                UserName = postCadastroUsuarioDto.Email,
                Email = postCadastroUsuarioDto.Email,
                Nome = postCadastroUsuarioDto.Nome,
                //usuario.Sobrenome = postCadastroUsuarioDto.Sobrenome;
                Genero = postCadastroUsuarioDto.Genero.ToString(),
                DataNascimento = postCadastroUsuarioDto.DataNascimento
            };

            IdentityResult result = await userManager.CreateAsync(usuario, postCadastroUsuarioDto.Senha);
            if (!result.Succeeded)
            {
                AddNotification(result.Errors.Select(r => r.Description).ToList());
                return false;
            }

            await userManager.SetLockoutEnabledAsync(usuario, false);
            await GerarTokenConfirmacaoEmail(usuario);
            return true;
        }

        public async Task<ViewUsuarioDto> UpdateUsuario(PutCadastroUsuarioDto putCadastroUsuarioDto)
        {
            Usuario usuario = await userManager.FindByIdAsync(putCadastroUsuarioDto.Id.ToString());
            if (usuario is null)
            {
                AddNotification("Nenhum usuário foi encontrado com o id informado.");
                return null;
            }

            usuario.Nome = putCadastroUsuarioDto.Nome;
            //usuario.Sobrenome = putCadastroUsuarioDto.Sobrenome;
            usuario.Genero = putCadastroUsuarioDto.Genero.ToString();
            usuario.DataNascimento = putCadastroUsuarioDto.DataNascimento;
            usuario.Status = putCadastroUsuarioDto.Status.ToString();
            usuario.AlteradoEm = DateTime.Now;

            IdentityResult result = await userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
            {
                AddNotification(result.Errors.Select(r => r.Description).ToList());
                return null;
            }

            return mapper.Map<ViewUsuarioDto>(usuario);
        }

        public async Task<ViewUsuarioDto> PutStatusAsync(Guid id, EStatus status)
        {
            Usuario usuario = await usuarioService.PutStatusAsync(id, status);
            if (usuario is null)
            {
                return null;
            }

            return mapper.Map<ViewUsuarioDto>(usuario);
        }

        public async Task<ViewLoginDto> Login(PostLoginUsuarioDto postLoginUsuarioDto)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(postLoginUsuarioDto.Email, postLoginUsuarioDto.Senha, false, true);
            if (result.Succeeded)
            {
                ViewLoginDto usuarioLoginResponse = await GerarCredenciais(postLoginUsuarioDto.Email);

                if (usuarioLoginResponse is null)
                {
                    AddNotification("Erro ao gerar as credenciais de acesso.");
                    return null;
                }

                return usuarioLoginResponse;
            }
            else
            {
                if (result.IsLockedOut)
                {
                    AddNotification("Essa conta está bloqueada.");
                }
                else if (result.IsNotAllowed)
                {
                    AddNotification("É necessario verificar o e-mail para fazer login.");
                }
                else if (result.RequiresTwoFactor)
                {
                    AddNotification("É necessário confirmar o login no seu segundo fator de autenticação.");
                }
                else
                {
                    AddNotification("Usuário ou senha estão incorretos.");
                }

                return null;
            }
        }

        public async Task<bool> AlterarSenha(PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto)
        {
            IdentityResult result = await userManager.ChangePasswordAsync(await userManager.FindByIdAsync(postAlterarSenhaUsuarioDto.UsuarioId.ToString()),
                                                                            postAlterarSenhaUsuarioDto.SenhaAntiga, postAlterarSenhaUsuarioDto.NovaSenha);

            if (!result.Succeeded)
            {
                AddNotification(result.Errors.Select(r => r.Description).ToList());
                return false;
            }

            return true;
        }

        public async Task<bool> ResetarSenha(PostResetarSenhaDto postResetarSenhaDto)
        {
            Usuario usuario = await userManager.FindByIdAsync(postResetarSenhaDto.UsuarioId.ToString());
            if (usuario is null)
            {
                AddNotification("Usuário não encontrado.");
                return false;
            }

            if (await userManager.CheckPasswordAsync(usuario, postResetarSenhaDto.NovaSenha))
            {
                AddNotification("A senha atual não pode ser a mesma da anterior.");
                return false;
            }

            IdentityResult passwordReset = await userManager.ResetPasswordAsync(usuario, HttpUtility.UrlDecode(postResetarSenhaDto.Token), postResetarSenhaDto.NovaSenha);
            if (!passwordReset.Succeeded)
            {
                AddNotification(passwordReset.Errors.Select(r => r.Description).ToList());
                return false;
            }

            return true;
        }

        public async Task<bool> ConfimarEmail(PostConfirmacaoEmailDto postConfirmacaoEmailDto)
        {
            Usuario usuario = await userManager.FindByIdAsync(postConfirmacaoEmailDto.UsuarioId.ToString());
            if (usuario is null)
            {
                AddNotification("Usuário não encontrado.");
                return false;
            }

            IdentityResult confirmation = await userManager.ConfirmEmailAsync(usuario, HttpUtility.UrlDecode(postConfirmacaoEmailDto.Token));
            if (!confirmation.Succeeded)
            {
                AddNotification(confirmation.Errors.Select(r => r.Description).ToList());
                return false;
            }

            return true;
        }

        public async Task<bool> ReenviarConfirmacaoEmail(PostEmailDto postConfirmacaoEmailDto)
        {
            Usuario usaurio = await userManager.FindByEmailAsync(postConfirmacaoEmailDto.Email);
            if (usaurio is null)
            {
                AddNotification("Usuário não encontrado.");
                return false;
            }

            if (usaurio.EmailConfirmed)
            {
                AddNotification("Email já foi confirmado.");
                return false;
            }

            await GerarTokenConfirmacaoEmail(usaurio);

            return true;
        }

        public async Task<bool> EnviarEmailResetarSenha(PostEmailDto postConfirmacaoEmailDto)
        {
            Usuario usuario = await userManager.FindByEmailAsync(postConfirmacaoEmailDto.Email);
            if (usuario is null)
            {
                AddNotification("Usuário não encontrado.");
                return false;
            }

            await GerarTokenResetSenha(usuario);

            return true;
        }

        private async Task GerarTokenConfirmacaoEmail(Usuario user)
        {
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await CriarEmail(user, token, configuration.GetSection("Application:ConfirmarEmail").Value, "Olá {{UserName}}, confirme o seu email do Libro Vault.", "ConfirmarEmail");
            }
        }

        private async Task GerarTokenResetSenha(Usuario user)
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await CriarEmail(user, token, configuration.GetSection("Application:ResetarSenha").Value, "Olá {{UserName}}, resetar sua senha (Libro Vault).", "ResetSenha");
            }
        }

        private async Task CriarEmail(Usuario user, string token, string pageName, string subject, string template)
        {
            string appdomain = configuration.GetSection($"Application:AppDomain{actualEnvironment}").Value;
            string emailLink = $"{pageName}?UsuarioId={user.Id}&token={HttpUtility.UrlEncode(token)}";

            UserEmailOptions options = new()
            {
                ToEmails = new List<string> { user.Email },
                PlaceHolders = new()
                    {
                        new KeyValuePair<string, string>("{{UserName}}",  user.Nome),
                        new KeyValuePair<string, string>("{{Year}}",  DateTime.Now.Year.ToString()),
                        new KeyValuePair<string, string>("{{Link}}",  appdomain + emailLink)
                    },
                Subject = subject
            };

            await emailApplication.SendEmail(options, template);
        }

        private async Task<ViewLoginDto> GerarCredenciais(string email)
        {
            Usuario usuario = await userManager.FindByEmailAsync(email);

            bool versaoToken = await usuarioService.VersaoToken(new(usuario.Id));
            if (versaoToken is false)
            {
                return null;
            }

            if (usuario is null)
            {
                AddNotification("Usuário não encontrado.");
                return null;
            }

            IList<Claim> accessTokenClaims = await ObterClaims(usuario, adicionarClaimsUsuario: true);
            IList<Claim> refreshTokenClaims = await ObterClaims(usuario, adicionarClaimsUsuario: false);

            DateTime dataExpiracaoAccessToken = DateTime.Now.AddSeconds(jwtOptions.AccessTokenExpiration);
            DateTime dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(jwtOptions.RefreshTokenExpiration);

            string accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
            string refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

            Usuario usuarioAtualizado = await usuarioService.AlteraUltimoAcesso(new(usuario.Id));
            if (usuarioAtualizado is null)
            {
                AddNotification("Erro ao alterar o último acesso de usuário.");
                return null;
            }

            return new ViewLoginDto
            {
                Id = Guid.Parse(usuario.Id),
                Nome = usuario.Nome,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Notificar = usuario.Notificar
            };
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                Expires = dataExpiracao,
                NotBefore = DateTime.Now,
                SigningCredentials = jwtOptions.SigningCredentials
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }

        private async Task<IList<Claim>> ObterClaims(Usuario usuario, bool adicionarClaimsUsuario)
        {
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("VersaoToken", usuario.VersaoToken.ToString())
            };

            if (adicionarClaimsUsuario)
            {
                IList<Claim> userClaims = await userManager.GetClaimsAsync(usuario);
                IList<string> roles = await userManager.GetRolesAsync(usuario);

                claims.AddRange(userClaims);
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                //foreach (string role in roles)
                //    claims.Add(new Claim("role", role));
            }

            return claims;
        }

        public async Task<ViewUsuarioDto> DeleteAsync(Guid id)
        {
            Usuario usuario = await userManager.FindByIdAsync(id.ToString());
            if (usuario is null)
            {
                AddNotification("Nenhum usuário foi encontrado com o id informado.", ENotificationType.Warning);
                return null;
            }

            IdentityResult removido = await userManager.DeleteAsync(usuario);
            if (!removido.Succeeded)
            {
                AddNotification("Erro ao excluir o usuário.", ENotificationType.Warning);
                return null;
            }

            return mapper.Map<ViewUsuarioDto>(usuario);
        }

        public bool ValidarId(Guid id)
        {
            return usuarioService.ValidarId(id);
        }

        protected void AddNotification(string messege)
        {
            notifier.AddNotification(new Notification(messege));
        }

        protected void AddNotification(string messege, ENotificationType notificationType)
        {
            notifier.AddNotification(new Notification(messege, notificationType));
        }

        protected void AddNotification(List<string> messegeList)
        {
            foreach (string erro in messegeList)
            {
                notifier.AddNotification(new Notification(erro));
            }
        }

        protected void AddNotification(List<string> messegeList, ENotificationType notificationType)
        {
            foreach (string erro in messegeList)
            {
                notifier.AddNotification(new Notification(erro, notificationType));
            }
        }
    }
}