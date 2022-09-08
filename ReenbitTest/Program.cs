using ReenbitTest.Context;
using ReenbitTest.Hubs;
using Microsoft.EntityFrameworkCore;
using ReenbitTest.Services;
using ReenbitTest.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddDbContext<ChatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatConnectionString")));

builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupUserService, GroupUserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChatContext>();
    context.Database.EnsureCreated();
}

app.UseRouting();
app.UseCors(builder => builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000"));

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/hub");
});

app.Run();