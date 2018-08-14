using System;
using System.Collections.Generic;
using System.Text;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public class ChalcoArticle : Article
    {
        public bool HasPhotoInChalco { get; private set; }

        public string Color { get; private set; }

        public string WebColor { get; private set; }

        public string Youtube { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public float Depth { get; private set; }

        public float Weight { get; private set; }

        public string Materials { get; private set; }

    }
}