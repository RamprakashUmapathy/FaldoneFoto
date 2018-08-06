using System;
using System.Collections.Generic;
using System.Text;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public class PriceList : BaseEntity<string>, IEquatable<PriceList>
    {
        public PriceList()
        {

        }

        public bool Equals(PriceList other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public string ArticleId { get; private set; }

        public string PriceListId { get; private set; }

        public decimal GrossSalesPrice { get; private set; }

    }
}
