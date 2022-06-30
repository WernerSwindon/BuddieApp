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
            String Url = "https://swapi.dev/api/people";

            List<CharacterGroup> CharacterGroups = new List<CharacterGroup>();

            while (!String.IsNullOrEmpty(Url)) // Loop through pages.
            {
                String JsonValue = GetJson(Url); // get the Json value.
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(JsonValue);

                foreach (var x in myDeserializedClass.results)
                {
                    Boolean found = false;

                    //Console.WriteLine(x.name);
                    //foreach (var f in x.films)
                    //    Console.WriteLine(f);
                    //Console.WriteLine("");

                    for (int y = 0; y < CharacterGroups.Count(); y++)
                    {
                        var firstNotSecond = CharacterGroups[y].Films.Except(x.films).ToList();
                        var secondNotFirst = x.films.Except(CharacterGroups[y].Films).ToList();

                        // Check if there is already a group with the same films. Add the name to the list.
                        if (!firstNotSecond.Any() && !secondNotFirst.Any())
                        {
                            CharacterGroups[y].Names += "," + x.name;
                            found = true;
                            break;
                        }
                    }

                    // No grouping match found, add name with list of films to the list.
                    if (!found)
                    {
                        CharacterGroup cg = new CharacterGroup();
                        cg.Films = x.films;
                        cg.Names = x.name;
                        CharacterGroups.Add(cg);
                    }
                }

                Url = myDeserializedClass.next;
            }

            // Output the list.
            Console.WriteLine("Buddies");

            foreach (var x in CharacterGroups)
            {
                Console.WriteLine(x.Names);
            }

            Console.WriteLine($"{Environment.NewLine}Completed.");
            Console.ReadLine();
        }

        private static String GetJson(String Url)
        {
            String responseFromServer = String.Empty;

            WebRequest request = WebRequest.Create(Url);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            responseFromServer = reader.ReadToEnd();
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

        public class CharacterGroup
        {
            public List<String> Films { get; set; }

            public String Names { get; set; }
        }
    }
}
