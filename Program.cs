using AkademicReport.Data;
using AkademicReport.Service.AsignaturaServices;
using AkademicReport.Service.AulaServices;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.ConceptoServices;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Service.NivelServices;
using AkademicReport.Service.PeriodoServices;
using AkademicReport.Service.RecintoServices;
using AkademicReport.Service.ReposteServices;
using AkademicReport.Service.UsuarioServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));

});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRecintoService, RecintoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPeriodoService, PeriodoService>();
builder.Services.AddScoped<INivelAcademicoService, NivelAcademicoService>();
builder.Services.AddScoped<IConceptoService, ConceptoService>();
builder.Services.AddScoped<IAulaService, AulaService>();
builder.Services.AddScoped<IAsignaturaService, AsignaturaService>();

builder.Services.AddScoped<IDocenteService, DocenteService>();
builder.Services.AddScoped<ICargaDocenteService, CargaService>();
builder.Services.AddScoped<IReposteService, ReporteService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
