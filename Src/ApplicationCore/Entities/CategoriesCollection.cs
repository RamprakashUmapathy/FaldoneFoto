using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{


    public class CategoriesCollection : IEnumerable<Category>
    {
        public class ResultSet
        {
            public string CategoryId { get; set; }
            public string FamilyId { get; set; }
            public string SeriesId { get; set; }
            public string Level1Id { get; set; }
            public string Level2Id { get; set; }
            public string StyleId { get; set; }
            public string PriceListId { get; set; }
            public string StockGroupId { get; set; }
            public string SupplyingStatusId { get; set; }
            public string TagId { get; set; }

        }

        private static SortedList<string, Category> _list = new SortedList<string, Category>();

        private CategoriesCollection()
        {
            _list = new SortedList<string, Category>();
        }

        public IEnumerator<Category> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        public static IEnumerable<Category> BuildTree(IEnumerable<ResultSet> results)
        {
            Category category = null;
            Family family = null;
            Series series = null;
            Level1 level1 = null;
            Level2 level2 = null;

            foreach (var result in results)
            {
                string categoryId = result.CategoryId;
                string familyId = result.FamilyId;
                string seriesId = result.SeriesId;
                string level1Id = result.Level1Id;
                string level2Id = result.Level2Id;
                string styleId = result.StyleId;
                string priceListId = result.PriceListId;
                string stockGroupId = result.StockGroupId;
                string supplyStatusId = result.SupplyingStatusId;
                if(!_list.ContainsKey(categoryId))
                {
                    category = new Category() { Id = categoryId };
                    _list.Add(categoryId, category);
                }
                else
                {
                    category = _list[categoryId];
                }
                family = category.CreateOrGet(familyId);
                series = family.CreateOrGet(seriesId);
                level1 = series.CreateOrGet(level1Id);
                level2 = level1.CreateOrGet(level2Id);
                level2.CreateOrGet(styleId);
                level2.CreateOrGet(new PriceList() { Id = priceListId });
                level2.CreateOrGet(new StockGroup() { Id = stockGroupId });
                level2.CreateOrGet(new SupplyStatus() { Id = supplyStatusId });
            }

            return _list.Values;
        }
    }
}
