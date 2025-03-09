


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Talabat.Apis.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductRepo, IMapper mapper,IGenericRepository<ProductType> TypeRepo, IGenericRepository<ProductBrand> BrandRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
        }



        // Get All Products
        [HttpGet]
        public async Task<ActionResult<Pagination<ProdcutToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProdcutWithBrandAndTypeSpecifications(Params);

            var products = await _productRepo.GetAllAsyncWithSpecifications(Spec);

            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProdcutToReturnDto>>(products);

            //var ReturnedObject = new Pagination<ProdcutToReturnDto>()
            //{
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data = MappedProducts
            //};

            var CountSpec = new ProductWithFiltrationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProdcutToReturnDto>(Params.PageIndex, Params.PageSize, Count, MappedProducts));
        }

        // Get product by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdcutToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProdcutToReturnDto>> GetProductById(int id)
        {
            var Spec = new ProdcutWithBrandAndTypeSpecifications(id);

            var product = await _productRepo.GetEntityAsyncWithSpecifications(Spec);
            if(product is null)
                return NotFound(new APIResponse(404));

            var MappedProduct = _mapper.Map<Product, ProdcutToReturnDto>(product);
            return Ok(MappedProduct);
        }




        // Get All Types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }


        // Get All Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
