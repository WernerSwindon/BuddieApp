using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuddieApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            String Url = "https://swapi.dev/api/people/?page=1";

            //List<(String Name, Int32 HashValue)> Characters = new List<(string Name, int HashValue)>();

            List<(List<String> Films, String Name)> Characters = new List<(List<string> Films, string Name)>();

            while (!String.IsNullOrEmpty(Url))
            {
                String JsonValue = GetJson(Url);
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(JsonValue);

                foreach (var x in myDeserializedClass.results)
                {
                    if (Characters.Any(y => y.Films == x.films))
                    {
                        //Characters.Where(y => y.Films == x.films).
                    }

                    //Characters.Add((x.name, x.films.GetHashCode()));
                }

                Url = myDeserializedClass.next;
            }

            //var newList = Characters.OrderBy(z => z.HashValue);

            //Int32 PreviousHashValue = newList.First().HashValue;
            //String BuddieResponse = String.Empty;

            //foreach (var x in newList)
            //{
            //    if (PreviousHashValue != x.HashValue)
            //    {
            //        Console.WriteLine(BuddieResponse);
            //        PreviousHashValue = x.HashValue;
            //        BuddieResponse = String.Empty;
            //    }
            //    else
            //    {
            //        BuddieResponse += x.Name + ",";
            //    }
            //}

            Console.WriteLine("Completed.");
            Console.ReadLine();
        }

        private static String GetJson(String Url)
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(Url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            //Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        public class Result
        {
            public string name { get; set; }
            public string height { get; set; }
            public string mass { get; set; }
            public string hair_color { get; set; }
            public string skin_color { get; set; }
            public string eye_color { get; set; }
            public string birth_year { get; set; }
            public string gender { get; set; }
            public string homeworld { get; set; }
            public List<string> films { get; set; }
            public List<string> species { get; set; }
            public List<string> vehicles { get; set; }
            public List<string> starships { get; set; }
            public DateTime created { get; set; }
            public DateTime edited { get; set; }
            public string url { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public string next { get; set; }
            public object previous { get; set; }
            public List<Result> results { get; set; }
        }

    }
}
