using MamisSolidarias.WebAPI.Users.StartUp;


var builder = WebApplication.CreateBuilder(args);
ServiceRegistrar.Register(builder);

var app = builder.Build();
MiddlewareRegistrator.Register(app);

app.Run();

