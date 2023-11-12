using FluentValidation;
using MazeRunner.API;
using MazeRunner.Application.Behaviours;
using MazeRunner.Data;
using MazeRunner.Domain;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Mediatr+FluentVlidation
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(MazeRunner.Application.Lib.GetAssembly()));
builder.Services.AddValidatorsFromAssembly(MazeRunner.Application.Lib.GetAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//Repos
builder.Services.AddSingleton<IMazesRepository>(new MazesRepositoryFake());
builder.Services.AddSingleton<IGamesRepository>(new GamesRepositoryFake());

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

//Cors to allow wasm application
app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials()
);

app.Run();
