using DbConnector.Tools;
using PraiseTeam.Models.Domain;
using PraiseTeam.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PraiseTeam.Services
{
    public class FaqCategoryService : BaseService, IFaqCategoryService
    {
        public IEnumerable<FaqCategory> SelectAll()
        {
            return Adapter.LoadObject<FaqCategory>("dbo.FaqCategory_SelectAll");
        }

        public FaqCategory SelectById(int id)
        {
            return Adapter.LoadObject<FaqCategory>("dbo.FaqCategory_SelectById", new[] { new SqlParameter("@Id", id) }).FirstOrDefault();
        }

        public int Insert(FaqCategoryAddRequest model)
        {
            //SqlParameter idOut = new SqlParameter("@Id", 0);
            //idOut.Direction = System.Data.ParameterDirection.Output;
            int id = 0;
            Adapter.ExecuteQuery("dbo.FaqCategory_Insert",
                new[]
            {
                SqlDbParameter.Instance.BuildParameter("@Title", model.Title, System.Data.SqlDbType.NVarChar, 200),
                SqlDbParameter.Instance.BuildParameter("@Description", model.Description, System.Data.SqlDbType.NVarChar, 4000),
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

        public void Update(FaqCategoryUpdateRequest model) //change out void to int 
        {
            //int id = 0;
            Adapter.ExecuteQuery("dbo.FaqCategory_Update",
                new[]
            {
                SqlDbParameter.Instance.BuildParameter("@Title", model.Title, System.Data.SqlDbType.NVarChar, 200),
                SqlDbParameter.Instance.BuildParameter("@Description", model.Description, System.Data.SqlDbType.NVarChar, 4000),
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
            return Adapter.ExecuteQuery("dbo.FaqCategory_Delete", new[]
            {
               SqlDbParameter.Instance.BuildParameter("@Id", id, System.Data.SqlDbType.Int)
            });
        }

    }
}
