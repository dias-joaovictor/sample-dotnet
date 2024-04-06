namespace SampleDotnet.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{

    private readonly ProductService _productService;

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductService productService, ILogger<ProductsController> logger){
        _productService = productService;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult GetAll(){
        return Ok(_productService.getAllProducts());
    }

}