using DOTRestAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connection string dari appsettings.json
// Pastikan di appsettings.json ada "ConnectionStrings:LOCAL"
var connectionString = builder.Configuration.GetConnectionString("LOCAL");

// Tambahkan DbContext ke DI container
builder.Services.AddDbContext<DOTRestDbContext>(options =>
{
    // Mengaktifkan Lazy Loading
    options.UseLazyLoadingProxies()
           .UseSqlServer(connectionString);
});

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Mengabaikan (Ignore) referensi berulang
        // Opsi 1: IgnoreCycles (tidak akan lempar error, tetapi objek berulang jadi null)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Opsi 2: Preserve (gunakan $id, $ref, dsb., untuk menjaga referensi)
        // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

        // (Opsional) menambah max depth
        // default 32. Jika butuh lebih dalam, bisa naikkan
        options.JsonSerializerOptions.MaxDepth = 64;
    });

// Swagger (opsional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfigurasi pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
