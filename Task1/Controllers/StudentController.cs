using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Task1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task1.Controllers
{
    public class StudentController : Controller
    {
        System.Data.SqlClient.SqlConnection _connection;

        public ActionResult Create()
        {
            List<string> branchNames;
            _connection = new SqlConnection("Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;User ID=sa;Password=Welcome2evoke@1234");
            _connection.Open();
            SqlCommand command = new SqlCommand("Select BranchId, BranchName from Branch", _connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                branchNames = new List<string>();
                while (reader.Read())
                {
                    branchNames.Add(reader["BranchName"].ToString());
                }
            }
            ViewBag.branchNames = new SelectList(branchNames);
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
            _connection = new SqlConnection("Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;User ID=sa;Password=Welcome2evoke@1234");
            _connection.Open();
            // Get BranchId based on BranchName
            int branchId;
            using (SqlCommand branchCommand = new SqlCommand("SELECT BranchId FROM Branch WHERE BranchName = @BranchName", _connection))
            {
                Branch branch = new Branch();
                branchCommand.Parameters.AddWithValue("@BranchName", branch.BranchName);
                branchId = (int)branchCommand.ExecuteScalar();
            }

            // Insert data into Student table using the stored procedure
            using (SqlCommand command = new SqlCommand("InsertStudent", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@BranchId", branchId);
                command.Parameters.AddWithValue("@YearName", student.YearName);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Success");
        }
        public IActionResult StudentsDetails()
        {
            _connection = new SqlConnection("Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;User ID=sa;Password=Welcome2evoke@1234");
            _connection.Open();
            SqlCommand cmd = new SqlCommand("StudentDetails", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //_connection.Open();
            List<StudentsDetails> details = new List<StudentsDetails>();
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                details.Add(new StudentsDetails
                {
                    FirstName = (string)r["FirstName"],
                    LastName = (string)r["LastName"],
                    BranchName= (string)r["BranchName"],
                    YearName = (string)r["YearName"]
                });
            }
            _connection.Close();
            return View(details);
        }
       
    }
}
