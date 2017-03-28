using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;

namespace PapayaX2.Models
{
    public class SystemModel
    {
        public rs_assets System { get; set; }
        public rs_assets SubAsset { get; set; }
        public rs_accessories Accessories { get; set; }
        public List<rs_assets> Assets { get; set; }
        public int Step { get; set; }
        public int OriginLocId { get; set; }
        public int CurrentLocId { get; set; }
        public int OwnerId { get; set; }
        public int DivId { get; set; }
        public int OwnerShipId { get; set; }
        public int SubAssetId { get; set; }
        public int SystemId { get; set; }
        public int Availability { get; set; }
        public SystemModel()
        {
            System = new rs_assets();
            SubAsset = new rs_assets();
            Assets = new List<rs_assets>();
        }
    }

    public class JsonAssetModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string ImagePath { get; set; }
        public string OwnerShip { get; set; }
        public string Owner { get; set; }
        public bool Success { get; set; }

        public string ErrMsg { get; set; }
    }

    public class AssetModel
    {
        public rs_assets Asset { get; set; }
        public rs_assets Parent { get; set; }
    }

    public class AssetDisplayModel
    {
        public List<SystemModel> Assets { get; set; }

        public string SearchString { get; set; }

        public List<SearchField> SearchFields { get; set; }
    }

    public enum SearchField
    {
        SerialNumber,
        Accessories,
        Brand,
        Desciption,
        HardwareOpt,
        HardwareVer,
        MaterialNo,
        Model,
        PurchasePO,
        Remarks,
        SoftwareOpt,
        SoftwareVer,
        TrackingNo
    };
}