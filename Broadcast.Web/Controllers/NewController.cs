using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Broadcast.Web.Business.Common;
using Broadcast.Web.Business.Common.Enums;
using Broadcast.Web.Business.Enums;
using Broadcast.Web.Business.Interfaces;
using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Broadcast;
using Broadcast.Web.Filters;
using Broadcast.Web.Models.New;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Broadcast.Web.Controllers
{
    public class NewController : Controller
    {
        private readonly IBroadcastService _broadcastService;
        private readonly IBroadcastTypeService _broadcastTypeService;

        public NewController(IBroadcastService broadcastService, IBroadcastTypeService broadcastTypeService)
        {
            _broadcastService = broadcastService;
            _broadcastTypeService = broadcastTypeService;
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_LIST)]
        public ActionResult List(string errorMessage = "")
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }
            ListViewModel model = new ListViewModel();
            model.Filter = new ListFilterViewModel();
            model.CurrentPage = 1;
            model.PageSize = 10;

            BroadcastSearchFilter searchFilter = new BroadcastSearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            if (model.Filter.Filter_ValidationEndDateTime.HasValue)
            {
                searchFilter.Filter_ValidationEndDateTimeAsString = model.Filter.Filter_ValidationEndDateTime.Value.ToString("yyyy-MM-dd");
            }
            searchFilter.Filter_BroadcastTypeId = BroadcastTypeStatic.News;
            model.Filter.Filter_BroadcastTypeId = BroadcastTypeStatic.News;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterIsActiveSelectList = GetIsActiveSelectList(SessionHelper.CurrentLanguageTwoChar);

            var apiResponseModel = _broadcastService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new Business.Models.PaginatedList<BroadcastWithDetail>();
                model.DataList.Items = new List<BroadcastWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_LIST)]
        [HttpPost]
        public ActionResult List(ListViewModel model)
        {
            // filter bilgilerinin default boş değerlerle doldurulması sağlanıyor
            if (model.Filter == null)
            {
                model.Filter = new ListFilterViewModel();
            }

            if (!model.CurrentPage.HasValue)
            {
                model.CurrentPage = 1;
            }

            if (!model.PageSize.HasValue)
            {
                model.PageSize = 10;
            }

            BroadcastSearchFilter searchFilter = new BroadcastSearchFilter();

            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;

            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_BroadcastTypeId = BroadcastTypeStatic.News;
            model.Filter.Filter_BroadcastTypeId = BroadcastTypeStatic.News;
            searchFilter.Filter_TitleEN = model.Filter.Filter_TitleEN;
            searchFilter.Filter_TitleTR = model.Filter.Filter_TitleTR;
            if (model.Filter.Filter_ValidationEndDateTime.HasValue)
            {
                searchFilter.Filter_ValidationEndDateTimeAsString = model.Filter.Filter_ValidationEndDateTime.Value.ToString("yyyy-MM-dd");
            }
            searchFilter.Filter_IsActive = model.Filter.Filter_IsActive;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterIsActiveSelectList = GetIsActiveSelectList(SessionHelper.CurrentLanguageTwoChar);

            var apiResponseModel = _broadcastService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new Business.Models.PaginatedList<BroadcastWithDetail>();
                model.DataList.Items = new List<BroadcastWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_ADD)]
        public ActionResult Add()
        {
            Models.New.AddViewModel model = new AddViewModel();
            model.ValidationEndDateTime = DateTime.Now.AddMonths(1);
            model.IsActive = false;
            model.BroadcastTypeId = BroadcastTypeStatic.News;
            //select lists
            model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            model.SessionGuid = Guid.NewGuid().ToString();
            model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_ADD)]
        [HttpPost]
        public ActionResult Add(Models.New.AddViewModel model, IFormFile ImageFilePath)
        {
            //image ekleme için faydalanılan link -> https://www.webtrainingroom.com/aspnetcore/file-upload
            model.ImageFilePath = ImageFilePath;
            if (ImageFilePath != null)
            {
                string imageFileName = ImageFilePath.FileName;
                model.ImageInformation = ConvertIFormFileToImageInformation(ImageFilePath);
                SessionHelper.SetObject(model.SessionGuid, model.ImageInformation);

            }
            var sessionImage = SessionHelper.GetObject<ImageInformation>(model.SessionGuid);
            if (sessionImage != null)
            {
                model.SessionImageFileName = sessionImage.FileName;
                if (ImageFilePath == null)
                {
                    model.ImageInformation = sessionImage;
                }
            }

            if (!ModelState.IsValid)
            {
                //select lists
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                return View(model);
            }

            //başlıklardan en az birinin girilmesi istenmektedir. Bunun kontrolu işlevidir.
            if (string.IsNullOrEmpty(model.TitleTR) && string.IsNullOrEmpty(model.TitleEN))
            {
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                ViewBag.ErrorMessage = "Title TR Or Title EN Is Required";
                return View(model);
            }

           
            Business.Models.Broadcast.AddBroadcastWithDetail broadcastWithDetail = new Business.Models.Broadcast.AddBroadcastWithDetail();
            broadcastWithDetail.BroadcastTypeId = BroadcastTypeStatic.News;
            broadcastWithDetail.ImageInformation = model.ImageInformation;
            broadcastWithDetail.TitleTR = model.TitleTR;
            broadcastWithDetail.TitleEN = model.TitleEN;
            broadcastWithDetail.DescriptionTR = model.DescriptionTR;
            broadcastWithDetail.DescriptionEN = model.DescriptionEN;
            broadcastWithDetail.VideoFileUrl = model.VideoFileUrl;
            broadcastWithDetail.ValidationEndDateTime = model.ValidationEndDateTime;
            broadcastWithDetail.IsActive = model.IsActive;

            var apiResponseModel = _broadcastService.AddWithDetail(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, broadcastWithDetail);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                SessionHelper.SetObject(model.SessionGuid, null);
                if (SessionHelper.CheckAuthForCurrentUser(AuthCodeStatic.PAGE_NEWS_LIST))
                {
                    return RedirectToAction(nameof(NewController.List), "New");
                }
                return RedirectToAction(nameof(HomeController.Index), "Home"); //todo: şimdilik bu sekilde ayarlandı.
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Kaydedilemedi.";//todo: kulturel olacak NotSaved
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                // todo: select lists
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                return View(model);
            }
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_EDIT)]
        public ActionResult Edit(int id)
        {
            Models.New.AddViewModel model = new AddViewModel();

            var apiBroadcastResponseModel = _broadcastService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiBroadcastResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return RedirectToAction(nameof(NewController.List), "New", new { errorMessage = apiBroadcastResponseModel.ResultStatusMessage });
            }
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null || apiBroadcastResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }
          
            model.SessionGuid = Guid.NewGuid().ToString();
            model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
            model.Id = apiBroadcastResponseModel.Data.Id;
            model.BroadcastTypeId = apiBroadcastResponseModel.Data.BroadcastTypeId;
            model.SessionImageFileName = String.IsNullOrEmpty(apiBroadcastResponseModel.Data.ImageFilePath) ? "" : ConfigHelper.ApiBaseUrl + apiBroadcastResponseModel.Data.ImageFilePath;
            model.ValidationEndDateTime = apiBroadcastResponseModel.Data.ValidationEndDateTime;
            model.IsActive = apiBroadcastResponseModel.Data.IsActive;
            model.TitleTR = apiBroadcastResponseModel.Data.TitleTR;
            model.TitleEN = apiBroadcastResponseModel.Data.TitleEN;
            model.DescriptionTR = apiBroadcastResponseModel.Data.DescriptionTR;
            model.DescriptionEN = apiBroadcastResponseModel.Data.DescriptionEN;
            model.VideoFileUrl = apiBroadcastResponseModel.Data.VideoFileUrl;
           
            //select list
            model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_EDIT)]
        [HttpPost]
        public ActionResult Edit(Models.New.AddViewModel model, IFormFile ImageFilePath)
        {
            //image ekleme için faydalanılan link -> https://www.webtrainingroom.com/aspnetcore/file-upload
            model.ImageFilePath = ImageFilePath;
            if (ImageFilePath != null)
            {
                string imageFileName = ImageFilePath.FileName;
                model.ImageInformation = ConvertIFormFileToImageInformation(ImageFilePath);
                SessionHelper.SetObject(model.SessionGuid, model.ImageInformation);
            }
            var sessionImage = SessionHelper.GetObject<ImageInformation>(model.SessionGuid);
            if (sessionImage != null)
            {
                model.SessionImageFileName = sessionImage.FileName;
                if (ImageFilePath == null)
                {
                    model.ImageInformation = sessionImage;
                }
            }

            if (!ModelState.IsValid)
            {
                //select lists
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                return View(model);
            }

            //başlıklardan en az birinin girilmesi istenmektedir. Bunun kontrolu işlevidir.
            if (string.IsNullOrEmpty(model.TitleTR) && string.IsNullOrEmpty(model.TitleEN))
            {
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                ViewBag.ErrorMessage = "Title TR Or Title EN Is Required";
                return View(model);
            }

            var apiBroadcastResponseModel = _broadcastService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Id);
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null || apiBroadcastResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }

            Business.Models.Broadcast.EditBroadcastWithDetail broadcastWithDetail = new Business.Models.Broadcast.EditBroadcastWithDetail();
            broadcastWithDetail.Id = apiBroadcastResponseModel.Data.Id;
            broadcastWithDetail.BroadcastTypeId = BroadcastTypeStatic.News;
            broadcastWithDetail.ImageInformation = model.ImageInformation;
            broadcastWithDetail.ValidationEndDateTime = model.ValidationEndDateTime;
            broadcastWithDetail.IsActive = false; //güncellendikten sonra  false olarak kaydedilecek 
            broadcastWithDetail.ModifiedBy = SessionHelper.CurrentUser.ID;
            broadcastWithDetail.ModifiedDateTime = DateTime.Now;
            broadcastWithDetail.TitleTR = model.TitleTR;
            broadcastWithDetail.TitleEN = model.TitleEN;
            broadcastWithDetail.DescriptionTR = model.DescriptionTR;
            broadcastWithDetail.DescriptionEN = model.DescriptionEN;
            broadcastWithDetail.VideoFileUrl = model.VideoFileUrl;

            var apiResponseModel = _broadcastService.EditWithDetail(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, broadcastWithDetail);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                SessionHelper.SetObject(model.SessionGuid, null);
                if (SessionHelper.CheckAuthForCurrentUser(AuthCodeStatic.PAGE_NEWS_LIST))
                {
                    return RedirectToAction(nameof(NewController.List), "New");
                }
                return RedirectToAction(nameof(HomeController.Index), "Home"); //todo: şimdilik bu sekilde ayarlandı.
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Kaydedilemedi.";//todo: kulturel olacak NotSaved
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                // todo: select lists
                model.BroadcastTypeSelectList = GetBroadcastTypeSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                return View(model);
            }
        }

        [AppAuthorizeFilter(new string[] { AuthCodeStatic.PAGE_NEWS_ACTIVEPASSIVE })] 
        public ActionResult MakeActive(int id)
        {
            var apiBroadcastResponseModel = _broadcastService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null)
            {
                return View("_ErrorNotExist");
            }

            var apiUpdateStatusResponseModel = _broadcastService.UpdateStatus(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id, true);
            if (apiUpdateStatusResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                string errorMessage = apiUpdateStatusResponseModel.ResultStatusMessage;
                return RedirectToAction(nameof(NewController.List), "New", new { errorMessage = errorMessage });
            }
            else
            {
                return RedirectToAction(nameof(NewController.List), "New");
            }
        }

        [AppAuthorizeFilter(new string[] { AuthCodeStatic.PAGE_NEWS_ACTIVEPASSIVE })]
        public ActionResult MakePassive(int id)
        {
            var apiBroadcastResponseModel = _broadcastService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null)
            {
                return View("_ErrorNotExist");
            }

            var apiUpdateStatusResponseModel = _broadcastService.UpdateStatus(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id, false);
            if (apiUpdateStatusResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                string errorMessage = apiUpdateStatusResponseModel.ResultStatusMessage;
                return RedirectToAction(nameof(NewController.List), "New", new { errorMessage = errorMessage });
            }
            else
            {
                return RedirectToAction(nameof(NewController.List), "New");
            }
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_DISPLAY)]
        public ActionResult Display(int id)
        {
            var apiBroadcastResponseModel = _broadcastService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null || apiBroadcastResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }

            Broadcast.Web.Business.Models.Broadcast.Broadcast model = new Broadcast.Web.Business.Models.Broadcast.Broadcast();
            model = apiBroadcastResponseModel.Data;
            return View(model);
        }



        [NonAction]
        private ImageInformation ConvertIFormFileToImageInformation(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }
            ImageInformation imageInformation = new ImageInformation();
            byte[] imageByteData;
            using (var br = new BinaryReader(imageFile.OpenReadStream()))
            {
                imageByteData = br.ReadBytes((int)imageFile.OpenReadStream().Length);
            }

            imageInformation.ImageByteData = imageByteData;
            imageInformation.FileName = imageFile.FileName;
            imageInformation.Length = imageFile.Length;
            return imageInformation;
        }

        [NonAction]
        [AppAuthorizeFilter]
        private List<SelectListItem> GetBroadcastTypeSelectList(string userToken, string displayLanguage)
        {
            // yayın tipi kayıtları listelenir
            List<SelectListItem> resultList = new List<SelectListItem>();

            // api'den çekim yapılacak
            var apiResponseModel = _broadcastTypeService.GetAll(userToken, displayLanguage);
            resultList = apiResponseModel.Data.OrderBy(r => displayLanguage == "tr" ? r.NameTR : r.NameEN).Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = displayLanguage == "tr" ? r.NameTR : r.NameEN }).ToList();
            return resultList;
        }

        [NonAction]
        private List<SelectListItem> GetIsActiveSelectList(string displayLanguage)
        {
            // aktif veya pasif olarak combobox'ta listelenir
            List<SelectListItem> resultList = new List<SelectListItem>();
            resultList.AddRange(new List<SelectListItem>() {
                new SelectListItem() { Value="0", Text = "No" },
                new SelectListItem() { Value="1", Text = "Yes" }
            });
            return resultList;
        }

    }
}