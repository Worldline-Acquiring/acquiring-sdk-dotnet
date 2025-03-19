/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ServiceLocationData
    {
        /// <summary>
        /// Address where the cardholder received the service
        /// </summary>
        public ServiceLocationAddress Address { get; set; }

        /// <summary>
        /// Geographical coordinates where the cardholder received the service.
        /// Geographical coordinates in decimal degree (DD) format Latitude,Longitude where Latitude and Longitude
        /// are floating point numbers with the unit degree. Integer and decimal digits are separated by a dot. East
        /// and north are indicated by positive numbers whereas west and south have negative ones.
        /// </summary>
        public GeoCoordinates GeoCoordinates { get; set; }
    }
}
