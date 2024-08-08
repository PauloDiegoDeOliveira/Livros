using Livros.API.Configuration;
using Livros.Application.Utilities.Paths;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configuração do Serilog para logging
    SerilogConfig.AddSerilogApi();
    builder.Host.UseSerilog(Log.Logger);
    Log.Warning("Iniciando API");

    // Configurações e ambiente da aplicação
    ConfigurationManager configurationManager = builder.Configuration;
    IWebHostEnvironment environment = builder.Environment;
    Log.Warning("Ambiente atual: {EnvironmentName}", environment.EnvironmentName);

    // Registro de serviços
    builder.Services.AddControllers();
    builder.Services.AddJwtTConfiguration(configurationManager);
    builder.Services.AddFluentValidationConfiguration();
    builder.Services.AddAutoMapperConfiguration();
    builder.Services.AddDatabaseConfiguration(configurationManager);
    builder.Services.AddDependencyInjectionConfiguration();
    builder.Services.AddSMTPConfiguration(configurationManager);
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddAuthorizationPolicies();
    builder.Services.AddCorsConfiguration();
    builder.Services.AddVersionConfiguration();
    builder.Services.AddHealthChecksConfiguration(configurationManager);

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Carregando URLs Base64
    await GerenciadorDeCaminhos.CarregarURLsBase64();

    // Construção da aplicação Web
    var app = builder.Build();
    Log.Warning("Aplicação construída.");
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configuração do pipeline de requisições HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseCors("Development");
    }
    else if (app.Environment.IsStaging())
    {
        app.UseCors("Staging");
    }
    else if (app.Environment.IsProduction())
    {
        app.UseCors("Production");
        app.UseHsts();
    }

    // Configurações adicionais e mapeamento de rotas
    app.UseDatabaseConfiguration();
    app.UseSwaggerConfiguration(environment, provider);
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseJwtConfiguration();
    app.UseHealthChecksConfiguration();
    app.MapControllers();

    // Iniciando a aplicação
    Log.Warning("API iniciada. Aguardando requisições...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uma exceção não tratada {ExceptionType} ocorreu, levando ao término da API. Detalhes: {ExceptionMessage}, Pilha de Chamadas: {StackTrace}", ex.GetType().Name, ex.Message, ex.StackTrace);
}
finally
{
    Log.Information("A execução da API foi concluída às {DateTime}. Fechando e liberando recursos...", DateTime.Now);
    Log.CloseAndFlush();
}