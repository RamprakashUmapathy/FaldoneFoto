using AutoMapper;
using JsonExtensions;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        internal IMapper Mapper { get; set; }

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

        public IEnumerable<SelectListItem> PriceListItems { get; private set; }

        public string SupplyStatusId { get; set; }

        public IEnumerable<SelectListItem> SupplyStatusItems { get; private set; }

        public string StockGroupId { get; set; }

        public IEnumerable<SelectListItem> StockGroupItems { get; private set; }

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

        public List<ArticleCardViewModel> Articles { get; set; }

        public string PhotoBaseUrl { get; set; }

        public HomeViewModel()
        {
            Data = new List<ShopSign>();
            Articles = new List<ArticleCardViewModel>();
            IsCardView = true;
        }

        public async Task<HomeViewModel> LoadAsync(bool isPostBack)
        {
            string emptyItemText = await GetItem("EmptyItemSelect");
            var emptyItem = new SelectListItem() { Text = emptyItemText, Value = "" };
            EmptyItemText = emptyItemText;

            //Load combo shop signs
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
            PriceListItems = new List<SelectListItem>();  //Data.GetPriceLists();
            SupplyStatusItems = new List<SelectListItem>(); //Data.GetSupplyStatuses();
            StockGroupItems = new List<SelectListItem>();// Data.GetStockGroups();

            if (isPostBack == true)
            {
                //Load Articles
                //{ shopsignid}/{ categoryid}/{familyid?}/{seriesid?}/{level1id?}/{level2id?}/{styleid?}
                string uri = $"articles/{ShopSignId}";
                if(!String.IsNullOrEmpty(CategoryId))
                     uri = uri + $"/{CategoryId}";
                if (!String.IsNullOrEmpty(FamilyId))
                    uri = uri + $"/{FamilyId}";
                if (!String.IsNullOrEmpty(SeriesId))
                    uri = uri + $"/{SeriesId}";
                if (!String.IsNullOrEmpty(Level1Id))
                    uri = uri + $"/{Level1Id}";
                if (!String.IsNullOrEmpty(Level2Id))
                    uri = uri + $"/{Level2Id}";
                if (!String.IsNullOrEmpty(StyleId))
                    uri = uri + $"/{StyleId}";
                
                using (HttpResponseMessage response = await Client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new PrivateSetterContractResolver()
                    };
                    var articles = JsonConvert.DeserializeObject<List<ChalcoArticle>>(data, settings);
                    articles.ForEach(a =>
                    {
                        var vm = Mapper.Map<ChalcoArticle, ArticleCardViewModel>(a);
                        Articles.Add(vm);
                    });
                }
            }
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
