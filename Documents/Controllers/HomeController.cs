using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HeyRed.MarkdownSharp;
using System.IO;
using Business;
using Services.Interface;
using Helper;
using System.Configuration;

namespace Documents.Controllers
{
    public class HomeController : Controller
    {
        private IFolderService _folderService;

        public HomeController() { }

        public HomeController(IFolderService folder)
        {
            this._folderService = folder;
        }

        public ActionResult Index()
        {
            var model = new PageModel();


            model.CurrentFolder = this._folderService.getCurrentFoler(ConfigurationManager.AppSettings["DocumentsPath"]);

            if (model.CurrentFolder.MainDocument != null)
            {
                var mark = new Markdown();
                var file = System.IO.File.ReadAllText(model.CurrentFolder.MainDocument.Path);
                //var html = CommonMark.CommonMarkConverter.Convert(file);
                var html = MarkDownHelper.Convert(file, "markdown", "html5");

                IHtmlString str = new HtmlString(html);
                var PageModel = new PageModel();
                model.Content = str;
            }

            return View("Directory", model);
        }

        public ActionResult Directory(string path)
        {
            var model = new PageModel();

            model.CurrentFolder = this._folderService.getCurrentFoler(path);
            if (model.CurrentFolder.MainDocument != null)
            {
                var mark = new Markdown();
                var file = System.IO.File.ReadAllText(model.CurrentFolder.MainDocument.Path);
                //var html = CommonMark.CommonMarkConverter.Convert(file);
                var html = MarkDownHelper.Convert(file, "markdown", "html5");

                IHtmlString str = new HtmlString(html);
                var PageModel = new PageModel();
                model.Content = str;
            }

            return View("Directory", model);
        }

        public ActionResult ViewDoc(string path)
        {

            var mark = new Markdown();
            var file = System.IO.File.ReadAllText(path);
            //var html = CommonMark.CommonMarkConverter.Convert(file);
            var html = MarkDownHelper.Convert(file, "markdown", "html5");

            IHtmlString str = new HtmlString(html);
            var PageModel = new PageModel();
            PageModel.Content = str;
            PageModel.CurrentFolder = this._folderService.getCurrentFoler(Path.GetDirectoryName(path));

            return View("ViewDoc", PageModel);
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