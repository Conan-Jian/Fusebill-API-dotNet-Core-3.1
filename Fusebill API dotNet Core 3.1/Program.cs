using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fusebill_API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //args[0] is the customer id
            //args[1] is the apiKey
            string apiKey = "MDpISmp5QVRTdzZPMlhXSHFteFY1VU0xalNkM1RjS3BsOWxZVjAySnlnZUxqbm1BQ0R2SFQ3cnpUYXFTa05ZNUp4";
            int customerId = 10121826;
            List<string> info = await GetSubscriptionItemId(customerId,apiKey);
            Console.WriteLine($"Customer id: {info[0]}\n{info[1]}, {info[2]}" +
                $"\nTransaction Fee's Subscription Item Id: {info[3]} ");
            
            //must use output generated in solution to se Subscription Product Get. Take that body and put it in a PUT request with the 11/12 quantity field changed

        }
        public static async Task<List<string>> GetSubscriptionItemId(int customerId, string apiKey)
        {
            try 
            {

                //Getting the json from Fusebill Api
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"https://secure.fusebill.com/v1/customers/{customerId}/subscriptions");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + apiKey);
                string result = await client.GetStringAsync("");
                client.Dispose();

                //Converting the Fusebill API into an object and returning the correct subscription Id
                var payload = JsonConvert.DeserializeObject<dynamic>(result);
                List<string> info = new List<string>();
                info.Add(payload[payload.Count-1].customerId.ToString());
                info.Add(payload[payload.Count - 1].planName.ToString());
                info.Add(payload[payload.Count - 1].status.ToString());
                info.Add(payload[payload.Count - 1].subscriptionProducts[1].id.ToString());
                return info;
            }
            catch (Exception e)
            {
                List<string> error = new List<string>() { e.Message, "issue", "issue2", "issue3" };
                return error;
            }
        }
    }
}

