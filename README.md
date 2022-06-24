This is a ASP.net Core MVC web API adapted from this in-memory storage example (https://github.com/tutorpraveen/BasicWebAPI/tree/master/CountryAPI).
This API uses dependency injection of HttpClient methods in order to store .JSON strings to a firebase realtime database.

This API can be built and tested using Visual studio and Postman. JSON body post commands can be sent to https://localhost:7219/api/CountryItems in the following format:

{
  "name": "USA",
  "isDone":true
}
