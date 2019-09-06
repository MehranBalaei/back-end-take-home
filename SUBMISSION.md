### Technologies used
- The main project (AirRouteFinder) is a standard ASP.Net Web API.
	These are the main files of the project:
	~\AirRouteFinderSolution\AirRouteFinder\Controllers\RouteController.cs  (Controller API)
	~\AirRouteFinderSolution\AirRouteFinder\Controllers\Logic.cs  (The logic that runs the algorithm)
	~\AirRouteFinderSolution\AirRouteFinder\Models\ApplicationDb.cs   (Contains method to connect to database and run queries)
- It contains a unit test project (AirRouteFinder.Tests) which uses MS Test framework.
	The tests are located in this file:
	~\AirRouteFinderSolution\AirRouteFinder.Tests\Controllers\RouteControllerTest.cs
- I use OleDbConnection object to connect to the csv database.


### Running the application
The request should be sent in this format:
	http://{host}/Get/{Origin}/{Destination}
For example: 
	http://localhost:57437/Get/YYZ/JFK

I have also hosted this service in an Azure App service. You can use it here:
	http://mehran.azurewebsites.net/Get/YYZ/JFK
This is pretty slow, since I have a very basic Azure account.

### Data files
The main project (AirRouteFinder) uses the full database and the unit test project (AirRouteFinder.tests) uses the test data.
The connection string for the main project is set in this file:
	~\AirRouteFinderSolution\AirRouteFinder\Web.config
The connction string for the unit test project is set in this file:
	~\AirRouteFinderSolution\AirRouteFinder.Tests\App.config

On my computer, the data files are located in this folder:
	C:\Coding\Guestlogix\AirRouteFinderSolution\AirRouteFinder\App_Data

In order to be able to build and run the solution, you will need to set the connection strings in the above .config files 
to point to the full and test folders on your computer respectively.


###Logic

I first wrote a recursive method, which calculated the children of each node (airport) before moving to the next node.
Then I realised that method does not return the shortest path and we need a BFS traverse. i.e. we need to complete each level before moving to children.
I am using a queue to implement the BFS traversal.


###Thank you!

Thanks for giving me this opportunity and thanks for reviewing my application.
Hope to see you soon.

