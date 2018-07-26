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

        public bool HasPhotoInChalco { get; private set; }

        public string SupplyStatusId { get; private set; }

        public string ItemBarCode { get; private set; }

        public bool IsPrivateLabel { get; private set; }

        public bool IsDirectDelivery { get; private set; }

        public bool IsMandatoryPickup { get; private set; }

        public bool InMagento { get; private set; }

        public bool InAmazon { get; private set; }

        public bool InEBay { get; private set; }

        public int StockQuantity { get; private set; }

        public int MasterQuantity { get; private set; }

        public string VendorAccount { get; private set; }

        public string PrimaryVendorAccount { get; private set; }

        public IEnumerable<string> StockGroups { get; private set; }

        public IEnumerable<ArticlePriceList> PriceLists { get; private set; }


    }

    public class ArticlePriceList
    {
        public string  PriceListId { get; private set; }

        public decimal Price { get; private set; }

        public decimal NetPrice { get; private set; }

        public double DiscountPercent { get; private set; }
    }

    public class ChalcoArticle : Article
    {
        public string  Color { get; private set; }

        public string WebColor { get; private set; }

        public string YoutubeVideo { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public double Depth { get; private set; }

        public double Weight { get; private set; }

        public double PackWidth { get; private set; }

        public double PackHeight { get; private set; }

        public double PackDepth { get; private set; }

        public double PackWeight { get; private set; }

        public bool Fragile { get; private set; }

        public bool Packaging { get; private set; }

        public bool Delivery { get; private set; }

        public string SubTitle { get; private set; }

        public new string Description { get; private set; }

        public string ChalcoDescriptionShort { get; private set; }

        public string Materials { get; private set; }

    }

}
