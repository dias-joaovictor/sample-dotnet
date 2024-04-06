namespace SampleDotnet.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("hello-world")]
public class HelloWorldController : ControllerBase
{

    // private readonly ProductsProducer _productsProducer;

    // private readonly ILogger<HelloWorldController> _logger;

    // public HelloWorldController(ProductsProducer productsProducer, ILogger<HelloWorldController> logger){
    //     _productsProducer = productsProducer;
    //     _logger = logger;
    // }

    [HttpGet]
    public IActionResult Get(){
        Product product = new Product(
                    Guid.NewGuid().ToString(), 
                    GenerateRandomString(100), 
                    new Random().Next(0, 100000)/100m);
        // _logger.LogInformation("Producing Product " + product);
        //_productsProducer.ProduceAsync(product);
        return Ok("Hello World");
    }

    private static string GenerateRandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}