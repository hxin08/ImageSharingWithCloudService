using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using ImageSharingWithCloudService.Models;

namespace ImageSharingWithCloudService.DAL
{
    public class LogContext
    {
        public const string LOG_TABLE_NAME = "imageviews";
        protected static CloudTable table;

        public static IEnumerable<LogEntry> Select()
        {
            table = GetContext();
            TableQuery<LogEntry> query = new TableQuery<LogEntry>()
            .Where(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,DateTime.UtcNow.ToString("MMddyyy")));

            return table.ExecuteQuery(query);
            
        }

        protected static CloudTable GetContext()
        {
            CloudStorageAccount account =
                    CloudStorageAccount.Parse
                    (CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            table.CreateIfNotExists();
            return table;
        }

        public static void AddLogEntry(ApplicationUser user,ImageView image)
        {

            table = GetContext();
            LogEntry entry = new LogEntry(image.Id);
            entry.Userid = user.UserName;
            entry.Caption = image.Caption;
            entry.ImageId = image.Id;
            entry.Uri = image.Uri;


            TableOperation insertOp = TableOperation.Insert(entry);
            table.Execute(insertOp);
   
        }

    }
}