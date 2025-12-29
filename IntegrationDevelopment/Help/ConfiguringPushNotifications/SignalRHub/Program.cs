using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        // Specify credentials to sign in to Acumatica ERP.
        // If you use a multi-tenant Acumatica ERP instance,
        // append the tenant name to the login as follows: "admin@MyTenant".
        var login = "admin";
        var password = "123";
        // Set up a Basic authentication token.
        var basicAuthToken = Convert.ToBase64String(
         Encoding.UTF8.GetBytes(login + ":" + password));

        // Specify the URL of an Acumatica ERP instance.
        string acumaticaErpUrl = "http://localhost:80/AcumaticaDB_SignalR/";
        // Instead of "TestSignalR", specify the name
        // that you specified on the Push Notifications (SM302000) form.
        var notification = "TestSignalR";

        // Create and configure a SignalR Hub connection.
        HubConnection connection = new HubConnectionBuilder()
            // Specify the URL of the Push Notifications Hub.
            // It must end with "signalr/hubs/PushNotificationsHub".
            .WithUrl(acumaticaErpUrl + "signalr/hubs/PushNotificationsHub", 
            options =>
            {
                options.Headers.Add("Authorization", "Basic " + basicAuthToken);
                options.Transports = HttpTransportType.WebSockets;
                options.SkipNegotiation = true;
            }).ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddProvider(new CustomConsoleLoggerProvider());
            }).Build();

        try
        {
            await connection.StartAsync();
            // Subscribe to the specified notification.
            await connection.InvokeAsync("Subscribe", notification);
            // Handle the received notifications.
            connection.On<object>("ReceiveNotification", notificationReceived =>
                Console.WriteLine(notificationReceived.ToString()));
            while (Console.Read() != '3')
            {
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during connection: {ex.Message}");
            Console.Read();
        }
        finally
        {
            await connection.StopAsync();
            Console.WriteLine("Connection stopped.");
        }
    }

    // A logger to display SignalR logs in the console.
    public class CustomConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => 
            new CustomConsoleLogger();
        public void Dispose() { }
    }

    public class CustomConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, 
            TState state, Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"[{logLevel}] {formatter(state, exception)}");
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }
    }
}