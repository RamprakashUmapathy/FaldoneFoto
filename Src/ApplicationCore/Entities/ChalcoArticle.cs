using System;
using System.Collections.Generic;
using System.Text;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public class ChalcoArticle : Article
    {
        public string Color { get; private set; }

        public string WebColor { get; private set; }

        public string YoutubeVideo { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public double Depth { get; private set; }

        public double Weight { get; private set; }

        public string Materials { get; private set; }

    }
}