using GQ.Database;
using GQ.Entities;
using Microsoft.EntityFrameworkCore;

namespace GQ.Seeder;

internal static class Program
{
    private static DataContext _db = null!;

    private static void Main()
    {
        var opts = new DbContextOptionsBuilder<DataContext>();
        opts.UseSqlite(@"Data Source=C:\Users\juarias\RiderProjects\POC\GQ\GQ.Api\GraphQl-POC.db;");
        _db = new DataContext(opts.Options);

        SeedProductTypes();
        SeedProducts();
    }

    private static void SeedProducts()
    {
        var products = new List<Product>
        {
            new Product { Name = "Shirt", Code = "001", ProductTypeId = 1 },
            new Product { Name = "Pants", Code = "002", ProductTypeId = 1 },
            new Product { Name = "Hats", Code = "003", ProductTypeId = 1 },
            new Product { Name = "Shoes", Code = "004", ProductTypeId = 1 },
            new Product { Name = "Jackets", Code = "005", ProductTypeId = 1 },
            new Product { Name = "Socks", Code = "006", ProductTypeId = 1 },
            new Product { Name = "Sweaters", Code = "007", ProductTypeId = 1 },
            new Product { Name = "T-Shirts", Code = "008", ProductTypeId = 1 },
            new Product { Name = "Jeans", Code = "009", ProductTypeId = 1 },
            new Product { Name = "HW Hammer", Code = "010", ProductTypeId = 2 },
            new Product { Name = "HW Screwdriver", Code = "011", ProductTypeId = 2 },
            new Product { Name = "HW Wrench", Code = "012", ProductTypeId = 2 },
            new Product { Name = "HW Saw", Code = "013", ProductTypeId = 2 },
            new Product { Name = "HW Drill", Code = "014", ProductTypeId = 2 },
            new Product { Name = "HW Crowbar", Code = "015", ProductTypeId = 2 },
            new Product { Name = "HW Welder", Code = "016", ProductTypeId = 2 },
            new Product { Name = "HW Soldering Iron", Code = "017", ProductTypeId = 2 },
            new Product { Name = "HW Shovel", Code = "018", ProductTypeId = 2 },
            new Product { Name = "Toy Car", Code = "019", ProductTypeId = 3 },
            new Product { Name = "Toy Ball", Code = "020", ProductTypeId = 3 },
            new Product { Name = "Toy Bird", Code = "021", ProductTypeId = 3 },
            new Product { Name = "Toy Cat", Code = "022", ProductTypeId = 3 },
            new Product { Name = "Toy Dog", Code = "023", ProductTypeId = 3 },
            new Product { Name = "Toy Horse", Code = "024", ProductTypeId = 3 },
            new Product { Name = "Toy Mouse", Code = "025", ProductTypeId = 3 },
            new Product { Name = "Toy Rabbit", Code = "026", ProductTypeId = 3 },
            new Product { Name = "Toy Snake", Code = "027", ProductTypeId = 3 },
            new Product { Name = "Toy Spider", Code = "028", ProductTypeId = 3 },
            new Product { Name = "Laptop", Code = "029", ProductTypeId = 4 },
            new Product { Name = "Phone", Code = "030", ProductTypeId = 4 },
            new Product { Name = "Tablet", Code = "031", ProductTypeId = 4 },
            new Product { Name = "Computer", Code = "032", ProductTypeId = 4 },
            new Product { Name = "Mouse", Code = "033", ProductTypeId = 4 },
            new Product { Name = "Keyboard", Code = "034", ProductTypeId = 4 },
            new Product { Name = "Headphones", Code = "035", ProductTypeId = 4 },
            new Product { Name = "Camera", Code = "036", ProductTypeId = 4 },
            new Product { Name = "Speaker", Code = "037", ProductTypeId = 4 },
            new Product { Name = "Monitor", Code = "038", ProductTypeId = 4 },
            new Product { Name = "Headset", Code = "039", ProductTypeId = 4 },
            new Product { Name = "Power Bank", Code = "040", ProductTypeId = 4 },
            new Product { Name = "Charger", Code = "041", ProductTypeId = 4 },
            new Product { Name = "Microwave", Code = "042", ProductTypeId = 5 },
            new Product { Name = "Oven", Code = "043", ProductTypeId = 5 },
            new Product { Name = "Toaster", Code = "044", ProductTypeId = 5 },
            new Product { Name = "Fridge", Code = "045", ProductTypeId = 5 },
            new Product { Name = "Washing Machine", Code = "046", ProductTypeId = 5 },
            new Product { Name = "Dishwasher", Code = "047", ProductTypeId = 5 },
            new Product { Name = "Blender", Code = "048", ProductTypeId = 5 },
            new Product { Name = "Kettle", Code = "049", ProductTypeId = 5 },
            new Product { Name = "Microwave Oven", Code = "050", ProductTypeId = 5 },
            new Product { Name = "Stove", Code = "051", ProductTypeId = 5 },
            new Product { Name = "Furnace", Code = "052", ProductTypeId = 5 },
            new Product { Name = "Linen", Code = "053", ProductTypeId = 6 },
            new Product { Name = "Cotton", Code = "054", ProductTypeId = 6 },
            new Product { Name = "Polyester", Code = "055", ProductTypeId = 6 },
            new Product { Name = "Wool", Code = "056", ProductTypeId = 6 },
            new Product { Name = "Engine", Code = "057", ProductTypeId = 7 },
            new Product { Name = "Transmission", Code = "058", ProductTypeId = 7 },
            new Product { Name = "Brakes", Code = "059", ProductTypeId = 7 },
            new Product { Name = "Wheels", Code = "060", ProductTypeId = 7 },
            new Product { Name = "Suspension", Code = "061", ProductTypeId = 7 },
            new Product { Name = "Tires", Code = "062", ProductTypeId = 7 },
            new Product { Name = "Kitchen Knife", Code = "063", ProductTypeId = 8 },
            new Product { Name = "Fork", Code = "064", ProductTypeId = 8 },
            new Product { Name = "Spoon", Code = "065", ProductTypeId = 8 },
            new Product { Name = "Bowl", Code = "066", ProductTypeId = 8 },
            new Product { Name = "Plate", Code = "067", ProductTypeId = 8 },
            new Product { Name = "Cutting Board", Code = "068", ProductTypeId = 8 },
            new Product { Name = "Mixing Bowl", Code = "069", ProductTypeId = 8 },
            new Product { Name = "Pan", Code = "070", ProductTypeId = 8 },
            new Product { Name = "Spatula", Code = "071", ProductTypeId = 8 },
            new Product { Name = "Soap", Code = "072", ProductTypeId = 9 },
            new Product { Name = "Shampoo", Code = "073", ProductTypeId = 9 },
            new Product { Name = "Deodorant", Code = "074", ProductTypeId = 9 },
            new Product { Name = "Toothpaste", Code = "075", ProductTypeId = 9 },
            new Product { Name = "Toothbrush", Code = "076", ProductTypeId = 9 },
            new Product { Name = "Mosquito Repellent", Code = "077", ProductTypeId = 9 },
            new Product { Name = "Hand Sanitizer", Code = "078", ProductTypeId = 9 },
            new Product { Name = "Tissue Box", Code = "079", ProductTypeId = 9 },
            new Product { Name = "Tissue Tube", Code = "080", ProductTypeId = 9 },
            new Product { Name = "Tissue Paper", Code = "081", ProductTypeId = 9 },
            new Product { Name = "Tissue Shampoo", Code = "082", ProductTypeId = 9 },
            new Product { Name = "Tissue Soap", Code = "083", ProductTypeId = 9 },
            new Product { Name = "Tissue Deodorant", Code = "084", ProductTypeId = 9 },
            new Product { Name = "Trash bags", Code = "085", ProductTypeId = 9 },
            new Product { Name = "Windows", Code = "086", ProductTypeId = 10 },
            new Product { Name = "Mac", Code = "087", ProductTypeId = 10 },
            new Product { Name = "Linux", Code = "088", ProductTypeId = 10 },
            new Product { Name = "Chrome", Code = "089", ProductTypeId = 10 },
            new Product { Name = "Firefox", Code = "090", ProductTypeId = 10 },
            new Product { Name = "Edge", Code = "091", ProductTypeId = 10 },
            new Product { Name = "Safari", Code = "092", ProductTypeId = 10 },
            new Product { Name = "Internet Explorer", Code = "093", ProductTypeId = 10 },
            new Product { Name = "Opera", Code = "094", ProductTypeId = 10 },
            new Product { Name = "Chrome OS", Code = "095", ProductTypeId = 10 },
            new Product { Name = "Android", Code = "096", ProductTypeId = 10 },
            new Product { Name = "Word", Code = "097", ProductTypeId = 10 },
            new Product { Name = "Excel", Code = "098", ProductTypeId = 10 },
            new Product { Name = "PowerPoint", Code = "099", ProductTypeId = 10 },
            new Product { Name = "Outlook", Code = "100", ProductTypeId = 10 },
            new Product { Name = "OneNote", Code = "101", ProductTypeId = 10 },
            new Product { Name = "Access", Code = "102", ProductTypeId = 10 },
            new Product { Name = "Publisher", Code = "103", ProductTypeId = 10 },
            new Product { Name = "Visio", Code = "104", ProductTypeId = 10 },
            new Product { Name = "Project", Code = "105", ProductTypeId = 10 },
            new Product { Name = "SharePoint", Code = "106", ProductTypeId = 10 },
            new Product { Name = "Skype", Code = "107", ProductTypeId = 10 },
            new Product { Name = "Teams", Code = "108", ProductTypeId = 10 },
            new Product { Name = "Notepad++", Code = "109", ProductTypeId = 10 },
            new Product { Name = "Visual Studio Code", Code = "110", ProductTypeId = 10 },
            new Product { Name = "Visual Studio", Code = "111", ProductTypeId = 10 },
            new Product { Name = "Git", Code = "112", ProductTypeId = 10 },
            new Product { Name = "GitHub", Code = "113", ProductTypeId = 10 },
            new Product { Name = "Sublime Text", Code = "114", ProductTypeId = 10 },
            new Product { Name = "MS SqlServer", Code = "115", ProductTypeId = 10 },
            new Product { Name = "Oracle", Code = "116", ProductTypeId = 10 },
            new Product { Name = "MySQL", Code = "117", ProductTypeId = 10 },
            new Product { Name = "PostgreSQL", Code = "118", ProductTypeId = 10 },
            new Product { Name = "MongoDB", Code = "119", ProductTypeId = 10 },
            new Product { Name = "Docker", Code = "120", ProductTypeId = 10 },
            new Product { Name = "Kubernetes", Code = "121", ProductTypeId = 10 },
            new Product { Name = "Azure", Code = "122", ProductTypeId = 10 },
            new Product { Name = "AWS", Code = "123", ProductTypeId = 10 }
        };

        _db.Products.AddRange(products);
        _db.SaveChanges();
    }

    private static void SeedProductTypes()
    {
        var types = new List<ProductType>
        {
            new()
            {
                Code = "001",
                Name = "Apparel"
            },
            new()
            {
                Code = "002",
                Name = "Hardware"
            },
            new()
            {
                Code = "003",
                Name = "Toys"
            },
            new()
            {
                Code = "004",
                Name = "Electronics"
            },
            new()
            {
                Code = "005",
                Name = "House Appliances"
            },
            new()
            {
                Code = "006",
                Name = "Linen"
            },
            new()
            {
                Code = "007",
                Name = "Car Parts"
            },
            new()
            {
                Code = "008",
                Name = "Kitchen tools"
            },
            new()
            {
                Code = "009",
                Name = "Cleaning Products"
            },
            new()
            {
                Code = "010",
                Name = "Software"
            }
        };

        _db.ProductTypes.AddRange(types);
        _db.SaveChanges();
    }
}