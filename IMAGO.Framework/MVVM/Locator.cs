using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IMAGO.Framework.MVVM
{
    public class Locator
    {
        #region Private Fields
        [ImportMany("ViewModel", AllowRecomposition = true)]
        private IEnumerable<Lazy<object, IViewModelMetadata>> _viewModelsLazy;
        #endregion

        #region Protected Properties 
        public object this[string item]
        {
            get
            {
                return this._viewModelsLazy.Single(x => x.Metadata.Name.Equals(item)).Value;
            }
        }
        #endregion

        #region Constructor 
        public Locator()
        {
            string localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(localDirectory))
            {
                var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
                var container = new CompositionContainer(assemblyCatalog);

                // Will load from the Folder where the application runs. For developement all we need it's in the same Assembly
                //var directoryCatalog = new DirectoryCatalog(localDirectory);
                //var container = new CompositionContainer(directoryCatalog);

                container.ComposeParts(this);
            }
    }
        #endregion
    }
}