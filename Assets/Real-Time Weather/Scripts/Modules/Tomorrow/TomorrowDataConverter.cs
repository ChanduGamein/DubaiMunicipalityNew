﻿//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.AlertSystem;
using RealTimeWeather.Classes;
using RealTimeWeather.Tomorrow.Classes;
using RealTimeWeather.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace RealTimeWeather.Tomorrow
{
    /// <summary>
    /// This class is used as a converter of Tomorrow data to Real-Time Weather data
    /// </summary>
    public class TomorrowDataConverter
    {
        #region Private Constants
        private const string kTomorrowDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
        private const string kUnableToParseStr = "Unable to parse ";
        private const string kExceptionStr = "\nException:";
        private const string kCurrentStr = "current";
        private const string kHourlyStr = "1h";
        private const string kDailyStr = "1d";
        #endregion

        #region Constructors
        public TomorrowDataConverter(TomorrowData tomorrowData)
        {
            _tomorrowData = tomorrowData;
        }
        #endregion

        #region Private Variables
        [SerializeField]
        private TomorrowData _tomorrowData;
        #endregion

        #region Public Methods
        /// <summary>
        /// Converts Tomorrow data to RTW weather data.
        /// </summary>
        /// <returns>
        /// A WeatherData class instance.
        /// </returns>
        public WeatherData ConvertCurrentTomorrowDataToRtwData()
        {
            if (_tomorrowData == null || _tomorrowData.Data.TimelinesList.Count == 0)
            {
                return null;
            }

            var rtwCurrentWeatherData = new WeatherData();

            foreach (var timeData in _tomorrowData.Data.TimelinesList.Where(timeData => kCurrentStr == timeData.Timestep))
            {
                ConvertCurrentTomorrowWeatherDataToRtwData(timeData, ref rtwCurrentWeatherData);
            }

            return rtwCurrentWeatherData;
        }

        /// <summary>
        /// Converts Tomorrow hourly data to RTW weather data list.
        /// </summary>
        /// <returns>
        /// A WeatherData class instance list.
        /// </returns>
        public List<WeatherData> ConvertHourlyTomorrowDataToRtwData()
        {
            var rtwHourlyWeatherDataList = new List<WeatherData>();
            if (_tomorrowData == null || _tomorrowData.Data.TimelinesList.Count < 2) //TimelinesList[0] always is current time.
            {
                return rtwHourlyWeatherDataList;
            }

            foreach (var timeData in _tomorrowData.Data.TimelinesList.Where(timeData => kHourlyStr == timeData.Timestep))
            {
                ConvertTomorrowWeatherDataIntervalsToRtwDataList(timeData, ref rtwHourlyWeatherDataList);
            }

            return rtwHourlyWeatherDataList;
        }

        /// <summary>
        /// Converts Tomorrow daily data to RTW weather data list.
        /// </summary>
        /// <returns>
        /// A WeatherData class instance list.
        /// </returns>
        public List<WeatherData> ConvertDailyTomorrowDataToRtwData()
        {
            var rtwDailyWeatherDataList = new List<WeatherData>();
            if (_tomorrowData == null || _tomorrowData.Data.TimelinesList.Count < 2) //First element(TimelinesList[0]) always is current time.
            {
                return rtwDailyWeatherDataList;
            }

            foreach (var timeData in _tomorrowData.Data.TimelinesList.Where(timeData => kDailyStr == timeData.Timestep))
            {
                ConvertTomorrowWeatherDataIntervalsToRtwDataList(timeData, ref rtwDailyWeatherDataList);
            }

            return rtwDailyWeatherDataList;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Converts the specified string representation of a date and time to its DateTime equivalent.
        /// </summary>
        /// <param name="tomorrowDateTime">A string that contains a date and time to convert.</param>
        /// <returns>
        /// A DateTime instance.
        /// </returns>
        private DateTime ConvertTomorrowTimeInDateTime(string tomorrowDateTime)
        {
            try
            {
                return DateTime.ParseExact(tomorrowDateTime, kTomorrowDateTimeFormat, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                Debug.Log(kUnableToParseStr + tomorrowDateTime + kExceptionStr + exception);
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Converts Tomorrow weather current data to RTW weather data.
        /// </summary>
        /// <param name="currentTimeData">Current TimeData</param>
        /// <param name="weatherDataOut">A WeatherData class reference out</param>
        private void ConvertCurrentTomorrowWeatherDataToRtwData(TimeData currentTimeData, ref WeatherData weatherDataOut)
        {
            weatherDataOut.Dewpoint = currentTimeData.Intervals[0].WeatherData.DewPoint;
            weatherDataOut.Humidity = currentTimeData.Intervals[0].WeatherData.Humidity;
            weatherDataOut.Precipitation = currentTimeData.Intervals[0].WeatherData.PrecipitationIntensity;
            weatherDataOut.Pressure = currentTimeData.Intervals[0].WeatherData.PressureSurfaceLevel;
            weatherDataOut.Temperature = currentTimeData.Intervals[0].WeatherData.Temperature;
            weatherDataOut.Visibility = currentTimeData.Intervals[0].WeatherData.Visibility;
            weatherDataOut.Wind.Speed = currentTimeData.Intervals[0].WeatherData.WindSpeed;
            weatherDataOut.Wind.Direction = Utilities.DegreeToVector2(currentTimeData.Intervals[0].WeatherData.WindDirection);
            weatherDataOut.WeatherState = ConvertWeatherCodeIntoWeatherState(currentTimeData.Intervals[0].WeatherData.WeatherCode);
            weatherDataOut.DateTime = ConvertTomorrowTimeInDateTime(currentTimeData.Intervals[0].StartTime);

            ConvertTomorrowLocalizationDataToRtwData(ref weatherDataOut);
        }

        /// <summary>
        /// Converts Tomorrow weather data intervals to RTW weather data list.
        /// </summary>
        /// <param name="timeData">Hourly TimeData</param>
        /// <param name="weatherDataOut">A WeatherData class reference out</param>
        private void ConvertTomorrowWeatherDataIntervalsToRtwDataList(TimeData timeData, ref List<WeatherData> weatherDataOut)
        {
            foreach (IntervalData interval in timeData.Intervals)
            {
                var weatherData = new WeatherData
                {
                    Dewpoint = interval.WeatherData.DewPoint,
                    Humidity = interval.WeatherData.Humidity,
                    Precipitation = interval.WeatherData.PrecipitationIntensity,
                    Pressure = interval.WeatherData.PressureSurfaceLevel,
                    Temperature = interval.WeatherData.Temperature,
                    Visibility = interval.WeatherData.Visibility,
                    Wind =
                    {
                        Speed = interval.WeatherData.WindSpeed,
                        Direction = Utilities.DegreeToVector2(interval.WeatherData.WindDirection)
                    },
                    WeatherState = ConvertWeatherCodeIntoWeatherState(interval.WeatherData.WeatherCode),
                    DateTime = ConvertTomorrowTimeInDateTime(interval.StartTime)
                };

                ConvertTomorrowLocalizationDataToRtwData(ref weatherData);

                weatherDataOut.Add(weatherData);
            }
        }

        /// <summary>
        /// Converts WeatherCode value to WeatherState value.
        /// </summary>
        /// <param name="weatherCode">A WeatherCode enum value.</param>
        /// <returns>
        /// A WeatherState enum value.
        /// </returns>
        private WeatherState ConvertWeatherCodeIntoWeatherState(WeatherCode weatherCode)
        {
            switch (weatherCode)
            {
                case WeatherCode.Unknown:
                case WeatherCode.Clear:
                    return WeatherState.Clear;
                case WeatherCode.Cloudy:
                    return WeatherState.Cloudy;
                case WeatherCode.MostlyClear:
                    return WeatherState.PartlyClear;
                case WeatherCode.PartlyCloudy:
                case WeatherCode.MostlyCloudy:
                    return WeatherState.PartlyCloudy;
                case WeatherCode.Fog:
                case WeatherCode.LightFog:
                    return WeatherState.Mist;
                case WeatherCode.LightWind:
                case WeatherCode.Wind:
                case WeatherCode.StrongWind:
                    return WeatherState.Windy;
                case WeatherCode.Drizzle:
                case WeatherCode.Rain:
                case WeatherCode.LightRain:
                case WeatherCode.HeavyRain:
                    return WeatherState.RainPrecipitation;
                case WeatherCode.Flurries:
                    return WeatherState.Fair;
                case WeatherCode.Snow:
                case WeatherCode.LightSnow:
                case WeatherCode.HeavySnow:
                case WeatherCode.IcePellets:
                case WeatherCode.HeavyIcePellets:
                case WeatherCode.LightIcePellets:
                    return WeatherState.SnowPrecipitation;
                case WeatherCode.FreezingDrizzle:
                case WeatherCode.FreezingRain:
                case WeatherCode.LightFreezingRain:
                case WeatherCode.HeavyFreezingRain:
                    return WeatherState.RainSnowPrecipitation;
                case WeatherCode.Thunderstorm:
                    return WeatherState.Thunderstorms;
                default:
                    return WeatherState.Clear;
            }
        }

        /// <summary>
        /// Converts Tomorrow weather localization and time to RTW weather data.
        /// </summary>
        /// <param name="weatherDataOut">A WeatherData class reference out</param>
        private void ConvertTomorrowLocalizationDataToRtwData(ref WeatherData weatherDataOut)
        {
            weatherDataOut.Localization.Latitude = _tomorrowData.Latitude;
            weatherDataOut.Localization.Longitude = _tomorrowData.Longitude;
            weatherDataOut.Localization.City = _tomorrowData.City;
            weatherDataOut.Localization.Country = _tomorrowData.Country;
            weatherDataOut.TimeZone = Utilities.GetTimeZone(_tomorrowData.Latitude, _tomorrowData.Longitude);

#if UNITY_STANDALONE || UNITY_IOS
            weatherDataOut.UTCOffset = Utilities.GetUTCOffsetData(weatherDataOut.TimeZone);
#endif

#if UNITY_ANDROID
            rtwWeatherData.UTCOffset = Utilities.GetUTCOffsetOnAndroid(rtwWeatherData.TimeZone);
#endif

            weatherDataOut.DateTime = weatherDataOut.DateTime.Add(weatherDataOut.UTCOffset);
        }
        #endregion
    }
}