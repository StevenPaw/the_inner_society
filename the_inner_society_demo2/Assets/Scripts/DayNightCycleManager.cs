using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace farmingsim
{
    /// <summary>
    /// Script that handles the day night cycle
    /// </summary>
    public class DayNightCycleManager : MonoBehaviour
    {
        [SerializeField] private string startDate = "2018-12-16";
        private DateTime currentDate;
        [Range(0, 24)] public float timeOfDay = 12; //Has to be public so it can be used in the editor

        [SerializeField] private TMP_Text dateDisplay;
        [SerializeField] private GameObject sun;
        [SerializeField] private RectTransform timeCircle;
        [SerializeField] private AnimationCurve sunIntensity;
        [SerializeField] private AnimationCurve lightColorRed;
        [SerializeField] private AnimationCurve lightColorGreen;
        [SerializeField] private AnimationCurve lightColorBlue;
        private Light2D sunLight;

        [SerializeField] private float secondsPerMinute = 60;
        private float secondsPerHour;
        private float secondsPerDay;
        private static DayNightCycleManager instance;

        [SerializeField] private float timeMultiplier = 1;
        [SerializeField] private bool doesDayNightCycle;
        [SerializeField] private float dayTimeChangeTriggerSpeed;
        
        public static DayNightCycleManager Instance => instance;
        
        private void Awake()
        {
            if (Instance == null || Instance == this)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        
            DontDestroyOnLoad(gameObject);
        }

        public bool DoesDayNightCycle
        {
            get => doesDayNightCycle;
            set => doesDayNightCycle = value;
        }

        #region Unity Methods
        
        private void Start()
        {
            currentDate = DateTime.Parse(startDate);
            //Calculate time variables
            secondsPerHour = secondsPerMinute * 60;
            secondsPerDay = secondsPerHour * 24;
            sunLight = sun.GetComponent<Light2D>();
            UpdateDate();
        }

        /// <summary>
        /// Allow changing the timeOfDay in the editor
        /// </summary>
        private void OnValidate()
        {
            sunLight = sun.GetComponent<Light2D>();
            ChangeLighting();
        }

        private void Update()
        {
            ChangeLighting(); //Change the lighting intensity
            
            //Reset the time of day if midnight is hit
            if (timeOfDay >= 24)
            {
                timeOfDay = 0;
                currentDate = currentDate.AddDays(1);
                UpdateDate();
            }
            
            if (doesDayNightCycle)
            {
                //Calculate the time of day with the seconds per Day and the Time Multiplier to make time run faster/slower then real time
                timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;
            }
        }

        #endregion

        /// <summary>
        /// Changes the lighting according to the daytime
        /// </summary>
        private void ChangeLighting()
        {
            //Change the lighting intensity based on the time of day
            sunLight.intensity = sunIntensity.Evaluate(timeOfDay);
            
            Color newColor = new Color(lightColorRed.Evaluate(timeOfDay), lightColorGreen.Evaluate(timeOfDay),
                lightColorBlue.Evaluate(timeOfDay));
            sunLight.color = newColor;

            timeCircle.rotation = Quaternion.Euler(0,0, 365 / 24 * (timeOfDay + 12));
        }

        /// <summary>
        /// Change the time of day to a specific time
        /// </summary>
        /// <param name="dayTimeIn">(float) time to switch to</param>
        /// <param name="changeSpeed">(float) how fast the change should be</param>
        public void ChangeDayTime(float dayTimeIn, float changeSpeed)
        {
            if (dayTimeIn >= timeOfDay)
            {
                DOTween.To(() => timeOfDay, x => timeOfDay = x, dayTimeIn, changeSpeed);
            }
            else
            {
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(DOTween.To(() => timeOfDay, x => timeOfDay = x, 23.9f, changeSpeed / 2));
                mySequence.Append(DOTween.To(() => timeOfDay, x => timeOfDay = x, 0f, 0.01f));
                mySequence.Append(DOTween.To(() => timeOfDay, x => timeOfDay = x, dayTimeIn, changeSpeed / 2));
            }
        }

        public void UpdateDate()
        {
            dateDisplay.text = currentDate.ToString("dd.MM.yyyy");
        }
    }
}