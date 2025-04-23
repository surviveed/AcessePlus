using AcessePlus.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("gerenciador/example")]
public class ExampleController : Controller
{
    // Mocked data list
    private static List<ExampleDTO> _mockData = new List<ExampleDTO>
    {
        new ExampleDTO { Id = 1, Name = "Example 1", Description = "This is the first example" },
        new ExampleDTO { Id = 2, Name = "Example 2", Description = "This is the second example" }
    };

    // GET: /gerenciador/example
    [Route("")]
    public IActionResult List([FromQuery] int? page)
    {
        ViewBag.Example = _mockData;
        return View();
    }

    // GET: /gerenciador/example/edit/{id?}
    [Route("edit/{id?}")]
    public IActionResult Edit(int? id)
    {
        var model = id.HasValue
            ? _mockData.FirstOrDefault(x => x.Id == id.Value)
            : new ExampleDTO();

        return View(model);
    }

    // POST: /gerenciador/example/edit-form/{id?}
    [Route("edit-form/{id?}")]
    public IActionResult Insert([FromForm] ExampleDTO model, int? id)
    {
        if (id.HasValue)
        {
            var existing = _mockData.FirstOrDefault(x => x.Id == id.Value);
            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Description = model.Description;
            }
        }
        else
        {
            model.Id = _mockData.Any() ? _mockData.Max(x => x.Id) + 1 : 1;
            _mockData.Add(model);
        }

        return RedirectToAction("List");
    }

    // POST: /gerenciador/example/delete-form/{id}
    [Route("delete-form/{id}")]
    public IActionResult Delete(int id)
    {
        var item = _mockData.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _mockData.Remove(item);
        }

        return RedirectToAction("List");
    }
}
