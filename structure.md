entities
dbContext
controller
-home
-user
--service
--viewmodels
--validation
-product
security

  a. Гледаме html View-to
  b. Ако нямаме @Model или foreach или if else - slagame
  c. Ako iskame ViewModel
  d. IProductService
  e. ProductService
  f. Controller
  
----------------------  
 
 return this.Redirect("/");
 
 или
 
 return this.View();
 
 или
 
return this.View(product);
 
--------------------------------- 
  

http://localhost/Home/Index

Validations се добавя в контролерите.

# 1. Entities:

user:
--> IdentityUser

-----------------------------------
public class User : IdentityUser<string>
{

}
-----------------------------------    


# 2. DbContext
--> DbSet

public class AndreysDbContext : DbContext
    
    {
        public AndreysDbContext()
        {

        }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseSqlServer(@"Server=LILLY\SQLEXPRESS;Database=Andreys;Integrated Security=True");

            base.OnConfiguring(optionsBuilder);
        }
    }


-----------------------------------

# 3. Проверка в Startup

using (var db = new AndreysDbContext())
            {
                db.Database.EnsureCreated();
            }
            
-----------------------------------

# 4. Проверявам HomeController
и Views/Home/Index.html

-----
Ctrl + F5
http://localhost/Home/Index в Chrome
-----

-----------------------------------

# 5. Shared/_Layout.html - проверяваме функционалността на User-a и си пишем @if проверки какво да се вижда:

@if(User == null)
                    {
                    <li class="nav-item">
                        <a class="nav-link text-white active h5" href="/Users/Login">Login</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white active h5" href="/Users/Login">Register</a>
                    </li>
                    }
                    @else {
                    <li class="nav-item">
                        <a class="nav-link text-white active h5" href="/Products/Add">Add Product</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-danger active h5" href="/Logout">Logout</a>
                    </li>
                    }
                    
-----------------------------------

# 6. Users Controller създаваме и го вземаме от Ники 
public class UsersController : Controller
    {
    }

-----------------------------------

# 7.Правим си public interface IUsersService и си го вземаме от Github от Services    

public interface IUsersService
    {
        string GetUserId(string username, string password);

        void Register(string username, string email, string password);

        bool UsernameExists(string username);

        bool EmailExists(string email);

        string GetUsername(string id);
    }
    
-----------------------------------    

# 8. UsersService  - пак го вземаме от Ники и фиксваме:

ApplicationDbContext - {imeto na bazata}DbContext

-----------------------------------

# 9. Връщаме се в UsersController и трябва да си направим 2 модела във ViewModels/Users
--> RegisterInputModel
--> LoginInputModel

-----------------------------------

# 10. Validation: В UsersController правим проверки за логването за име и парола, например:
if (input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Password must be at least 6 characters and at most 20");
            }

            if (input.Username.Length < 4 || input.Username.Length > 10)
            {
                return this.Error("Username must be at least 4 characters and at most 10");
            }
            


# 11. Съобщение означава да регистрираме нашите сървиси в Startup.cs / ConfigureServices:

public void ConfigureServices(IServiceCollection serviceCollection)
{
    serviceCollection.Add<IUsersService, UsersService>();

}

System.NullReferenceException: Object reference not set to an instance of an object.
   at SIS.MvcFramework.ServiceCollection.CreateInstance(Type type) in C:\Users\Administrator\Desktop\CSharpWebExamPrep\Exam-Framework\SIS.MvcFramework\ServiceCollection.cs:line 32
   at SIS.MvcFramework.ServiceCollection.CreateInstance(Type type) in C:\Users\Administrator\Desktop\CSharpWebExamPrep\Exam-Framework\SIS.MvcFramework\ServiceCollection.cs:line 34
   
# 12. Register не работи --> търсим в Shared/_Layout.html

# 13. Грешка:
System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation.
 ---> System.InvalidOperationException: Unable to track an entity of type 'User' because primary key property 'Id' is null.

Решение:
в UsersService --> Id = Guid.NewGuid().ToString():

public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Role = IdentityRole.User,
                Username = username,
                Email = email,
                Password = this.Hash(password),
            };
            this.db.Users.Add(user);
            this.db.SaveChanges();
        }
        
        
