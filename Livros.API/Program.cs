using Livros.API.Configuration;
using Livros.Application.Utilities.Paths;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configura��o do Serilog para logging
    SerilogConfig.AddSerilogApi();
    builder.Host.UseSerilog(Log.Logger);
    Log.Warning("Iniciando API");

    // Configura��es e ambiente da aplica��o
    ConfigurationManager configurationManager = builder.Configuration;
    IWebHostEnvironment environment = builder.Environment;
    Log.Warning("Ambiente atual: {EnvironmentName}", environment.EnvironmentName);

    // Registro de servi�os
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

    // Constru��o da aplica��o Web
    var app = builder.Build();
    Log.Warning("Aplica��o constru�da.");
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configura��o do pipeline de requisi��es HTTP
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

    // Configura��es adicionais e mapeamento de rotas
    app.UseDatabaseConfiguration();
    app.UseSwaggerConfiguration(environment, provider);
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseJwtConfiguration();
    app.UseHealthChecksConfiguration();
    app.MapControllers();

    // Iniciando a aplica��o
    Log.Warning("API iniciada. Aguardando requisi��es...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uma exce��o n�o tratada {ExceptionType} ocorreu, levando ao t�rmino da API. Detalhes: {ExceptionMessage}, Pilha de Chamadas: {StackTrace}", ex.GetType().Name, ex.Message, ex.StackTrace);
}
finally
{
    Log.Information("A execu��o da API foi conclu�da �s {DateTime}. Fechando e liberando recursos...", DateTime.Now);
    Log.CloseAndFlush();
}