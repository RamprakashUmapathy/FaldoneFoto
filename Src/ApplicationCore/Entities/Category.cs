using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public class ShopSign : BaseEntity<string>, IEquatable<ShopSign>
    {
        private List<Category> _list;

        public ShopSign()
        {
            _list = new List<Category>();
        }

        public List<Category> Categories
        {
            get
            {
                return _list;
            }
            private set
            {
                _list = value;
            }
        }

        public Category CreateOrGet(string categoryId)
        {
            if (categoryId == null) throw new ArgumentNullException();
            Category cat = new Category() { Id = categoryId };
            int idx = _list.IndexOf(cat);
            if (idx == -1)
            {
                _list.Add(cat);
            }
            else
            {
                cat = _list[idx];
            }
            return cat;
        }

        public bool Equals(ShopSign other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Category : BaseEntity<string>, IEquatable<Category>
    {

        private List<Family> _list;

        public Category()
        {
            _list = new List<Family>();

        }

        public IEnumerable<Family> Families
        {
            get
            {
                return _list.AsReadOnly();
            }
            private set
            {
                //value.ToList().ForEach(f =>
                //{
                //    CreateOrGet(f.Id);
                //});
                _list = value.ToList();
            }
        }

        public Family CreateOrGet(string familyId)
        {
            if (familyId == null) throw new ArgumentNullException();
            Family family = new Family() { Id = familyId };
            int idx = _list.IndexOf(family);
            if (idx == -1)
            {
                _list.Add(family);
            }
            else
            {
                family = _list[idx];
            }
            return family;
        }

        public bool Equals(Category other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Family : BaseEntity<string>, IEquatable<Family>
    {

        private List<Series> _list = null;

        public Family()
        {
            _list = new List<Series>();
        }

        public IEnumerable<Series> Series
        {
            get
            {
                return _list.AsReadOnly();
            }
            private set
            {
                _list = value.ToList(); 
            }
        }

        public Series CreateOrGet(string seriesId)
        {
            if (seriesId == null) throw new ArgumentNullException();
            Series series = new Series() { Id = seriesId };
            int idx = _list.IndexOf(series);
            if (idx == -1)
            {
                _list.Add(series);
            }
            else
            {
                series = _list[idx];
            }
            return series;
        }

        public bool Equals(Family other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Series : BaseEntity<string>, IEquatable<Series>
    {

        private List<Level1> _list;

        public Series()
        {
            _list = new List<Level1>();
        }

        public IEnumerable<Level1> Level1s
        {
            get
            {
                return _list.AsReadOnly();
            }
            private set
            {
                _list = value.ToList(); //value.ToList().ForEach(f => CreateOrGet(f.Id));
            }
        }

        public Level1 CreateOrGet(string level1Id)
        {
            if (level1Id == null) throw new ArgumentNullException();
            Level1 level1 = new Level1() { Id = level1Id };
            if (!_list.Contains(level1))
            {
                _list.Add(level1);
            }
            return level1;
        }

        public bool Equals(Series other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Level1 : BaseEntity<string>, IEquatable<Level1>
    {

        private List<Level2> _list;

        public Level1()
        {
            _list = new List<Level2>();
        }

        public IEnumerable<Level2> Level2s
        {
            get
            {
                return _list.AsReadOnly();
            }
            private set
            {
                _list = value.ToList(); //value.ToList().ForEach(f => CreateOrGet(f.Id));
            }
        }

        public Level2 CreateOrGet(string level2Id)
        {
            if (level2Id == null) throw new ArgumentNullException();
            Level2 level2 = new Level2() { Id = level2Id };
            if (!_list.Contains<Level2>(level2))
            {
                _list.Add(level2);
            }
            return level2;
        }

        public bool Equals(Level1 other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

    }

    public class Level2 : BaseEntity<string>, IEquatable<Level2>
    {



        public Level2()
        {
            _styles = new List<Style>();
            _priceLists = new List<PriceList>();
            _stockGroups = new List<StockGroup>();
            _supplyStatuses = new List<SupplyStatus>();
            _tags = new List<Tag>();
        }

        private List<Style> _styles;
        public IEnumerable<Style> Styles
        {
            get
            {
                return _styles.AsReadOnly();
            }
            private set
            {
                _styles = value.ToList();
            }
        }

        private List<PriceList> _priceLists;
        public IEnumerable<PriceList> PriceLists
        {
            get
            {
                return _priceLists.AsReadOnly();
            }
            private set
            {
                _priceLists = value.ToList();
            }
        }

        private List<StockGroup> _stockGroups;
        public IEnumerable<StockGroup> StockGroups
        {
            get
            {
                return _stockGroups.AsReadOnly();
            }
            private set
            {
                _stockGroups = value.ToList();
            }

        }

        private List<SupplyStatus> _supplyStatuses;
        public IEnumerable<SupplyStatus> SupplyStatuses
        {
            get
            {
                return _supplyStatuses.AsReadOnly();
            }
            set
            {
                _supplyStatuses = value.ToList();
            }
        }

        private List<Tag> _tags;
        public IEnumerable<Tag> Tags
        {
            get
            {
                return _tags.AsReadOnly();
            }
            private set
            {
                _tags = value.ToList();
            }
        }

        public Style CreateOrGet(string styleId)
        {
            if (styleId == null) throw new ArgumentNullException();
            Style style = new Style() { Id = styleId };
            int idx = _styles.IndexOf(style);
            if (idx == -1)
            {
                _styles.Add(style);
            }
            else
            {
                style = _styles[idx];
            }
            return style;
        }

        public PriceList CreateOrGet(PriceList priceList)
        {
            if (priceList == null) throw new ArgumentNullException();
            if (!_priceLists.Contains(priceList))
            {
                _priceLists.Add(priceList);
            }
            return priceList;
        }

        public StockGroup CreateOrGet(StockGroup stockGroup)
        {
            if (stockGroup == null) throw new ArgumentNullException();
            if (!_stockGroups.Contains(stockGroup))
            {
                _stockGroups.Add(stockGroup);
            }
            return stockGroup;
        }

        public SupplyStatus CreateOrGet(SupplyStatus supplyStatus)
        {
            if (supplyStatus == null) throw new ArgumentNullException();
            if (!_supplyStatuses.Contains(supplyStatus))
            {
                _supplyStatuses.Add(supplyStatus);
            }
            return supplyStatus;
        }

        public Tag CreateOrGet(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException();
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
            return tag;
        }

        public bool Equals(Level2 other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Style : BaseEntity<string>, IEquatable<Style>
    {
        public Style()
        {

        }

        public bool Equals(Style other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    //public class PriceList : BaseEntity<string>, IEquatable<PriceList>
    //{
    //    public PriceList()
    //    {

    //    }

    //    public bool Equals(PriceList other)
    //    {
    //        if (other == null) return false;
    //        return this.Id == other.Id;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return this.Id.GetHashCode();
    //    }
    //}

    public class StockGroup : BaseEntity<string>, IEquatable<StockGroup>
    {
        public StockGroup()
        {

        }
        public bool Equals(StockGroup other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class SupplyStatus : BaseEntity<string>, IEquatable<SupplyStatus>
    {
        public SupplyStatus()
        {

        }
        public bool Equals(SupplyStatus other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class Tag : BaseEntity<string>, IEquatable<Tag>
    {

        public Tag()
        {

        }

        public bool Equals(Tag other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
