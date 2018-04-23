using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

public class ResourceFiller {
    public static string[] Users = new string[] { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" };
    public static string[] Usernames = new string[] { "sbrighton", "rfischer", "amiller" };

    public static void FillResources(ASPxSchedulerStorage storage, int count) {
        ResourceCollection resources = storage.Resources.Items;
        storage.BeginUpdate();
        try {
            int cnt = Math.Min(count, Users.Length);
            for (int i = 1; i <= cnt; i++) {
                resources.Add(new Resource(Usernames[i - 1], Users[i - 1]));
            }
        }
        finally {
            storage.EndUpdate();
        }
    }
}
