using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Wx
{
    // This will be your most complex class. It must maintain events that
    // inform observers of new wind data and when it changes state from
    // simulated data to network-sourced data.

    public class Wind : MonoBehaviour
    {
        public string airportID;

        // ReportWind should be called with the values (wind direction, wind speed)
        public event Action<float, float> ReportWind;

        // ReportState should be called with the new value of isSimulated
        public event Action<bool> ReportState;

        // Use these values to help in making controlled random
        // changes when simulating wind reports.

        const int windDirectionMax = 360;
        const float windSpeedMax = 30;
        const float windSpeedMin = 5;
        const float deltaWindSpeed = 3;
        const float deltaWindDirection = 10;

        System.Random r = new System.Random();

        float windDirection = 0;
        float windSpeed = 0;

        bool isSimulated = true;

        public void Start()
        {
            StartCoroutine(GetNetworkWind());
        }

        public void ChangeState()
        {
            // Add code here to switch states and inform observers

            if (isSimulated)
            {
                StopCoroutine(SimulateWind());
                ReportState(false);
                StartCoroutine(GetNetworkWind());
            } else
            {
                StopCoroutine(GetNetworkWind());
                ReportState(true);
                StartCoroutine(SimulateWind());
            }
            isSimulated = !isSimulated;
        }

        IEnumerator SimulateWind()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (true)
            {
                // Add code here that reports simulated wind direction
                // and speeds every half-second to the proper observers.
                // Be sure to use some randomness to change the direction
                // and speed.

                yield return wait;
            }
        }

        IEnumerator GetNetworkWind()
        {
            WaitForSeconds wait = new WaitForSeconds(10f);

            while (true)
            {
                // Add code here to obtain live data from an airport
                // JSON weather feed, and report it to the proper
                // observers. Query the airport data every ten seconds.
                // DO NOT query the airport data more often than five
                // times in a single second, or the server will block
                // you. Also, do not query the airport data more than
                // 10,000 times in a single day (which would be about
                // once every eight to nine seconds).

                // Query for network data with the UnityWebRequest class.
                // To query for JSON data from airport IAD, you would use
                // the Get method and this string as its argument:
                //
                // "https://api.weather.gov/stations/KIAD/observations/latest"
                //
                // The Get method returns a UnityWebRequest object. Use its
                // SendWebRequest method to send the actual query. It returns
                // a UnityWebRequestAsyncOperation object which you can use
                // in a yield return to pause a coroutine until the request
                // has been answered. Note that you will yield TWICE for each
                // request: once to pause until the request has been answered,
                // and a second time to pause for ten seconds before sending
                // another request.

                UnityWebRequest webRequest = UnityWebRequest.Get("https://api.weather.gov/stations/" + airportID + "/observations/latest");
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Successful weather request for " + airportID + ": " + webRequest.downloadHandler.text);
                        break;
                    default:
                        Debug.Log("Error on weather request for " + airportID + ": " + webRequest.error);
                        break;
                }

                Wx windData = JsonUtility.FromJson<Wx>(webRequest.downloadHandler.text);
                webRequest.Dispose();
                ReportWind(windData.properties.windDirection.value, windData.properties.windSpeed.value);

                yield return wait;
            }
        }
    }
}