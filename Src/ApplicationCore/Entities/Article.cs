using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public class Article : NameEntity<string>
    {
        /// <summary>
        /// Returns the item category
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// returns the item family
        /// </summary>
        public string Family { get; private set; }

        /// <summary>\
        /// returns the item Series
        /// </summary>
        public string Series { get; private set; }

        /// <summary>
        /// returns the Level1
        /// </summary>
        public string Level1 { get; private set; }

        /// <summary>
        /// returns the Level2
        /// </summary>
        public string Level2 { get; private set; }

        /// <summary>
        /// returns the style
        /// </summary>
        public string Style { get; private set; }

        /// <summary>
        /// Returns the alias name of the article 
        /// </summary>
        public string NameAlias { get; private set; }

        public string Line { get; private set; }

        public string BrandId { get; private set; }
        /// <summary>
        /// Country of origin
        /// </summary>
        public string OriginCountryId { get; private set; }

        public bool HasPhoto { get; private set; }

        public string ItemBarCode { get; private set; }

        public bool IsPrivateLabel { get; private set; }

        public bool IsDirectDelivery { get; private set; }

        public bool IsMandatoryPickup { get; private set; }

        public bool InMagento { get; private set; }

        public bool InAmazon { get ; private set; }

        public bool InEBay { get; private set; }

        public int StockQuantity { get; private set; }

        public int MasterQuantity { get; private set; }

        public string VendorAccount { get; private set; }

        public string PrimaryVendorAccount { get; private set; }

        public List<Kasanova.ApplicationCore.Entities.StockGroup> StockGroups { get; private set; }

        public List<PriceList> PriceLists { get; private set; }

        public string PriceListNames{ get { return string.Join(',', PriceLists.Select(f => f.PriceListId)); } }

        public IEnumerable<SupplyStatus> SupplyStatuses { get; private set; }

        public string TagName { get; private set; }

        public Article()
        {
            PriceLists = new List<PriceList>();
            StockGroups = new List<Kasanova.ApplicationCore.Entities.StockGroup>();
        }


    }

}
