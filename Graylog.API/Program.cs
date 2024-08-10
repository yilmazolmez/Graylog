using Graylog.API.Filters;
using Graylog.API.Middlewares;
using Graylog.API.Modules;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromFile("Nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers(config =>
    {
        config.Filters.Add<CustomExceptionFilter>();
        config.Filters.Add<LogEventActionsFilter>();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

    builder.Services.AddEndpointsApiExplorer();

    #region Modules
    builder.Services.AddSwaggerModule();
    builder.Services.AddCommonModule();
    builder.Services.AddApplicationModule();
    builder.Services.AddLoggingModule();
    #endregion


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    #region Middlewares
    app.UseMiddleware<TraceIdAndLoggerMiddleware>();
    app.UseMiddleware<RequestAndResponseMiddleware>();
    //app.UseExceptionMiddleware(); //Custom Exception Filter Kullanýlacaktýr
    #endregion

    app.UseAuthorization();


    #region CORS
    app.UseCors(option => option.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
    #endregion


    app.MapControllers();

    app.Run();



}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}