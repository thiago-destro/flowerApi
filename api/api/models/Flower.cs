using Newtonsoft.Json;

namespace api.models;

public class Flower
{
    public ushort Id { get; set; }
    
    public string ImageFilePath { get; set; }
    
    public IList<string> Compositions { get; set; } = new List<string>();

    public IList<string> Colors { get; set; } = new List<string>();
    
    public IList<string> Occasions { get; set; } = new List<string>();
    
    public string Background { get; set; }

    public static async Task<Flower> NewAsync(string path)
    {
        Flower flower = new();

        await flower.ReadJsonAsync(path);

        return flower;
    }

    public async Task ReadJsonAsync(string path)
    {
        using var readerStream = new StreamReader(File.OpenRead(path));
        var jsonReader = new JsonTextReader(readerStream);

        _ = await jsonReader.ReadAsync();
        while (await jsonReader.ReadAsync())
        {
            switch (jsonReader.Value)
            {
                case "id":
                    int? readNumber = await jsonReader.ReadAsInt32Async();
                    if (!readNumber.HasValue)
                    {
                        throw new InvalidOperationException("attribute number missing in image definition");
                    }
                    
                    this.Id = (ushort)readNumber;
                    
                    break;
                
                case "imageFilePath":
                    string? readImageFilePath = await jsonReader.ReadAsStringAsync();
                    this.ImageFilePath = readImageFilePath;
                    break;
                
                case "background":
                    string? readBackground = await jsonReader.ReadAsStringAsync();
                    this.Background = readBackground;
                    break;
                   
                case "compositions":
                    this.Compositions.Clear();
                    _ = await jsonReader.ReadAsync();
                    string? readComposition = "";
                    while (!string.IsNullOrEmpty((readComposition = await jsonReader.ReadAsStringAsync())))
                    {
                        if (!string.IsNullOrEmpty(readComposition))
                        {
                            this.Compositions.Add(readComposition);
                        }
                    }
                    
                    break;
                case "colors":
                    this.Colors.Clear();
                    _ = await jsonReader.ReadAsync();
                    string? readColor = "";
                    while (!string.IsNullOrEmpty((readColor = await jsonReader.ReadAsStringAsync())))
                    {
                        if (!string.IsNullOrEmpty(readColor))
                        {
                            this.Colors.Add(readColor);
                        }
                    }
                   
                    break;           
                
                case "ocasions":
                    this.Occasions.Clear();
                    _ = await jsonReader.ReadAsync();
                    string? readOcasion = "";
                    while (!string.IsNullOrEmpty((readOcasion = await jsonReader.ReadAsStringAsync())))
                    {
                        if (!string.IsNullOrEmpty(readOcasion))
                        {
                            this.Occasions.Add(readOcasion);
                        }
                    }
                    
                    break;                   
                
                default:
                    await jsonReader.SkipAsync();
                    break;
            }
        }
        
    }
}