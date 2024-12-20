using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using ProjectGamma_0;
using Newtonsoft.Json;
using ProjectGamma_0.models;

[Route("api/users")]
[ApiController]
public class JsonFrontReturn : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        string? jsonFilePath = "wwwroot/JsonReg.json"; //Местонахождения Json
        var jsonData = System.IO.File.ReadAllText(jsonFilePath);
        var users = JsonConvert.DeserializeObject<List<JsonReg>>(jsonData);
        return Ok(users);
    }
}



