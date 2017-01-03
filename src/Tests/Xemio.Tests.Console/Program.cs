using System;
using System.Threading;
using Newtonsoft.Json.Linq;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Press enter to start test");
            Console.ReadLine();

            string bearerToken =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2hhZWZlbGUuZXUuYXV0aDAuY29tLyIsInN1YiI6ImF1dGgwfDU4MzE4YTFiOWNhNjY2YzY2Yzk2ZDJjNiIsImF1ZCI6IlR0UHpIQ2syUTQ2ODNNZk0wRUN4SGR3QTI2NGNIMjhzIiwiZXhwIjoxNDgyMzgxOTUzLCJpYXQiOjE0ODIzNDU5NTN9.tSgpw1zCYUTtu02agyzNx6B7Vh13-ONJ086QR8muSpQ";

            var client = new FoldersClient(bearerToken);

            var rootFoldes = client.UpdateFolderAsync(1, JObject.FromObject(new
            {
                Name = "Hoi",
                ParentFolderId = (long?)null
            }), CancellationToken.None).Result;
        }
    }
}