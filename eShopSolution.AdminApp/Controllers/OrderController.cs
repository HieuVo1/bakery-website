using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Orders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SelectPdf;

namespace eShopSolution.AdminApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string FILE_SAVE_FOLDER_NAME = "QtBinariesWindows";
        private const string url = "https://localhost:5003/order/detail/";

        public OrderController(ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IOrderService orderService) : base(languageService, categoryService, configuration)
        {
            _orderService = orderService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var result = await _orderService.GetAll();
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            if (result.IsSuccessed)
            {
                ViewData["orders"] = result.ResultObject;
            }
            return View();
        }
        public async Task<IActionResult> Detail(int orderId)
        {
            var result = await _orderService.GetOrderDetail(orderId);
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            var order = await _orderService.GetById(orderId);
            if (result.IsSuccessed)
            {
                ViewData["orderDetails"] = result.ResultObject;
                ViewData["order"] = order.ResultObject;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Print(string HtmlString,int orderId)
        {
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(HtmlString);
            byte[] pdf = pdfDocument.Save();
            pdfDocument.Close();
            return File(pdf, "application/pdf", $"Invoice-{orderId}.pdf");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int orderId)
        {
            var result = await _orderService.Delete(orderId);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "Delete Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Index", "order");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, int status)
        {
            var result = await _orderService.UpdateStatus(status,orderId);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "update Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Detail", "order", new { orderId= orderId });
        }
    }
}