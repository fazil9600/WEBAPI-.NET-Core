using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LatestWEBAPI.Models;

namespace LatestWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Adding an API method get all the department details
        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                        select DepartmentID, DepartmentName from dbo.department";
            DataTable table = new DataTable();
            string sqldatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader  myReader;
            using(SqlConnection myCon = new SqlConnection(sqldatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        //Adding a post method to insert DATA

        [HttpPost]

        public JsonResult Post(Department dep)
        {
            string query = @"
                        insert into dbo.Department values ('"+dep.DepartmentName+@"')";
            DataTable table = new DataTable();
            string sqldatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqldatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Department Added Succesfully");
        }

    }
}
