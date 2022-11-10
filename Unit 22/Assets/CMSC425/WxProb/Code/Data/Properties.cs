using System;

// Use this class as part of your deserialization from JSON data
// into C# objects.

namespace Wx
{
    [Serializable]
    public class Properties
    {
        public string timestamp;
        public string textDescription;
        public QuantitativeValue temperature;
        public QuantitativeValue windDirection;
        public QuantitativeValue windSpeed;
        public QuantitativeValue windGust;
        public QuantitativeValue relativeHumidity;
        public QuantitativeValue windChill;
        public QuantitativeValue heatIndex;
    }
}