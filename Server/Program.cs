using Server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.UseGrpcWeb();
app.UseCors();

app.MapGrpcService<GrpcConvertService>().EnableGrpcWeb()
                                        .RequireCors("AllowAll");

app.MapGet("/Protos/convert.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/convert.proto"));
                });

app.Run();
