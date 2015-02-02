using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using ImageSharingWithCloudService.Models;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

namespace ImageSharingWithCloudService.DAL
{
    //public class ImageSharingDBInitializer: DropCreateDatabaseAlways<ImageSharingDB>
    public class ImageSharingDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(roleStore);
            UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(userStore);

            IdentityResult ir;
            ApplicationUser nobody = createUser("nobody@example.org");
            ApplicationUser jfk = createUser("jfk@example.org");
            ApplicationUser nixon = createUser("nixon@example.org");
            ApplicationUser fdr = createUser("fdr@example.org");

            ir = um.Create(nobody, "nobody1234");
           // nobody = um.FindByName(nobody.UserName);

            ir = um.Create(jfk, "jfk1234");
            //jfk = um.FindByName(jfk.UserName);

            ir = um.Create(nixon, "nixon1234");
            //nixon = um.FindByName(nixon.UserName);
            ir = um.Create(fdr, "fdr1234");

            rm.Create(new IdentityRole("User"));
            if(! um.IsInRole(nobody.Id,"User"))
            {
                um.AddToRole(nobody.Id, "User");
            }
            if (!um.IsInRole(jfk.Id, "User"))
            {
                um.AddToRole(jfk.Id, "User");
            }
            if (!um.IsInRole(nixon.Id, "User"))
            {
                um.AddToRole(nixon.Id, "User");
            }
            if (!um.IsInRole(fdr.Id, "User"))
            {
                um.AddToRole(fdr.Id, "User");
            }

            rm.Create(new IdentityRole("Admin"));
            if (!um.IsInRole(nixon.Id, "Admin"))
            {
                um.AddToRole(nixon.Id, "Admin");
            }

            rm.Create(new IdentityRole("Approver"));
            if (!um.IsInRole(jfk.Id, "Approver"))
            {
                um.AddToRole(jfk.Id, "Approver");
            }

            rm.Create(new IdentityRole("Supervisor"));
            if (!um.IsInRole(fdr.Id, "Supervisor"))
            {
                um.AddToRole(fdr.Id, "Supervisor");
            }

            db.Tags.Add(new Tag { Name = "portrait" });
            db.Tags.Add(new Tag { Name = "landscape" });

            db.SaveChanges();

            /*
            db.Images.Add(new Image
            {
                Caption = "Maradona",
                Description = "Diego Maradona, best football player we have ever known",
                DateTaken = new DateTime(2014, 01, 01),
                UserId = jfk.Id,
                TagId = 1,
                Approved = true
            });
            */
            //LogContext.CreateTable();
            base.Seed(db);
        }
        private ApplicationUser createUser(String userName)
        {
            return new ApplicationUser { UserName = userName, Email = userName };
        }
    }
}