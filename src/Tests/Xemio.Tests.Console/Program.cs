using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
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
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2hhZWZlbGUuZXUuYXV0aDAuY29tLyIsInN1YiI6IndpbmRvd3NsaXZlfDI0ZDc3NmU0OWNmYjcxMDgiLCJhdWQiOiJUdFB6SENrMlE0NjgzTWZNMEVDeEhkd0EyNjRjSDI4cyIsImV4cCI6MTQ4MzU5NjUzMywiaWF0IjoxNDgzNTYwNTMzLCJhenAiOiJUdFB6SENrMlE0NjgzTWZNMEVDeEhkd0EyNjRjSDI4cyJ9.pBjg2bf9H5Ua31MXJgi6jQorI3HZyAgIUJ24EkmDoHo";

            var client = new FoldersClient(bearerToken);
            
            var updateFolder = new UpdateFolder();
            updateFolder.Name = null;

            var rootFoldes = client.UpdateFolderAsync(206, updateFolder, CancellationToken.None).Result;
        }
    }
}