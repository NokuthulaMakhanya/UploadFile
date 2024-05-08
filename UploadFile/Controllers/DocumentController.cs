using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;
using UploadFile.Data;

namespace UploadFile.Controllers
{
    public class DocumentController : Controller
    {
        private readonly DataContext _context;

        public DocumentController(DataContext context)
        {
            _context = context;
        }

        // GET: Document/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Document/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Document model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                // Validate file type
                if (Path.GetExtension(model.File.FileName).ToLower() == ".pdf" || Path.GetExtension(model.File.FileName).ToLower() == ".docx")
                {
                    var document = new Document
                    {
                        FileName = Path.GetFileName(model.File.FileName)
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await model.File.CopyToAsync(memoryStream);
                        document.Data = memoryStream.ToArray();
                    }

                    // Save document to database
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("File", "Only PDF and Word documents are allowed.");
                }
            }
            return View(); 
        }
        // POST: Document/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Document/Index
        public ActionResult Index()
        {
            var documents = _context.Documents.ToList();
            return View(documents);
        }

        // GET: Document/Download/{id}
        public FileResult Download(int id)
        {
            var document = _context.Documents.Find(id);
            if (document != null)
            {
                return File(document.Data, "application/octet-stream", document.FileName);
            }
            else
            {
                return null;
            }
        }
    }

}
