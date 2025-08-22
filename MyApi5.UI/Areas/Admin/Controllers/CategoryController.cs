using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;
using MyApi5.Business.Helpers;
using MyApi5.UI.Filters;
using MyApi5.UI.Resources;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApi5.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AuthFilter))]
    public class CategoryController : Controller
    {
        private readonly HttpClient _client;
        public CategoryController(HttpClient client)
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.GetAsync("https://localhost:7007/api/Category"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true};

                    string info = await response.Content.ReadAsStringAsync();
                    //var resource = JsonConvert.DeserializeObject<List<CategoryGetResource>>(info);
                    var resource1 = JsonSerializer.Deserialize<List<CategoryGetResource>>(info, options);
                    return View(resource1);
                }   
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                }
            }
            return View();
        }
        public async Task<IActionResult> Create()
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            //if(request.Name !=null && request.FormFile != null)
            //{
            //    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IncludeFields = true, WriteIndented = true };

            //var content = new StringContent(JsonSerializer.Serialize(request, options), Encoding.UTF8, "multipart/form-data");
            //    using (HttpResponseMessage response = await _client.PostAsync("https://localhost:7007/api/Category", content))
            //    {
            //        var bodyResponse = await response.Content.ReadAsStringAsync();
            //        if (response.IsSuccessStatusCode)
            //        {
            //            return RedirectToAction("Index");
            //        }
            //        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //        {
            //            ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(bodyResponse, options);
            //            if (errorResponse.Errors != null)
            //            {
            //                foreach (var item in errorResponse.Errors)
            //                {
            //                    ModelState.AddModelError(item.Key, item.ErrorMessage);
            //                }
            //                return View();
            //            }
            //        }
            //        else
            //        {
            //            TempData["Error"] = "Something went wrong.";
            //        }
            //    }
            //}

            //return Ok();


            // MultipartFormDataContent yaradılır
            using var formContent = new MultipartFormDataContent();

            // 'Name' propertisini əlavə edin
            if (!string.IsNullOrEmpty(request.Name))
            {
                formContent.Add(new StringContent(request.Name,Encoding.UTF8), "Name");
            }

            // 'FormFile' faylını əlavə edin
            if (request.FormFile != null)
            {
                var fileContent = new StreamContent(request.FormFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.FormFile.ContentType);
                formContent.Add(fileContent, "FormFile", request.FormFile.FileName);
            }

            // API-yə sorğu göndərin
            using HttpResponseMessage response = await _client.PostAsync("https://localhost:7007/api/Category", formContent);

            var bodyResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                // JSON-u düzgün oxuyaraq xətaları göstərin
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(bodyResponse, options);

                if (errorResponse != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        ModelState.AddModelError(error.Key, error.ErrorMessage);
                    }
                }
                return View(request);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Error"] = "Something went wrong.";
            }

            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (HttpResponseMessage response = await _client.GetAsync("https://localhost:7007/api/Category/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var categoryDto = await response.Content.ReadFromJsonAsync<CategoryGetResource>();
                    //var categoryDto = JsonSerializer.Deserialize<CategoryGetResource>(await response.Content.ReadAsStringAsync());


                    ViewData["IdKeeper"] = id;
                    CategoryUpdateRequest updateRequest = new CategoryUpdateRequest();
                    updateRequest.categoryGet = categoryDto;

                    return View(updateRequest);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStringAsync());
                    if (errorResponse?.Errors != null)
                    {
                        foreach (var error in errorResponse.Errors)
                        {
                            ModelState.AddModelError(error.Key, error.ErrorMessage);
                        }
                    }
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateRequest updateDto)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            using (var multiContent = new MultipartFormDataContent())
            {
                if (!string.IsNullOrEmpty(updateDto.categoryCreate.Name))
                {
                    var content = new StringContent(updateDto.categoryCreate.Name);

                    multiContent.Add(content, "Name");
                }
                if (updateDto.categoryCreate.FormFile != null)
                {
                    var fileContent = new StreamContent(updateDto.categoryCreate.FormFile.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(updateDto.categoryCreate.FormFile.ContentType);
                    multiContent.Add(fileContent, "FormFile", updateDto.categoryCreate.FormFile.FileName);
                }
                using (HttpResponseMessage response = await _client.PutAsync("https://localhost:7007/api/Category/update/" + updateDto.categoryGet.Id, multiContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStreamAsync(), options);
                        if (errorResponse != null)
                        {
                            foreach (var item in errorResponse.Errors)
                            {
                                ModelState.AddModelError(item.Key, item.ErrorMessage);
                            }
                        }
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong.";
                    }

                    return View(updateDto);
                }
            }

        }
        public async Task<IActionResult> Delete(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);
            using (var response = await _client.DeleteAsync("https://localhost:7007/api/Category/" + id))
            {
                if(response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();  
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return StatusCode(500);
                }

            }
        }
    }
}
