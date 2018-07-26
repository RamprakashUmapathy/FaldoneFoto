using JsonExtensions;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private IEnumerable<Category> Data { get; set; }

        public string Search { get; set; }

        public string SearchIn { get; set; }

        public string SearchOption { get; set; }

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

        public IEnumerable<Style> StyleItems { get; private set; }

        public double PriceRangeFrom { get; set; }

        public double PriceRangeTo { get; set; }

        public double DiscountFrom { get; set; }

        public double DiscountTo { get; set; }

        public double StockQty { get; set; }

        public bool IsCardView { get; set; }

        //public List<ArticleLightViewModel> Articles { get; set; }

        public HomeViewModel()
        {
            Data = new List<Category>();
            IsCardView = true;
        }

        public async Task<HomeViewModel> Load()
        {
            HttpResponseMessage response = await Client.GetAsync("Categories");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                this.Data = JsonConvert.DeserializeObject<IEnumerable<Category>>(data, settings);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            string emptyItemText = await GetItem("EmptyItemSelect");
            var emptyItem = new SelectListItem() { Text = emptyItemText, Value = "" };
            var emptyList = new List<SelectListItem>()  { emptyItem };

            this.EmptyItemText = emptyItemText;
            this.Categories = emptyList;
            this.Families = emptyList;
            this.Series = emptyList;
            this.Level1s = emptyList;
            this.Level2s = emptyList;
            this.PrivateLabelItems = emptyList;
            this.Categories = Data.ToSelectListItems(CategoryId, emptyItem);

            this.PrivateLabelItems = await GetItems("PrivateLabel", PrivateLabelId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.WebEnabledItems = await GetItems("WebEnabled", WebEnabledId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.VideoAvailableItems = await GetItems("VideoAvailable", VideoAvailableId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.MandatoryDeliveryItems = await GetItems("MandatoryDelivery", MandatoryDeliveryId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.WareHouseItems = await GetItems("WareHouseName", WareHouseNameId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.DirectDeliveryItems = await GetItems("DirectDelivery", DirectDeliveryId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.ChalcoItems = await GetItems("InChalco", ChalcoId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.PhotoItems = await GetItems("HasPhoto", PhotoId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.ColorItems = await GetItems("Color", ColorId, new SelectListItem() { Text = EmptyItemText, Value = "" });
            this.StyleItems = Data.GetStyles(); //.ToSelectListItems(StyleId, emptyItem);
            this.PriceListItems = Data.GetPriceLists();
            this.SupplyStatusItems = Data.GetSupplyStatuses();
            this.StockGroupItems = Data.GetStockGroups();

            return this;
        }

        private async Task<string> GetItem(string keyId)
        {
            using (var response = await Client.GetAsync("KeyItems/" + keyId))
            {
                if (response.IsSuccessStatusCode)
                {
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
                else
                {
                    await Task.Delay(1);
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetItems(string keyId, string selectedItems, SelectListItem emptyItem)
        {
            using (var response = await Client.GetAsync("KeyItems/" + keyId))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new PrivateSetterContractResolver()
                    };
                    var itemValues = JsonConvert.DeserializeObject<IEnumerable<KeyItemValue>>(data, settings);
                    return itemValues.ToSelectedListItems(selectedItems, emptyItem);
                }
                else
                {
                    await Task.Delay(1);
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
