using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FitLifeAPI.Dados;
using FitLifeAPI.Servicos;
using FitLifeAPI.Modelos;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURAÇÃO DE SERVIÇOS
// ============================================

// Controllers
builder.Services.AddControllers();

// Swagger com comentários XML
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FitLife API",
        Description = "API REST para gerenciamento de treinos, alimentação e hábitos saudáveis",
        Contact = new OpenApiContact
        {
            
        }
    });

    // Incluir comentários XML
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Entity Framework Core com SQL Server
builder.Services.AddDbContext<FitLifeContexto>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

// Injeção de Dependência - Serviços (Clean Architecture)
builder.Services.AddScoped<IServicoUsuario, ServicoUsuario>();
builder.Services.AddScoped<IServicoTreino, ServicoTreino>();
builder.Services.AddScoped<IServicoRefeicao, ServicoRefeicao>();
builder.Services.AddScoped<IServicoLinq, ServicoLinq>();

// CORS (caso precise de frontend separado)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ============================================
// APLICAR MIGRATIONS E SEED SIMPLES (para apresentação)
// ============================================
try
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<FitLifeContexto>();
    // Aplica migrations pendentes automaticamente
    ctx.Database.Migrate();

    // Seed simples: apenas se não houver usuários
    if (!ctx.Usuarios.Any())
    {
        var usuario = new Usuario
        {
            Nome = "Aluno Fit",
            Email = "aluno@fitlife.local",
            Idade =25,
            Peso =70,
            Altura =1.75,
            Objetivo = "Emagrecer"
        };
        ctx.Usuarios.Add(usuario);
        ctx.SaveChanges();

        var treino = new TreinoForca
        {
            Nome = "Treino Inicial",
            DuracaoMinutos =30,
            Data = DateTime.Now,
            UsuarioId = usuario.Id,
            Series =3,
            Repeticoes =10,
            PesoKg =20,
            GrupoMuscular = "Peito"
        };
        ctx.TreinosForca.Add(treino);
        ctx.SaveChanges();
    }
}
catch (Exception ex)
{
    // Não falhar a execução; apenas log simples
    Console.WriteLine("Erro ao aplicar migrations/seed: " + ex.Message);
}

// ============================================
// PIPELINE DE REQUISIÇÕES
// ============================================

// Sempre habilita Swagger e serve em /swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FitLife API v1");
    options.RoutePrefix = "swagger"; // UI disponível em /swagger/index.html
});

// Middlewares
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Mensagem no console
Console.WriteLine("??? FitLife API iniciada!");
Console.WriteLine("?? Swagger: https://localhost:{PORT}/swagger/index.html");

app.Run();