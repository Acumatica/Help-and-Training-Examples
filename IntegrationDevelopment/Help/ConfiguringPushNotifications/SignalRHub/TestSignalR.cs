using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR.Client;

class Program
{
    static void Main(string[] args)
    {
        var login = "admin";
        var tenant = "Tenant";
        var password = "123";
        // Set up a Basic authentication token 
        var basicAuthToken = Convert.ToBase64String(
         Encoding.UTF8.GetBytes(login + "@" + tenant + ":" + password));

        //Connect to an Acumatica ERP instance
        var connection = new HubConnection("http://localhost:8081/AcumaticaDB/");
        connection.Headers.Add("Authorization", "Basic " + basicAuthToken);

        //Create a proxy to hub
        //Use "PushNotificationsHub" as the address of the hub
        var myHub = connection.CreateHubProxy("PushNotificationsHub");
        connection.Start().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Console.WriteLine(
                  "There was an error during open of the connection:{0}",
                  task.Exception.GetBaseException());
            }
            else
            {
                //Instead of "TestSignalR", specify the name 
                //that you specified on the Push Notifications form
                myHub.Invoke<string>("Subscribe", "TestSignalR").Wait();
            }
        }).Wait();

        //Process the notifications
        myHub.On<NotificationResult>("ReceiveNotification", nr =>
        {
            Console.WriteLine("Inserted {0}", nr.Inserted.Length);
            Console.WriteLine("Deleted {0}", nr.Deleted.Length);
        });
        Console.Read();
        connection.Stop();
    }
}

public class NotificationResult
{
    public object[] Inserted { get; set; }
    public object[] Deleted { get; set; }
    public string Query { get; set; }
    public string CompanyId { get; set; }
    public Guid Id { get; set; }
    public long TimeStamp { get; set; }
    public Dictionary<string, object> AdditionalInfo { get; set; }
}
