using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.DAL;
using ImageSharingWithCloudService.Queues;

namespace ImageSharingWorkerRoleWithSBQueue
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        const string QueueName = "ImagesQueue";
       
        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        ApplicationDbContext db = new ApplicationDbContext();
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        public void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");
            WriteLog("in Run()");
            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Process the message
                        Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
                        
                        //validate the image
                        Image image = receivedMessage.GetBody<Image>();
                        bool isValidated = ValidateImage(image);
                        
                        if (isValidated)
                        {
                            image.Valid = true;
                            DatabaseAccess.UpdateImageInDB(image);
                            QueueManager.SendImageEntityToQueue(image);
                           // image.Id =2 ;
                            //db.Images.Add(image);
                            //db.Entry(image).State = System.Data.Entity.EntityState.Added;
                           // db.SaveChanges();
                        }
                        else
                        {
                            
                            DatabaseAccess.DeleteImageFromDB(image);
                            ImageStorage.DeleteImageFromBlob(image.Id);
                        }
                        
                        //Trace.WriteLine(order.Customer + ": " + order.Product, "ProcessingMessage");
                        receivedMessage.Complete();
                    }
                    catch (Exception ex)
                    {
                        
                        // Handle any message processing specific exceptions here
                    }
                });

            CompletedEvent.WaitOne();
        }
        private bool ValidateImage(Image image)
        {
            bool isValid = image.Valid; 
            if(!isValid)
            {
                System.IO.Stream imageFile =  ImageStorage.GetImageFromBlob(image.Id);
                System.Drawing.Image img = System.Drawing.Image.FromStream(imageFile);
                if (img.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                {
                    isValid = true;
                }
                
            }

            return isValid;
        }
         
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
        public void WriteLog(String msg)
        {
            //if (!EventLog.SourceExists("Application"))
              //  EventLog.CreateEventSource("Application", "ImageSharing");

            EventLog.WriteEntry("Application", msg);
        }
    }
}
