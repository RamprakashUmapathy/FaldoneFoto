using JsonExtensions;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.ViewModels
{
    public class HomeViewModel : IDisposable
    {
        public string EmptyItemText { get; private set; }

        public HttpClient Client { get; set; }

        private IEnumerable<ShopSign> Data { get; set; }

        public string Search { get; set; }

        public string SearchIn { get; set; }

        public string SearchOption { get; set; }

        private string _shopSignId;
        public string ShopSignId
        {
            get { return _shopSignId;  } 
            set { _shopSignId = value; }
        }

        public IEnumerable<SelectListItem> ShopSigns { get; private set; }

        public string CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; private set; }

        public string FamilyId { get; set; }

        public IEnumerable<SelectListItem> Families { get; private set; }

        public string SeriesId { get; set; }

        public IEnumerable<SelectListItem> Series { get; private set; }

        public string Level1Id { get; set; }

        public IEnumerable<SelectListItem> Level1s { get; private set; }

        public string Level2Id { get; set; }

        public IEnumerable<SelectListItem> Level2s { get; private set; }

        public string PrivateLabelId { get; set; }

        public IEnumerable<SelectListItem> PrivateLabelItems { get; private set; }

        public string WebEnabledId { get; set; }

        public IEnumerable<SelectListItem> WebEnabledItems { get; private set; }

        public string VideoAvailableId { get; set; }

        public IEnumerable<SelectListItem> VideoAvailableItems { get; private set; }

        public string MandatoryDeliveryId { get; set; }

        public IEnumerable<SelectListItem> MandatoryDeliveryItems { get; private set; }

        public string WareHouseNameId { get; set; }

        public IEnumerable<SelectListItem> WareHouseItems { get; private set; }

        public string DirectDeliveryId { get; set; }

        public IEnumerable<SelectListItem> DirectDeliveryItems { get; private set; }

        public string ChalcoId { get; set; }

        public IEnumerable<SelectListItem> ChalcoItems { get; private set; }

        public string PhotoId { get; set; }

        public IEnumerable<SelectListItem> PhotoItems { get; private set; }

        public string ColorId { get; set; }

        public IEnumerable<SelectListItem> ColorItems { get; private set; }

        public string PriceListId { get; set; }

        public IEnumerable<PriceList> PriceListItems { get; private set; }

        public string SupplyStatusId { get; set; }

        public IEnumerable<SupplyStatus> SupplyStatusItems { get; private set; }

        public string StockGroupId { get; set; }

        public IEnumerable<StockGroup> StockGroupItems { get; private set; }

        public string TagId { get; set; }

        public IEnumerable<Tag> TagItems { get; private set; }

        public string StyleId { get; set; }

        public IEnumerable<SelectListItem> StyleItems { get; private set; }

        public double PriceRangeFrom { get; set; }

        public double PriceRangeTo { get; set; }

        public double DiscountFrom { get; set; }

        public double DiscountTo { get; set; }

        public double StockQty { get; set; }

        public bool IsCardView { get; set; }

        //public List<ArticleLightViewModel> Articles { get; set; }

        public HomeViewModel()
        {
            Data = new List<ShopSign>();
            IsCardView = true;
        }

        public async Task<HomeViewModel> Load(bool post)
        {
            string emptyItemText = await GetItem("EmptyItemSelect");
            var emptyItem = new SelectListItem() { Text = emptyItemText, Value = "" };
            EmptyItemText = emptyItemText;

            ShopSigns = await GetItems("ShopSigns", ShopSignId, emptyItem);

            using (HttpResponseMessage response = await Client.GetAsync("shopsigns"))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                Data = JsonConvert.DeserializeObject<IEnumerable<ShopSign>>(data, settings);
            }

            ShopSigns = Data.ToSelectListItems(ShopSignId, emptyItem);
            Categories = Data.GetCategories(ShopSignId).ToSelectListItems(CategoryId, emptyItem);
            Families = Data.GetFamilies(ShopSignId, CategoryId).ToSelectListItems(FamilyId, emptyItem);
            Series = Data.GetSeries(ShopSignId, CategoryId, FamilyId).ToSelectListItems(SeriesId, emptyItem);
            Level1s = Data.GetLevel1s(ShopSignId, CategoryId, FamilyId, SeriesId).ToSelectListItems(Level1Id, emptyItem);
            Level2s = Data.GetLevel2s(ShopSignId, CategoryId, FamilyId, SeriesId, Level1Id).ToSelectListItems(Level2Id, emptyItem); 

            PrivateLabelItems = await GetItems("PrivateLabel", PrivateLabelId, emptyItem);
            WebEnabledItems = await GetItems("WebEnabled", WebEnabledId, emptyItem);
            VideoAvailableItems = await GetItems("VideoAvailable", VideoAvailableId, emptyItem);
            MandatoryDeliveryItems = await GetItems("MandatoryDelivery", MandatoryDeliveryId, emptyItem);
            WareHouseItems = await GetItems("WareHouseName", WareHouseNameId, emptyItem);
            DirectDeliveryItems = await GetItems("DirectDelivery", DirectDeliveryId, emptyItem);
            ChalcoItems = await GetItems("InChalco", ChalcoId, emptyItem);
            PhotoItems = await GetItems("HasPhoto", PhotoId, emptyItem);
            ColorItems = await GetItems("Color", ColorId, emptyItem);
            StyleItems = Data.GetStyles().ToSelectListItems(StyleId); //.ToSelectListItems(StyleId, emptyItem);
            PriceListItems = new List<PriceList>();  //Data.GetPriceLists();
            SupplyStatusItems = new List<SupplyStatus>(); //Data.GetSupplyStatuses();
            StockGroupItems = new List<StockGroup>();// Data.GetStockGroups();

            return this;
        }

        private async Task<string> GetItem(string keyId)
        {
            using (var response = await Client.GetAsync("KeyItems/" + keyId))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                var itemValues = JsonConvert.DeserializeObject<IEnumerable<KeyItemValue>>(data, settings);
                if (itemValues.Any())
                {
                    return itemValues.First().Description;
                }
                return null;
            }
        }

        /// <summary>
        /// Get items from database using a specific key
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="selectedItems"></param>
        /// <param name="emptyItem"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetItems(string keyId, string selectedItems, SelectListItem emptyItem)
        {
            using (var response = await Client.GetAsync("KeyItems/" + keyId))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                var itemValues = JsonConvert.DeserializeObject<IEnumerable<KeyItemValue>>(data, settings);
                return itemValues.ToSelectedListItems(selectedItems, emptyItem);
            }
        }

        public void Dispose()
        {
            Client.Dispose();
            Debug.WriteLine("Dispose called");
        }
    }
}
