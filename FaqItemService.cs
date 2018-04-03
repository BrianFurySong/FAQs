using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DbConnector.Adapter;
using DbConnector.Tools;
using PraiseTeam.Models;
using PraiseTeam.Models.Domain;
using PraiseTeam.Models.Requests;

namespace PraiseTeam.Services
{
    public class FaqItemService : BaseService, IFaqItemService
    {

        public IEnumerable<FaqItem> SelectAll()
        {
            return Adapter.LoadObject<FaqItem>("dbo.FaqItem_SelectAll");
        }

        public IEnumerable<FaqItem> SelectAllFaqItemsInCategory()
        {
            return Adapter.LoadObject<FaqItem>("dbo.FaqItem_SelectAllFaqItemsInCategory");
        }

        public FaqItem SelectById(int id)
        {
            return Adapter.LoadObject<FaqItem>("dbo.FaqItem_SelectById", new[] { new SqlParameter("@Id", id) }).FirstOrDefault();
        }

        public int Insert(FaqItemAddRequest model)
        {
            //SqlParameter idOut = new SqlParameter("@Id", 0);
            //idOut.Direction = System.Data.ParameterDirection.Output;
            int id = 0;
            Adapter.ExecuteQuery("dbo.FaqItem_Insert",
                new[]
            {
                SqlDbParameter.Instance.BuildParameter("@FaqCategoryId", model.FaqCategoryId, System.Data.SqlDbType.Int),
                SqlDbParameter.Instance.BuildParameter("@Question", model.Question, System.Data.SqlDbType.NVarChar, 200),
                SqlDbParameter.Instance.BuildParameter("@Answer", model.Answer, System.Data.SqlDbType.NVarChar, 4000),
                SqlDbParameter.Instance.BuildParameter("@DisplayOrder", model.DisplayOrder, System.Data.SqlDbType.Int),
                SqlDbParameter.Instance.BuildParameter("@IsActive", model.IsActive, System.Data.SqlDbType.Bit),
                SqlDbParameter.Instance.BuildParameter("@Id", 0, System.Data.SqlDbType.Int, paramDirection: System.Data.ParameterDirection.Output)

                //new SqlParameter("@Title", model.Title), THIS COULD BE IT BUT TOP ONE IS COOLER
                //new SqlParameter("@Message", model.Message),
                //new SqlParameter("@StackTrace", model.StackTrace),
                //new SqlParameter("@ErrorSourceTypeId", model.ErrorSourceTypeId),
            },
            (parameters =>
            {
                id = parameters.GetParamValue<int>("@Id");//an extention against the IDBparameters array
                //int.TryParse(parameters[4].Value.ToString(), out id);
            }
            ));
            return id;
        }

        public void Update(FaqItemUpdateRequest model) //change out void to int 
        {
            //int id = 0;
            Adapter.ExecuteQuery("dbo.FaqItem_Update",
                new[]
            {
                SqlDbParameter.Instance.BuildParameter("@FaqCategoryId", model.FaqCategoryId, System.Data.SqlDbType.Int),
                SqlDbParameter.Instance.BuildParameter("@Question", model.Question, System.Data.SqlDbType.NVarChar, 200),
                SqlDbParameter.Instance.BuildParameter("@Answer", model.Answer, System.Data.SqlDbType.NVarChar, 4000),
                SqlDbParameter.Instance.BuildParameter("@DisplayOrder", model.DisplayOrder, System.Data.SqlDbType.Int),
                SqlDbParameter.Instance.BuildParameter("@IsActive", model.IsActive, System.Data.SqlDbType.Bit),
                SqlDbParameter.Instance.BuildParameter("@Id", model.Id, System.Data.SqlDbType.Int)
            }); //get rid of the );
                //,
                //(parameters =>
                //{
                //    id = parameters.GetParamValue<int>("@Id");//an extention against the IDBparameters array
                //}
                //));
                //return id;
        }

        public int Delete(int id)
        {
            return Adapter.ExecuteQuery("dbo.FaqItem_Delete", new[]
            {
               SqlDbParameter.Instance.BuildParameter("@Id", id, System.Data.SqlDbType.Int)
            });
        }
    }
}
