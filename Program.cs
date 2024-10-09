using Challenge;
using Challenge.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/WordsFinding", ( WordsandWordStream myParams  ) =>
{

    var finder = new WordsFinder(myParams.matrix);
    // here we validate limits (64,64)
    if ( finder.ValidationMessage is not null )
    {
        return Results.BadRequest(finder.ValidationMessage);
    }

    var foundWords = finder.Find(myParams.wordStream);

    if (app.Environment.IsDevelopment())
    {
        Console.WriteLine("Found Words:");
        foreach (var word in foundWords)
        {
            Console.WriteLine(word);
        }
    }
    return  Results.Ok(foundWords);
})
.WithName("GetWords")
.WithOpenApi();




app.Run();