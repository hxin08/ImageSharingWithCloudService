using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithCloudService.Models
{
    public class SelectItemView
    {
        //
        // GET: /SelectItemView/
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public SelectItemView(int id, string name, bool c)
        {
            this.Id = id;
            this.Name = name;
            this.Checked = c;
        }

        public SelectItemView() { }
    }
}
