using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Content loader is a singleton that can be used to load 
    /// content using the content pipeline object.
    /// </summary>
    public class ContentLoader
    {
        private static ContentLoader instance;

        public ContentManager Content { get; private set; }

        public static ContentLoader Instance
        {
            get
            {
                if (instance == null)
                    instance = new ContentLoader();

                return instance;
            }
        }
        /// <summary>
        /// Initialize Content pipeline.
        /// </summary>
        /// <param name="content"></param>
        public void Initialize(ContentManager content)
        {
            Content = content;
        }
    }
}