# 14. Ако виждаме http://localhost/Home/Users/Logout вместо http://localhost/Users/Logout
то в Share/_Layout e забравен '/' пред Users


# 15. В HomeController си слагаме, за да отваря Index на / t.e. http://localhost/:
[HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return this.Index();
        }

# 16. ProductsController - create

# 17. Add
public HttpResponse Add()
        {
            return this.View();
        }
        
---> Post метода го правим от Get, но му слагаме [HttpPost].
После си правим ProductAddInputModel като ползваме модела (без Required)

enum-ite ги правим на string

# 18. IProductsService

int Add(ProductAddInputModel productAddInputModel); --> връща id-to, за да си го ползваме

Add() приема всичко от модела на Product (без id)

# 19. ProductsService (implement Add())

# 20. Добавяме AndreysDbContext dbContext с конструктор, за да записваме в базата
private readonly AndreysDbContext dbContext;

        public ProductsService(AndreysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


# 21. var product = new Product()
{

};

Ctrl + space показва всичко нужно

enum се парсва:

--> var genderAsEnum = Enum.Parse<Gender>(productAddInputModel.Gender);
    
--> правим си product и го запазваме в базата    
 public int Add(ProductAddInputModel productAddInputModel)
        {
            var genderAsEnum = Enum.Parse<Gender>(productAddInputModel.Gender);
            var categoryAsEnum = Enum.Parse<Category>(productAddInputModel.Category);

            var product = new Product()
            {
                Name = productAddInputModel.Name,
                Description = productAddInputModel.Description,
                ImageUrl = productAddInputModel.ImageUrl,
                Price = productAddInputModel.Price,
                Gender = genderAsEnum,
                Category = categoryAsEnum
            };

            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();

            return product.Id;
        }    


# 22. Startup - serviceCollection

serviceCollection.Add<IProductsService, ProductsService>();


# 23. Не се добавя, защото няма конструктор със Service -a за добавяне (Add(...)) в ProductsController

private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

Service добавя продукта, който е получен в Post на контролера:
и Post метода става:

[HttpPost]
public HttpResponse Add(ProductAddInputModel inputModel)
{
    var productId = this.productsService.Add(inputModel);

    return this.View();
}

# 24. Ако липсва name в някой html View, дава SQL Exception 

# 25. В HomeController - ако се е логнал user-a, ще му подадем view

public HttpResponse Index()
        {
            if (this.IsUserLoggedIn())
            {
                var allProducts = productsService.GetAll();
                return this.View(allProducts, "Home");
            }
            return this.View();
        }

        
Всички продукти се вземат от нашия сървиз.


 public IEnumerable<Product> GetAll()
        {
             //first variant
            //return this.dbContext.Products.ToArray();

            //second variant
            return this.dbContext.Products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price
            }).ToArray();
        }
    
  # 26. View compilation errors: The name 'item' does not exist in the current context
  Разглеждаме Home.html
  
  # 27. Details

  
  Products/Details.html
  
  Ако в html нямаме това, което е в базата - слагаме си @Model.
  -----------
  в Interface IProductsService:
  Product GetById(int id);
  -----------
  в ProductsService
  
  public Product GetById(int id)
        {
            return this.dbContext.Products.FirstOrDefault(x => x.Id == id);
        }
        
-----------

в ProductsController:

public HttpResponse Details(int id)
        {
            var product = this.productsService.GetById(id);

            return this.View(product);
        }       
        
        
# 28. Delete
controller:

public HttpResponse Delete(int id)
        {
            this.productsService.DeleteById(id);

            return this.Redirect("/");
        }
        

service:

public void DeleteById(int id)
        {
            var product = this.GetById(id);
            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();
        }    
        
# 29. Security в контролер
public HttpResponse Add(ProductAddInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            if (inputModel.Name.Length < 4 || inputModel.Name.Length > 20)
            {
                return this.View();
            }

            if (inputModel.Description.Length < 10)
            {
                return this.View();
            }

            var productId = this.productsService.Add(inputModel);

            return this.View();
        }

