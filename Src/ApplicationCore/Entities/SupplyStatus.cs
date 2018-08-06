using System;
using System.Collections.Generic;
using System.Text;

namespace Kasanova.ApplicationCore.Entities
{
    public class StockGroup
    {
        public string ArticleId { get; private set; }

        public string StockGroupId { get; set; }

        public string LastStatusId { get; set; }
    }
}
