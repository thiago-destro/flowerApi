using System.Net.Mime;
using api.models;

namespace api;

public static class FlowersHelper
{
    public static IList<Flower> Flowers { get; private set; }
    
    public static string ImagesPath { get; private set; }
    
    public static void LoadFlowers(string filesPath)
    {
        ImagesPath = filesPath;
        
        var files = Directory.GetFiles(filesPath, "*.json");
        Flowers = new List<Flower>(files.Length);
        FillFlowersAsync(files).Wait();
    }
    
    private static async Task FillFlowersAsync(string[] files)
    {
        foreach (string file in files)
        {
            Flowers.Add(await Flower.NewAsync(file));
        } 
    }
}