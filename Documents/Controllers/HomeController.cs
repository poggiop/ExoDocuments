using System.Web;
using System.Web.Mvc;
using HeyRed.MarkdownSharp;
using System.IO;
using Business;
using Services.Interface;
using Helper;
using System.Configuration;
using System.Net.Mime;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout.Element;

namespace Documents.Controllers
{
    public class HomeController : Controller
    {
        private IFolderService _folderService;

        public HomeController(IFolderService folder)
        {
            this._folderService = folder;
        }

        public ActionResult Index()
        {
            var model = new PageModel();

            model.CurrentFolder = this._folderService.getCurrentFoler(ConfigurationManager.AppSettings["DocumentsPath"]);

            return View("Directory", model);
        }

        public ActionResult Test(string path)
        {
            if(!Path.HasExtension(path)){
                return Json(this._folderService.getCurrentFoler(path), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }
        }

        public ActionResult Directory()
        {
            var model = new PageModel();


            return View("Directory");
        }


        public ActionResult ViewDoc(string path)
        {
            var mark = new Markdown();
            var file = System.IO.File.ReadAllText(path);
            //var html = CommonMark.CommonMarkConverter.Convert(file);
            var html = MarkDownHelper.Convert(file, "markdown", "html5");

            IHtmlString str = new HtmlString(html);

            return Content(str.ToString());
        }

        public FileResult DownloadPdf(string path)
        {
            var mark = new Markdown();

            var file = System.IO.File.ReadAllText(path);
            var content = MarkDownHelper.Convert(file, "markdown", "html5");

            byte[] res = null;
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                document.Add(new Paragraph(content));

                HtmlConverter.ConvertToPdf(content, writer);

                res = stream.ToArray();
            }

            return File(res, MediaTypeNames.Application.Pdf, Path.GetFileNameWithoutExtension(path) +".pdf");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}