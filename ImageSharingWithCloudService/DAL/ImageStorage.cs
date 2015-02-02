using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core;
using ImageSharingWithCloudService.Models;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.IO;

namespace ImageSharingWithCloudService.DAL
{
    public class ImageStorage
    {
        
        public const bool USE_BLOB_STORAGE = true;

        public const string ACCOUNT = "imagesharing";
        public const string CONTAINER = "images";

        private static CloudBlobContainer ConnectToStorage()
        {
            CloudStorageAccount account =
                    CloudStorageAccount.Parse
                    (CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(CONTAINER);
            return container;
        }

        public static void SaveFile(HttpServerUtilityBase server,
                               HttpPostedFileBase imageFile,
                               int imageId)
        {
            if (USE_BLOB_STORAGE)
            {

                CloudBlobContainer container = ConnectToStorage();
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(server, imageId));

                
                byte[] b = new byte[imageFile.InputStream.Length];
                imageFile.InputStream.Read(b,0,b.Length);
                //blob.UploadFromStream(imageFile.InputStream);
                blob.UploadFromByteArray(b, 0, b.Length);
            }
            else
            {
                 string imgFileName = FilePath(server, imageId);
                 imageFile.SaveAs(imgFileName);
            }
          
        }
        public static void DeleteImageFromBlob(int imageId)
        {
            if (USE_BLOB_STORAGE)
            {

                CloudBlobContainer container = ConnectToStorage();
                
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(null, imageId));
                blob.Delete();
                
            }
        }
        public static Stream GetImageFromBlob(int imageId)
        {

            String imageName = FileName(imageId);
            CloudBlobContainer container = ConnectToStorage();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
            MemoryStream image = new MemoryStream();
            blockBlob.DownloadToStream(image);
            return image;
        }

        public static string FilePath(HttpServerUtilityBase server,
                                       int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                return FileName(imageId);
            }
            else
            {
                string imgFileName = server.MapPath("~/Images/" + FileName(imageId));
                return imgFileName;
            }

        }

        public static string FileName(int imageId)
        {
            return imageId + ".jpg";
        }

        public static string ImageURI(UrlHelper urlHelper, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                return "https://" + ACCOUNT + ".blob.core.windows.net/" + CONTAINER + "/" + FileName(imageId);
            }
            else
            {
                return urlHelper.Content("~/Images/" + FileName(imageId));
            }
            
        }
    }
}