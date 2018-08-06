using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{

    internal static class UtilityExtensions
    {

        public static bool Contains(this IEnumerable<Family> coll, string seriesId)
        {
            return coll.Any(c => c.Id == seriesId);
        }

        public static IEnumerable<BaseEntity<string>> GetEmpty(this IEnumerable<BaseEntity<string>> coll)
        {
            return new List<BaseEntity<string>>();
        }

        public static IEnumerable<Family> GetFamilies(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families);
        }

        public static IEnumerable<Family> GetFamilies(this IEnumerable<Category> coll, string categoryId)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            var category = coll.Where(f => f.Id == categoryId).SingleOrDefault();
            return (category != null ? category.Families : new List<Family>());
        }

        public static IEnumerable<Series> GetSeries(this IEnumerable<Category> coll, string categoryId, string familyId)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.GetFamilies(categoryId).GetSeries(familyId);
        }

        public static IEnumerable<Series> GetSeries(this IEnumerable<Family> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(f => f.Series);
        }

        public static IEnumerable<Series> GetSeries(this IEnumerable<Family> coll,string familyId)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.Where(f => f.Id == familyId).GetSeries();
        }

        public static IEnumerable<Level1> GetLevel1s(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s);
        }

        public static IEnumerable<Level1> GetLevel1s(this IEnumerable<Family> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s);
        }

        public static IEnumerable<Level1> GetLevel1s(this IEnumerable<Series> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(s => s.Level1s);
        }

        public static IEnumerable<Level1> GetLevel1s(this IEnumerable<Category> coll, string categoryId, string familyId, string seriesId)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.GetFamilies(categoryId).GetSeries(familyId)
                            .GetLevel1s();
        }

        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s);
        }

        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Category> coll, string categoryId, string familyId, string seriesId, string level1Id )
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.GetLevel1s(categoryId, familyId, seriesId).GetLevel2s(level1Id);
        }
        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Family> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s);
        }

        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Series> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s);

        }

        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Level1> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(l1 => l1.Level2s);
        }

        public static IEnumerable<Level2> GetLevel2s(this IEnumerable<Level1> coll, string level1Id)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            var level1 = coll.Where(f => f.Id == level1Id).SingleOrDefault();
            return (level1 != null ? level1.Level2s : new List<Level2>());
        }

        public static IEnumerable<Style> GetStyles(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s)
                       .SelectMany(l2 => l2.Styles).Distinct();
        }

        public static IEnumerable<Style> GetStyles(this IEnumerable<Level2> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(l2 => l2.Styles).Distinct();
        }

        public static IEnumerable<PriceList> GetPriceLists(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s)
                       .SelectMany(l2 => l2.PriceLists).Distinct();
        }

        public static IEnumerable<PriceList> GetPriceLists(this IEnumerable<Level2> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(l2 => l2.PriceLists).Distinct();
        }

        public static IEnumerable<StockGroup> GetStockGroups(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s)
                       .SelectMany(l2 => l2.StockGroups).Distinct();
        }


        public static IEnumerable<StockGroup> GetStockGroups(this IEnumerable<Level2> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(l2 => l2.StockGroups).Distinct();
        }

        public static IEnumerable<SupplyStatus> GetSupplyStatuses(this IEnumerable<Category> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(c => c.Families)
                       .SelectMany(f => f.Series)
                       .SelectMany(s => s.Level1s)
                       .SelectMany(l1 => l1.Level2s)
                       .SelectMany(l2 => l2.SupplyStatuses).Distinct();
        }

        public static IEnumerable<SupplyStatus> GetSupplyStatuses(this IEnumerable<Level2> coll)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            return coll.SelectMany(l2 => l2.SupplyStatuses).Distinct();
        }


    }
}
