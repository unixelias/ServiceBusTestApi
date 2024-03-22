var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ServiceBusTestApi",
        Description = "API simples para enviar mensagens a tópicos do Service BUS para validação de desenvolvimento de aplicações consumidoras",
        Version = "v1",
        Contact = new()
        {
            Name = "Elias Alves",
            Email = "unixelias@gmail.com"
        },
        License = new()
        {
            Name = "MIT",
            Url = new("https://opensource.org/licenses/MIT")
        },
        TermsOfService = new("https://opensource.org/licenses/MIT")
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();