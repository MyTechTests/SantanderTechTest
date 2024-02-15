#Best of Hacker News API
A RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API.

##Prerequisites
Docker
.Net Core 8

##How to run
If you are using visual studio then set BestOfHackerNews as the startup project.  You may need to set the following option in chrome for the swagger page to load: WebTransport Developer Mode
Run the program, no argumants are required.  The swagger page should be loaded.
If using the exe directly: run BestOfHackerNews.exe

##Calling the api
Create an API key by generating a GUID and adding it to the environment variables as follows:

    ApiKeys:XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX=Some client name

This can alternatively be set by adding it to the appSettings.json file. e.g.:
  "ApiKeys": {
    "CA7329D1-0395-49C2-913D-50251419E8D5": "A clients name"
  }

Call the api as follows:
Using a client e.g. postman, set the header of a request with the value of "X-Api-Key" being the guid created above.


##Assumptions
The rest api is accessible i.e. no restrictions have been applied that would require firewall changes or urlacl.
The reference to Microsoft.AspNetCore in the BestOfHackerNews.Core library is deprecated but used by other micrtosoft packages.  Assumption here is that the packages using it will be updated in the future
The package Microsoft.AspNetCore.RateLimiting is in RC, but has not changed in two years.  Assuming that this is now stable.

##Further enhancements

## Contributing
Pull requests are welcome. Please open an issue first to discuss what you would like to change.  Please also ensure that all affected tests are updated/added where requried.

##Licence
Chosen based on guidance from here: https://choosealicense.com/licenses/
[GNU AFFERO GENERAL PUBLIC LICENSE](https://www.gnu.org/licenses/agpl-3.0.en.html)