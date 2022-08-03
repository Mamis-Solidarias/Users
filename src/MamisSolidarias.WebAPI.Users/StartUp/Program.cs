using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.StartUp;

var builder = WebApplication.CreateBuilder(args);

ServiceRegistrator.Register(builder);

var app = builder.Build();
MiddlewareRegistrator.Register(app);

app.Run();

