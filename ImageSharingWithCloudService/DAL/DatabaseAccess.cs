using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageSharingWithCloudService.Models;

namespace ImageSharingWithCloudService.DAL
{
    public class DatabaseAccess
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void UpdateImageInDB(Image image)
        {
            try
            {
                db.Entry(image).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
        public static void DeleteImageFromDB(Image image)
        {
            db.Images.Remove(image);
            db.SaveChanges();
        }

    }
}