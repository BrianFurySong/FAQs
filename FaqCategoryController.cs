using PraiseTeam.Models.Domain;
using PraiseTeam.Models.Requests;
using PraiseTeam.Models.Responses;
using PraiseTeam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PraiseTeam.Controllers.Api
{
    [RoutePrefix("api/FaqCategories")]

    public class FaqCategoryController : ApiController
    {
        IFaqCategoryService _faqCategoryService;

        public FaqCategoryController(IFaqCategoryService faqCategoryService)
        {
            _faqCategoryService = faqCategoryService;
        }

        [Route(), HttpGet]
        public HttpResponseMessage SelectAll()
        {
            try
            {
                IEnumerable<FaqCategory> faqCategory = _faqCategoryService.SelectAll();
                return Request.CreateResponse(HttpStatusCode.OK, faqCategory);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage SelectById(int id)
        {
            try
            {
                FaqCategory faqCategory = _faqCategoryService.SelectById(id);
                return Request.CreateResponse(HttpStatusCode.OK, faqCategory);
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        [Route(), HttpPost]
        public HttpResponseMessage Post(FaqCategoryAddRequest model)
        {
            try
            {
                int id = _faqCategoryService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Put(FaqCategoryUpdateRequest model)
        {
            try
            {
                _faqCategoryService.Update(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _faqCategoryService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
