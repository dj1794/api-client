using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Apiclient
{
    class UserLoginMode
    {
        public string userName;
        public string passWord;
    }
    class Token
    {
        [JsonProperty]
        public static string token { get; set; }
    }
    class root
    {
        public static List<Token> rootList { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string accessToken = string.Empty;
            //Username to get authentication Key.
            UserLoginMode userLoginMode = new UserLoginMode();
            userLoginMode.userName = "djoshi";
            userLoginMode.passWord = "djoshi";
            Console.WriteLine("Enter POST or GET");
            string commandMethod = Console.ReadLine();
            if(commandMethod == "GET")
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response;
                client.BaseAddress = new Uri("http://localhost:50357/");
                // Add an Accept header for JSON format.    
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Post request with userName to get access key.
                response = client.PostAsJsonAsync("api/taskPortal", userLoginMode).Result;
                // Response from the post method[Get key]
                var token = response.Content.ReadAsStringAsync().Result;
                if(response.IsSuccessStatusCode)
                {
                    //Convert json into model to read token.
                    JsonConvert.DeserializeObject<Token>(token);
                    // Add header with token to get access.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token.token);                   
                    // Simple get request with secured mode.
                    response = client.GetAsync("api/taskPortal").Result;  // Blocking call!    
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(data);
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }                
                
            }
            else if(commandMethod == "POST")
            {
                // To be continued....
            }
            else
            {
                Console.WriteLine("Wrong Method");
            }
            Console.ReadLine();

        }
    }
}
