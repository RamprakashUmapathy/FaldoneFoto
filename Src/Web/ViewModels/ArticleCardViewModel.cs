using AutoMapper;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{

    public class ArticleCardViewModel 
    {

        private const string PhotoBaseUrl = "http://192.168.146.249:10081/GetItemPhoto.ashx?CodiceArticolo={0}";

        public string Id { get; private set; }

        public string Description { get; private set; }

        public string NameAlias { get; private set; }

        public string Materials { get; private set; }

        public string Line { get; private set; }

        public string PhotoUrl { get { return string.Format(PhotoBaseUrl, Id); } }

        public string LargePhotoUrl { get { return string.Concat(PhotoUrl, "&", "UseNoThumb=1"); } }

        public double Height { get; private set; }

        public double Depth { get; private set; }

        public double Width { get; private set; }

        public double Weight { get; private set; }

        public bool HasPhotoInChalco { get; private set; }

        public string PurchasePrice { get { return "List Acq. € 9,86"; } }

        public string Price { get { return "€ 29,00"; } }

        public string NetPrice { get { return "€ 16,86"; } }

        public string DiscountPercent { get { return " 40,00%"; } }

        public string StockArrivalDate { get; private set; }

        public string StockArrivalQty { get; private set; }

        public string YoutubeVideo { get; private set; }


        public string Chalco
        {
            get
            {
                if (_chalco == null)
                {
                    _chalco = HasPhotoInChalco ? "~/Images/camera.png" : "~/Images/blank.gif";
                }
                return _chalco;
            }
            set
            {
                _chalco = value;
            }
        }
        private string _chalco;

        private string _dimensions;
        public string Dimensions
        {
            get
            {
                if (_dimensions == null)
                {
                    if (this.Width > 0 || this.Depth > 0 || this.Height > 0)
                    {
                        _dimensions = String.Format("{0:N0} x {1:N0} x {2:N0} (Cm)", this.Width, this.Depth, this.Height);
                    }
                    else
                    {
                        _dimensions = "";
                    }
                }
                return _dimensions;
            }
        }

        private string _weightInString;
        public string WeightInString
        {
            get
            {
                if (_weightInString == null)
                {
                    _weightInString = (Weight > 0) ? string.Format("{0:######,###} g", Weight) : "";
                }
                return _weightInString;
            }
        }

        private bool? _hasVideo;
        public bool HasVideo
        {
            get
            {
                if (!_hasVideo.HasValue)
                {
                    _hasVideo = YoutubeVideo != null && YoutubeVideo.Length > 0;
                }
                return _hasVideo.Value;
            }
        }

        public string Video { get { return HasVideo ? "~/Images/video.png" : ""; } }

        public bool IsDirectDelivery { get; private set; }
        public string DirectDelivery { get { return IsDirectDelivery ? "~/Images/delivery.png" : ""; } }

        public bool IsPrivateLabel { get; private set; }
        public string PrivateLabel { get { return IsPrivateLabel ? "~/Images/privatelabel.png" : ""; } }
    }
}
