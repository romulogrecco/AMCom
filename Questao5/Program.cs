using MediatR;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Application.Messages;
using Questao5.Domain;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;

var policyCors = "policyCors";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mediator
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

// Notifications
builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

builder.Services.AddSingleton<IMovimentoCommandStore, MovimentoCommandStore>();
builder.Services.AddSingleton<IMovimentoQueryStore, MovimentoQueryStore>();
builder.Services.AddSingleton<IIdempotenciaCommandStore, IdempotenciaCommandStore>();
builder.Services.AddSingleton<IIdempotenciaQueryStore, IdempotenciaQueryStore>();
builder.Services.AddSingleton<IContaCorrenteQueryStore, ContaCorrenteQueryStore>();
builder.Services.AddScoped<IRequestHandler<RealizarMovimentacaoCommand, bool>, MovimentoHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


