
namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        // Seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {
            try
            {
                if (!dbContext.ProductBrands.Any())
                {
                    //read brands from file as string
                    var BrandsData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/brands.json");

                    // transform into C# objects
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                    // add to db & save
                    if (Brands is not null && Brands.Any())
                    {
                        await dbContext.ProductBrands.AddRangeAsync(Brands);
                        await dbContext.SaveChangesAsync();
                    }

                }

                // product type
                if (!dbContext.ProductTypes.Any())
                {
                    //read types from file as string
                    var TypesData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/types.json");

                    // transform into C# objects
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    // add to db & save
                    if (Types is not null && Types.Any())
                    {
                        await dbContext.ProductTypes.AddRangeAsync(Types);
                        await dbContext.SaveChangesAsync();
                    }
                }



                // products
                if (!dbContext.Products.Any())
                {
                    //read products from file as string
                    var ProductsData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/products.json");

                    // transform into C# objects
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                    // add to db & save
                    if (Products is not null && Products.Any())
                    {
                        await dbContext.Products.AddRangeAsync(Products);
                        await dbContext.SaveChangesAsync();
                    }
                }

                // Delivery methods 
                if (!dbContext.DeliveryMethods.Any())
                {
                    //read Delivery Methods from file as string
                    var DeliveryMethodsData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/delivery.json");

                    // transform into C# objects
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                    // add to db & save
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                    {
                        await dbContext.DeliveryMethods.AddRangeAsync(DeliveryMethods);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex) { }

        }
    }
}
