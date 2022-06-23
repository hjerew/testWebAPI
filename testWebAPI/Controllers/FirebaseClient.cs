using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using testWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace testWebAPI.Controllers

{
    public class FirebaseClient
    {
        private readonly HttpClient _client;
        private string firebaseURI = "https://fir-functions-api-430be-default-rtdb.firebaseio.com";
        private string extURI = ".json";
        //private string firebaseLoc = "/CountryItems";
        public FirebaseClient(HttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<List<CountryItem>> GetItemsAsync()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var streamTask = _client.GetStreamAsync(firebaseURI + "/" + extURI);
            var countryItems = await JsonSerializer.DeserializeAsync<List<CountryItem>>(await streamTask);


            //Stream stream = streamTask.Result;
            //await foreach (var cItem in JsonSerializer.DeserializeAsyncEnumerable<CountryItem>(stream))
            //{
            //    countryItems.Add(cItem);
            //}

            return countryItems;
        }
        [HttpGet("{id}")]
#pragma warning disable IDE0051 // Remove unused private members
        public async Task<CountryItem> GetItemAsync(long Id)
#pragma warning restore IDE0051 // Remove unused private members
        {
            _client.DefaultRequestHeaders.Accept.Clear();

            var streamTask = _client.GetStreamAsync(firebaseURI + "/" + extURI);
            var countryItems = await JsonSerializer.DeserializeAsync<List<CountryItem>>(await streamTask);

            foreach (var countryItem in countryItems)
            {
                if (countryItem.Id.Equals(Id))
                {
                    return countryItem;
                }
            }
            return null;
        }

        [HttpPut("{id}")]
        public async Task<String> SaveChangesAsync(long Id, CountryItem countryItem)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var MsgContent = new StringContent(
                JsonSerializer.Serialize(countryItem),
                UnicodeEncoding.UTF8,
                "application/json");
            HttpResponseMessage response = await _client.PutAsync(firebaseURI + "/" + Id + "/" + extURI, MsgContent);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        [HttpPost]
        public async Task<String> PostAsync(CountryItem countryItem)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var MsgContent = new StringContent(
                JsonSerializer.Serialize(countryItem),
                UnicodeEncoding.UTF8,
                "application/json");
            HttpResponseMessage response = await _client.PutAsync(firebaseURI + "/" + countryItem.Id + "/" + extURI, MsgContent);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }



        public async Task<long> GetNewIdAsync()
        {
            var streamTask = _client.GetStreamAsync(firebaseURI + "/" + extURI);
            var countryItems = await JsonSerializer.DeserializeAsync<List<CountryItem>>(await streamTask);
            Console.WriteLine(countryItems.Last().Id);
            return countryItems.Last().Id + 1;
        }







        //async Task Main(string[] args)
        //{
        //    var repositories = await ProcessRepositories(); //ProcessRepositories is of type Task<> (generic) and returns type List<Repository>



        //    {
        //        Console.WriteLine(repositories.Name); //outputs the Name object in the "repo" item in the list of Repository objects called "repositories"
        //        Console.WriteLine(repositories.Description);
        //        //Console.WriteLine(repo.GitHubHomeUrl);
        //        //Console.WriteLine(repo.Homepage);
        //        //Console.WriteLine(repo.Watchers);
        //        //Console.WriteLine(repo.LastPush);
        //        Console.WriteLine();
        //    }



        //    //foreach (var repo in repositories)
        //    //{
        //    //    Console.WriteLine(repo.Name); //outputs the Name object in the "repo" item in the list of Repository objects called "repositories"
        //    //    //Console.WriteLine(repo.Description);
        //    //    //Console.WriteLine(repo.GitHubHomeUrl);
        //    //    //Console.WriteLine(repo.Homepage);
        //    //    //Console.WriteLine(repo.Watchers);
        //    //    //Console.WriteLine(repo.LastPush);
        //    //    Console.WriteLine();
        //    //}
        //}
        //private static async Task<List<Repository>> ProcessRepositories()
        //private async Task<Repository> ProcessRepositories()

        //{
        //    //accept header to accept JSON requests
        //    _client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(
        //    //new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        //    //user agent header checked by github server code to retrieve info from them
        //    //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        //    //makes a web req through HttpClient Client using a stream isntead of a string as the source

        //    //var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
        //    var streamTask = _client.GetStreamAsync("https://fir-functions-api-430be-default-rtdb.firebaseio.com/.json");

        //    //deserialize the JSON response and update the data fields in the Repository
        //    //deserializeAsync is a generic, type argument supplied specifies what kind of objects should be created form the JSON text
        //    //List<Repository> (also generic) stores a collection of type Repository, i.e. a collection of repository objects

        //    //var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
        //    var repositories = await JsonSerializer.DeserializeAsync<Repository>(await streamTask);

        //    return repositories;
        //}
    }
}

