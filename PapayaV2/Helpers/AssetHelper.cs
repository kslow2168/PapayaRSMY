using PapayaX2.Database;
using PapayaX2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapayaX2.Helpers
{
    public class AssetHelper
    {
        /*
  * Function to populate the system model and related assets into a list
  */
        public static SystemModel GetSystemModel(int id)
        {
            PapayaEntities db = new PapayaEntities();
            SystemModel model = new SystemModel();
            rs_assets system = db.rs_assets.Where(x => x.AssetId == id).SingleOrDefault(); //AssetId must be unique

            if (system != null)
            {
                if (system.IsSystem)
                {
                    List<rs_assets> assets = new List<rs_assets>();
                    List<rs_assets_rel> relations = db.rs_assets_rel.Where(x => x.SysId == id).ToList();
                    foreach (rs_assets_rel rel in relations)
                    {
                        assets.Add(rel.rs_assets);
                    }
                    model.Assets = assets;
                }

                model.System = system;
            }
            return model;
        }

        public static SystemBookData GetSystemBookModel(int id)
        {
            PapayaEntities db = new PapayaEntities();
            SystemBookData model = new SystemBookData();
            rs_assets system = db.rs_assets.Where(x => x.AssetId == id).SingleOrDefault(); //AssetId must be unique

            if (system != null)
            {
                if (system.IsSystem)
                {
                    List<SystemAsset> assets = new List<SystemAsset>();
                    List<rs_assets_rel> relations = db.rs_assets_rel.Where(x => x.SysId == id).ToList();
                    foreach (rs_assets_rel rel in relations)
                    {
                        SystemAsset asset = new SystemAsset();

                        asset.SubAsset = rel.rs_assets;
                        if (rel.rs_assets.Availability == 1)
                        {
                            asset.IsSelected = true;
                        }
                        assets.Add(asset);
                    }

                    model.Assets = assets;
                }

                model.System = system;
            }
            return model;
        }

        public static List<string> GetBookedDates(int AssetId)
        {
            PapayaEntities db = new PapayaEntities();
            List<string> ret = new List<string>();

            List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == AssetId && !x.Returned).ToList();

            foreach (rs_bookings book in bookings)
            {
                for (DateTime date = book.StartDate; date.Date <= book.EndDate.Date; date = date.AddDays(1))
                {
                    string dateStr = date.ToString("d-M-yyyy");
                    ret.Add(dateStr);
                }
            }
            return ret;
        }

        public static string GenerateTrackingNo(string ownerShip, int div, bool isSystem, DateTime purchaseDate)
        {
            PapayaEntities db = new PapayaEntities();
            string ret = "";

            //Always get the FIRST character of the Ownership
            ret = string.Format("{0}{1}", ret, ownerShip.Substring(0, 1));

            //Append Division No
            ret = string.Format("{0}{1}", ret, div.ToString());

            //Append System
            ret = string.Format("{0}{1}", ret, (isSystem ? "S" : "N"));

            //Search for running no from db with same purchase date
            int count = db.rs_assets.Where(x => x.PurchaseDate == purchaseDate && x.rs_ownership.OwnerType == ownerShip && x.rs_division.DivisionNo == div).Count() + 1;

            //Append Purchase date
            ret = string.Format("{0}-{1}", ret, purchaseDate.ToString("MMyy"));

            //Append Running No
            ret = string.Format("{0}-{1}", ret, count.ToString("0000"));

            return ret;
        }


        public static bool IsImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}