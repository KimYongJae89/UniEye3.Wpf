using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.RefractionMeasure.Resources
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        private Uri _sourceUri = null;

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get => _sourceUri;
            set
            {
                base.Source = value;

                if (!MergedDictionaries.Any(x => x.Source.ToString() == value.ToString()))
                {
                    var resourceDictionary = new ResourceDictionary() { Source = value };
                    MergedDictionaries.Add(resourceDictionary);
                }
            }
        }
    }
}
