/*
Author : Nitesh Patle
CreationDate : 26/06/2019
AppName : WeatherService (WeatherAPI)
UpdatedBy : Nitesh Patle
UpdatedOn : 27/06/2019
*/
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherAPI.Common
{
    public class Utility
    {
        string apiUrl = ConfigurationManager.AppSettings["apiUrl"].ToString();
        string appId = ConfigurationManager.AppSettings["appId"].ToString();
        string serviceContext = ConfigurationManager.AppSettings["serviceContext"].ToString();
        public dynamic GetWeatherDetails(string cityCode)
        {
            try
            {
                string apiParam = String.Format("id={0}&APPID={1}", cityCode,appId);
                string webUrl = String.Concat(apiUrl, serviceContext, apiParam);
                var webRequest = WebRequest.Create(webUrl);
                var httpWebRequest = (HttpWebRequest)webRequest;
                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Headers.Add("APPID", appId);
                httpWebRequest.Accept = "application/json";
                var webResponse = webRequest.GetResponse();
                var responseStream = webResponse.GetResponseStream();
                var streamReader = new StreamReader(responseStream, Encoding.Default);
                var json = streamReader.ReadToEnd();
                responseStream.Close();
                webResponse.Close();
                return json;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
