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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LatestWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        //again using the dependency injetion to get the application path to folder//

        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //Adding an API method get all the employee details

//        EmployeeID EmployeeName    Department DateJointed ProfileName
//1	Fazil IT	2021-06-18	faz
//2	Ruchika Support	2021-06-18	ruchii
        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                        select EmployeeID, EmployeeName,Department,
                        convert(varchar(10),DateJointed,120) as DateJointed, ProfileName         
                        from dbo.Employee";
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

            return new JsonResult(table);
        }

        //Adding a post method to insert DATA

        [HttpPost]

        public JsonResult Post(Employee emp)
        {
            string query = @"
                        insert into dbo.Employee values ('" + emp.EmployeeName +"','"+emp.Department+"','"+emp.DateJointed+"','"+emp.ProfileName+"'@)";
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

            return new JsonResult("Employee Added Succesfully");
        }


        //Adding a PUT method to update data

        [HttpPut]

        public JsonResult Put(Employee emp)
        {
            string query = @"
                        update dbo.Employee set
                        EmployeeName = '" + emp.EmployeeName + @"'
                        Department = '" + emp.Department + @"'
                        DateJointed = '" + emp.DateJointed + @"'
                        ProfileName = '" + emp.ProfileName + @"'
                        where EmployeeID = " + emp.EmployeeID + @";
";
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

            return new JsonResult("Employee Updated Succesfully");
        }


        //Adding a DELETE method to delete data from sql server//

        //when you want to add the parameter in the url, insert it to post http

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from dbo.Employee 
                        where EmployeeID = " + id + @";
";
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

            return new JsonResult("Employee Deleted Succesfully");
        }

        //API method to save photos

         [Route("SaveFile")]
         [HttpPost]

         public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;


                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("annonymous.png");
            }
        }
    }
}
