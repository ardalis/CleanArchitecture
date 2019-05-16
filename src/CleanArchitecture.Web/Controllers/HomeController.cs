using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            Foo foo = _repository.GetById<Foo>(1);
            Foo originalFoo = new Foo
            {
                Name = foo.Name,
                Bar = new Bar { Number = foo.Bar.Number }
            };
            if (foo == null)
            {
                throw new AbandonedMutexException("Gotta make a foo first!");
            }

            if (foo.Name == "Scott")
            {
                return View(foo);
            }

            foo.Name = "Scott";
            foo.Bar.Number++;

            // Choose which Update method to call here.
            _repository.UpdateUsingOriginalMethod(foo); // Original code, setting entity's state to EntityState.Modified.
            // _repository.UpdateUsingDbContextUpdate(foo); // New code, calling dbContext.Update(entity).

            return View(originalFoo);
        }

        public IActionResult Error() => View();
    }
}
