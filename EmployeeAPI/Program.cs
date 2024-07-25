using EmployeeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;


//better to configure serilog configuration in appsettings.json file.


try
{
    
    

    //Initializing logging functionality using Serilog
    Log.Information("Starting application...");   


    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.

    builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

    


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //adding a db context to cofigure connection to DB.
    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"));
    });

    //adding XML formatter to accept and respond data in XML format
    builder.Services.AddControllers().AddXmlSerializerFormatters();

    //Serilog service adding.
    builder.Services.AddSerilog();

    //line to configure logger to write to file
    

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch(Exception e)
{
    Log.Fatal(e, "Application terminated due to error");
}
finally
{
    Log.CloseAndFlush();
}


