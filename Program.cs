using Guariba.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaContext") ?? throw new InvalidOperationException("Connection string 'SocialMediaContext' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🔹 Adiciona o Identity com User customizado
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // pode ativar se quiser email
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole<int>>() // se for usar roles
    .AddEntityFrameworkStores<SocialMediaContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SocialMediaContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    // Se quiser popular dados iniciais, chame aqui um método para seed.
    DbInitializer.Initialize(context);
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
