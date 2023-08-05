using Newtonsoft.Json;

namespace api.models;

public class Image
{
    public ushort Number { get; set; }
    
    public string ImageFilePath { get; set; }
    
    public IList<string> Compositions { get; set; } = new List<string>();

    public IList<string> Colors { get; set; } = new List<string>();
    
    public IList<string> Ocasions { get; set; } = new List<string>();
    
    public string Background { get; set; }


    public void ReadJson(string path)
    {
        using var readerStream = new StreamReader(File.OpenRead(path));
        var jsonReader = new JsonTextReader(readerStream);

        while (jsonReader.Read())
        {
            switch (jsonReader.Value)
            {
                case "number":
                    int? readNumber = jsonReader.ReadAsInt32();
                    if (!readNumber.HasValue)
                    {
                        throw new InvalidOperationException("attribute number missing in image definition");
                    }
                    
                    this.Number = (ushort)readNumber;
                    
                    break;
                
                case "imageFilePath":
                    string? readImageFilePath = jsonReader.ReadAsString();
                    this.Background = readImageFilePath;
                    break;                
                    
                case "compositions":
                    this.Compositions.Clear();
                    while (jsonReader.Read())
                    {
                        string? readComposition = jsonReader.ReadAsString();
                        if (!string.IsNullOrEmpty(readComposition))
                        {
                            this.Compositions.Add(readComposition);
                        }
                    }
                    
                    break;
                case "colors":
                    this.Colors.Clear();
                    while (jsonReader.Read())
                    {
                        string? readColor = jsonReader.ReadAsString();
                        if (!string.IsNullOrEmpty(readColor))
                        {
                            this.Colors.Add(readColor);
                        }
                    }
                    
                    break;           
                
                case "ocasions":
                    this.Ocasions.Clear();
                    while (jsonReader.Read())
                    {
                        string? readOcasions = jsonReader.ReadAsString();
                        if (!string.IsNullOrEmpty(readOcasions))
                        {
                            this.Colors.Add(readOcasions);
                        }
                    }
                    
                    break;                   
                
                case "background":
                    string? readBackground = jsonReader.ReadAsString();
                    this.Background = readBackground;
                    break;
            }
        }
        
    }
}