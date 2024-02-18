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
Create an API key by generating a value and adding it to the environment variables as follows:

    BOHN_ApiKeys:test=this is a test key

This can alternatively be set by adding it to the appSettings.json file. e.g.:
  "ApiKeys": {
    "test": "this is a test key"
  }

Call the api as follows:
The application is built with swagger UI support.  In the swagger page click "Authorise" and use the key defined e.g. "test".
The api can be called via http as long as the header of a request is populated with the key "X-Api-Key" and the value being the key e.g. "test".

##Overload protection
The api will allow any number of clients to request the top n stories, but it will protect against the same client attempting a DoS attack.  Each client as identified by their host header is limited to 5 requests every 10 seconds.

##Assumptions
The rest api is accessible i.e. no restrictions have been applied that would require firewall changes or urlacl.
The package Microsoft.AspNetCore.RateLimiting is in RC, but has not changed in two years.  Assuming that this is now stable.
If a news item is marked as "dead", then it is not deleted, just inactive and should be returned
Once a story or comment has been retrieved, it may have changed the next time we need it (e.g. score value) so cannot be cached for long
Where the website states "Want to know the total number of comments on an article? Traverse the tree and count", it also states that "descendants	In the case of stories or polls, the total comment count.".  Assuming the count for stories does not require traversal of the tree.

##Known Issues
The reference to Microsoft.AspNetCore in the BestOfHackerNews.Core library is deprecated but used by other microsoft packages.  Assumption here is that the packages using it will be updated in the future

##Further enhancements

##Restrictions
This service will support a large number of requests from multiple sources as identified by the host header in each request.  It will limit the number of requests for an individual caller to prevent DoS attack.
An api key must be supplied in the x-api-key header value of each request as defined in "Calling the api" above.

## Contributing
Pull requests are welcome. Please open an issue first to discuss what you would like to change.  Please also ensure that all affected tests are updated/added where required.

##Licence
Chosen based on guidance from here: https://choosealicense.com/licenses/
[GNU AFFERO GENERAL PUBLIC LICENSE](https://www.gnu.org/licenses/agpl-3.0.en.html)