using api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class FlowersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flower>>> GetFlowers()
    {
        var queryParams = HttpContext.Request.Query;

        ushort? id = null;
        IList<string> compositions = new List<string>();
        IList<string> colors = new List<string>();
        IList<string> occasions = new List<string>();
        string background = string.Empty;
        
        if (queryParams.TryGetValue("id", out StringValues idQuery))
        {
            if (idQuery.Count == 1)
            {
                id = ushort.Parse(idQuery[0]);    
            }
        }
        
        if (queryParams.TryGetValue("background", out StringValues backgroundQuery))
        {
            if (backgroundQuery.Count == 1)
            {
                background = backgroundQuery[0];    
            }
        }
        
        if (queryParams.TryGetValue("compositions", out StringValues compositionsQuery))
        {
            if (compositionsQuery.Count == 1)
            {
                var splitEntries = compositionsQuery[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string entry in splitEntries)
                {
                    compositions.Add(entry);
                }
            }
        }
        
        if (queryParams.TryGetValue("colors", out StringValues colorsQuery))
        {
            if (colorsQuery.Count == 1)
            {
                var splitEntries = colorsQuery[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string entry in splitEntries)
                {
                    colors.Add(entry);
                }
            }
        }

        if (queryParams.TryGetValue("occasions", out StringValues occasionsQuery))
        {
            if (occasionsQuery.Count == 1)
            {
                var splitEntries = occasionsQuery[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string entry in splitEntries)
                {
                    occasions.Add(entry);
                }
            }
        }

        bool hasIdFilter = id.HasValue;
        bool hasBackgroundFilter = !string.IsNullOrEmpty(background);
        bool hasCompositionFilter = compositions.Any();
        bool hasColorsFilter = colors.Any();
        bool hasOccasionsFilter = occasions.Any();

        var flowers =
            FlowersHelper.Flowers.Where(flower => flower.Id == (hasIdFilter ? id.Value : flower.Id))
                .Where(flower => flower.Background == (hasBackgroundFilter ? background : flower.Background))
                .Where(flower => hasCompositionFilter ? flower.Compositions.Intersect(compositions).Any() : true)
                .Where(flower => hasColorsFilter ? flower.Colors.Intersect(colors).Any() : true)
                .Where(flower => hasOccasionsFilter ? flower.Occasions.Intersect(occasions).Any() : true);
            
        return flowers.ToList();

    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Flower>> GetFlower(ushort id)
    {
        var flower = FlowersHelper.Flowers.FirstOrDefault(flower => flower.Id == id);
        if (flower is null) 
            return NotFound();
        else 
            return flower;
    }
    
    [HttpGet("image/{id}")]
    public async Task<ActionResult> GetFlowerImage(ushort id)
    {
        Flower flower = FlowersHelper.Flowers.FirstOrDefault(flower => flower.Id == id);

        if (flower is null)
            return NotFound();
        
        var info = new DirectoryInfo( Path.Combine(FlowersHelper.ImagesPath, flower.ImageFilePath));
        string filename = info.Name;
        string filepath = info.FullName;

        byte[] filedata = await System.IO.File.ReadAllBytesAsync(filepath);
       
        var cd = new System.Net.Mime.ContentDisposition
        {
            FileName = filename,
            Inline = true //attachment
        };

        Response.Headers.Add("Content-Disposition", cd.ToString());
        
        bool hasContentType = new FileExtensionContentTypeProvider().TryGetContentType(filepath, out string contentType);
        if (!hasContentType)
        {
            contentType = "image/jpeg";
        }
        return File(filedata, contentType);
    }
}