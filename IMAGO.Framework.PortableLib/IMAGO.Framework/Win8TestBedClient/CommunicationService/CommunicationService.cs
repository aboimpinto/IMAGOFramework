using IMAGO.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Win8TestBedClient.CommunicationService
{
    public class CommunicationService
    {
        #region Constants 
        private const string _constServiceUrl = "http://localhost:59686";
        #endregion

        #region Private Fields
        private IEnumerable<EndPointDefinition> _endPointDefinions;
        private Dictionary<string, WeakReference> _services;
        #endregion

        #region Contructor 
        public CommunicationService(IEnumerable<EndPointDefinition> endPointDefinions)
        {
            _endPointDefinions = endPointDefinions;
            _services = new Dictionary<string, WeakReference>();
        }
        #endregion

        #region Public Methods 
        public T GetInstance<T>()
        {
            lock (_services)
            {
                Type tType = typeof(T);
                T service;

                if (_services.ContainsKey(tType.FullName)) _services.Remove(tType.FullName);

                EndPointDefinition epd = _endPointDefinions.Where(x => x.Contract == typeof(T).Name).FirstOrDefault();
                
                var binding = new BasicHttpBinding();
                var strUrl = string.Empty;
                if (epd.Service.StartsWith("http")) strUrl = epd.Service;
                else strUrl = string.Format("{0}/Services/{1}", _constServiceUrl, epd.Service);

                try
                {
                    var endPoint = new EndpointAddress(strUrl);
                    var channelFactory = new ChannelFactory<T>(binding, endPoint);
                    T tObject = channelFactory.CreateChannel();

                    _services.Add(tType.FullName, new WeakReference(tObject));

                    service = tObject;
                    return service;
                }
                catch (Exception ex )
                {
                    throw new Exception(string.Format("Error getting a reference of the service {0}", epd.Service), ex);
                }
            }
        }
        #endregion
    }
}
