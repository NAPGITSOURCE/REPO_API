# REPO_API
API Repository
**Version 1.0.0**

Document for Weather Service

---
## [**Things to do**] 
### Configuration and setting for Weather Service
1) Set following values in web.config file

   * Set your "projectFolderPath" in web.config
   
   * Set Location of CityList file based on project location to "app_data_textfile_path".
      
      **Note** : CityList.txt file in "app_data" folder contains the list of Cities and is used as input to take the weather condition
             from third party api.
             
   * Set the "allowMultipleFilesOnSameDay" key value "true" or "false".
 
      **Note** : If we set "true" then it will create multiple files based on the date and time for one city, if we set "false" then
             the details will be modified in existing file (override the existing file).
             
   * Set the string value for "mainFolderName" in controller.
---

## [**Folder Structure**] 
### Structure of Folder and file for Weather Service

* Folder creation based on city name
* File creation based on date or date and time

## [**Operations can perform**]

* Get the weather details for the cities in provided folder path (cities are available in CityList.text file).
* Add/Delete the City in/from citylist which are available in CityList.text file.

## [**Things to route the request**]

* {url}/api/Weather/GetWeather
  
  **Note :** Response will be the list of cities in json format in browser and folder/file will be created in provided path.
  

## License & Copyright

@ Nitesh Patle
