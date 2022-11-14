using UnityEngine;

namespace Wx
{
    public class DialWindObs : MonoBehaviour
    {
        public Wind wind;

        Vector3 axle;

        void Start()
        {
            // Add code to obtain reports of state changes.
            wind.ReportWind += ReportWind;
        }

        void ReportWind(float direction, float speed)
        {
            // Add code to manage reports of state changes.
            Debug.Log("Observer received ReportWind call with direction = " + direction + " and speed = " + speed + "\n");
        }
    }
}