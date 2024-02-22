// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using SpotifyWPF.lib;


Client client = new Client();
client.connect();

Console.WriteLine("Número de col: ");
int col = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Número de row: ");
int row = Convert.ToInt32(Console.ReadLine());

client.actions.validar(col, row);