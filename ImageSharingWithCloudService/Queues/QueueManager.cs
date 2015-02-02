using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageSharingWithCloudService.Models;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ImageSharingWithCloudService.Queues
{
    public class QueueManager
    {
        public const string UserMessagesQueueName = "UserMessagesQueue";
        public static void SendImageEntityToQueue(Image image)
        {
            // Retrieve storage account from connection string.
            CloudQueue queue = CreateQueue();

            // Create a message and add it to the queue.
            String msg = "Image: "+image.Caption + "is";
            if(image.Valid)
            {
                msg += " Validated";
            }
            if(image.Valid)
            {
                msg += " Approved";
            }
            
            CloudQueueMessage message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }
        public static CloudQueue ConnectToQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(UserMessagesQueueName);
            

            return queue;
            
        }
        public static CloudQueue CreateQueue()
        {
            CloudQueue queue =  ConnectToQueue();
            queue.CreateIfNotExists();
            return queue;
        }
        public static List<QueueMessage> ReadFromQueue()
        {
            CloudQueue queue = ConnectToQueue();

            IEnumerable<CloudQueueMessage> retrievedMessages = queue.GetMessages(100);
            List<QueueMessage> messages = new List<QueueMessage>();
            foreach( CloudQueueMessage message in retrievedMessages)
            {
                QueueMessage m = new QueueMessage();
                m.Id = message.Id;
                m.InsertionTime = message.InsertionTime.ToString();
                m.Message = message.AsString;
                messages.Add(m);
                queue.DeleteMessage(message);
            }
            return messages;
        }
    }
}