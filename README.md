*** Read Blob README ***

--- If Running Server Locally on a Mac ---

* There is an issue with TLS on Macs (mentioned in code comments) and additional configuration is required
* Navigate to ReadBlobServer Project -> Program.cs -> Main method
	1. Comment out the CreateHostBuilder call
	2. Uncomment the CreateHostBuilderNoTLS call
* When creating the docker container, the original non-Mac configuration must be used 


--- Everything was designed to run on http://localhost:5001 ---

* If desired, there are 3 places in code to change the 5001 port
	1. ReadBlobServer Project -> Properties Folder -> launchSettings.json -> applicationUrl variable
	2. ReadBlobServer Project -> Program.cs -> CreateHostBuilderNoTLS(string[] args) method
	3. FunctionalTest Project -> ClientFunctionalTest.cs -> Address variable


--- To Run Server Locally ---

* From terminal/cmd
	* Navigate into the ReadBlobServer folder and run the following commands
		1. dotnet build
		2. dotnet run
* Run within IDE


--- Docker ---

* In terminal/cmd navigate to ReadBlobServer folder
* Create image (feel free to change the name)
	- docker build -t readblobimg .
* Create and run the container (also feel free to change the name and/or port here)
	- docker run -d -p 5001:80 --name readblobcon readblobimg
	- If changing the port, make sure to change the port for the functional test (see above)


--- Tests ---

* Functional test can be run anytime the server is running locally or on a docker container (just ensure ports match)
* Unit tests can be run at anytime
* Integration tests can be run at anytime