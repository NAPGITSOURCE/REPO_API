using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherAPI.Models;
using WeatherAPI.Common;
using System.IO;
using System.Configuration;

namespace WeatherAPI.Controllers
{
    public class WeatherController : ApiController
    {
        Utility objUtility = new Utility();

        string projectFolderPath = ConfigurationManager.AppSettings["projectFolderPath"].ToString();
        string app_data_textfile_path = ConfigurationManager.AppSettings["app_data_textfile_path"].ToString();
        bool allowMultipleFilesOnSameDay = Convert.ToBoolean(ConfigurationManager.AppSettings["allowMultipleFilesOnSameDay"].ToString());

        // GET: api/Weather
        [Route("api/Weather/GetWeather")]
        public IEnumerable<City> Get()
        {
            // Utility used to create web request
            Utility objUtility = new Utility();

            string projectFolderPath = ConfigurationManager.AppSettings["projectFolderPath"].ToString();
            string app_data_textfile_path = ConfigurationManager.AppSettings["app_data_textfile_path"].ToString();
            bool allowMultipleFilesOnSameDay = Convert.ToBoolean(ConfigurationManager.AppSettings["allowMultipleFilesOnSameDay"].ToString());
            // Provide the Main Folder Name
            string mainFolderName = "WeatherData";

            // Check folder is exist or not if not exist then create the Main Folder
            if (!Directory.Exists(@projectFolderPath + mainFolderName))
                Directory.CreateDirectory(@projectFolderPath + mainFolderName);

            //Object of City Model to get the Json data from CityList.txt file available in App_Data
            City objCityList = new City();
            
            //Read cityCode and cityName from CityList.text file
            var cityDetails = System.IO.File.ReadAllText(app_data_textfile_path);
            //var cityDetails = System.IO.File.ReadAllText("c:\\users\\nitesh\\documents\\visual studio 2013\\Projects\\WeatherService\\WeatherService\\App_Data\\CityList.txt");
            List<City> cityList = JsonConvert.DeserializeObject<List<City>>(cityDetails);

            // Create folder + Get the weather details based on city name for each city
            foreach(City ct in cityList)
            {
                // Set folder Name as city name
                string foldername = ct.cityName;

                // Check the folder is exist or not, if not exist then create Folder based on city name
                if (!Directory.Exists(@projectFolderPath + mainFolderName + "/" + foldername))
                    Directory.CreateDirectory(@projectFolderPath + mainFolderName + "/" + foldername);

                // Check Multiple files for same days are allowed or not
                string subFolderName = string.Empty;
                if (allowMultipleFilesOnSameDay)
                    subFolderName = DateTime.Now.ToString("d") + "_" + DateTime.Now.ToString("hh:mm tt").Replace(":","");
                else
                    subFolderName = DateTime.Now.ToString("d");

                // Set the Output file path
                string filePath = projectFolderPath + mainFolderName + "/" + foldername + "/" + subFolderName;
                //if (!Directory.Exists(@"C:/Users/" + mainFolderName + "/" + foldername))
                //Directory.CreateDirectory(@"C:/Users/" + mainFolderName + "/" + foldername);

                // Get the details of Weather by creating web request
                dynamic objWetherData = objUtility.GetWeatherDetails(ct.cityCode);

                // Create/Override the output file
                fileCheckOrCreate(objWetherData,filePath);
            }
            return cityList;
        }

        // POST: api/Weather
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]
        public void AddCity(City objCity)
        {
            var cityDetails = System.IO.File.ReadAllText(app_data_textfile_path);
            List<City> cityList = JsonConvert.DeserializeObject<List<City>>(cityDetails);
            if (cityList.Any(e => (e.cityName == objCity.cityName)))
            {
                cityList.RemoveAll(x => x.cityName == objCity.cityName);
            }
            cityList.Add(objCity);
            string cityString = JsonConvert.SerializeObject(cityList.ToArray());
            File.WriteAllText(app_data_textfile_path, cityString);
        }

        [HttpDelete]
        public void DeleteCityBycityNameAndcityCode(string cityName, string cityCode)
        {
            var cityDetails = System.IO.File.ReadAllText(app_data_textfile_path);
            List<City> cityList = JsonConvert.DeserializeObject<List<City>>(cityDetails);
            if (cityList.Any(e => (e.cityName == cityName && e.cityCode == cityCode)))
            {
                cityList.RemoveAll(x => x.cityName == cityName && x.cityCode == cityCode);
            }
            string cityString = JsonConvert.SerializeObject(cityList.ToArray());
            File.WriteAllText(app_data_textfile_path, cityString);
        }

        public void fileCheckOrCreate(dynamic weatherData,string path)
        {
            string filePath = path + ".txt";
            //string weatherString = JsonConvert.SerializeObject(weatherData);
             File.WriteAllText(filePath, weatherData);

        }
    }
}
