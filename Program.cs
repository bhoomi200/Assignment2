using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace StudentAssignment
{
    internal class Program
    {
        public static string stuId;
        static string strConnection = "Data Source=BLR1-LHP-N80988\\SQLEXPRESS;Initial Catalog = StudentDetailsDB;Integrated Security=true;";
        static void Main(string[] args)
        {
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("\n------------------------Menu----------------------------");
                Console.WriteLine("1-Add\n2-Display\n3-DisplayAll\n4-Delete\n5-Update\n6-Exit");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Add();
                        break;
                    case 2:
                        Console.WriteLine("Enter the id to display:");
                        int studId = Convert.ToInt32(Console.ReadLine());
                        Display(studId);
                        break;
                    case 3:
                        DisplayAll();
                        break;
                    case 4:
                        Console.WriteLine("Enter the id to delete:");
                        int stud = Convert.ToInt32(Console.ReadLine());
                        Delete(stud);
                        break;
                    case 5:
                        Console.WriteLine("Enter the id to update:");
                        string StudId = Console.ReadLine();
                        Update(StudId);  
                        break;
                    case 6:
                        exit = false;
                        break;
                }
            }
            Console.ReadKey();
        }
        public static void DisplayAll()
        {
            SqlConnection con = new SqlConnection(strConnection);
            con.Open();
            string querystring = "select * from Vstudent_details";
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("-------------------Student Details------------------\t  -------------------Subject Details------------ ");
            Console.WriteLine("StudentId\tName\t\tAddress\t\tClass\t\tSubjectName\tMaxMarks\tMarksObtained");
            while (dr.Read())
            {
                Console.WriteLine(dr[0].ToString() + "\t\t" + dr[1].ToString() + "\t\t" + dr[2].ToString() + "\t\t" + dr[3].ToString() + "\t\t" + dr[4].ToString() + "\t\t" + dr[5].ToString() + "\t\t" + dr[6].ToString());
            }
        }
        public static void Display(int stuId)
        {
            SqlConnection con = new SqlConnection(strConnection);
            con.Open();
            string querystring = "select * from stu_detail('"+ stuId +"')";
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("-------------------Student Details-----------------\t  ---------------------Subject Details------------ ");
            Console.WriteLine("StudentId\tName\t\tAddress\t\tClass\t\tSubjectName\tMaxMarks\tMarksObtained");
            while (dr.Read())
            {
                Console.WriteLine(dr[0].ToString() + "\t\t" + dr[1].ToString() + "\t\t" + dr[2].ToString() + "\t\t" + dr[3].ToString() + "\t\t" + dr[4].ToString() + "\t\t" + dr[5].ToString() + "\t\t" + dr[6].ToString());
            }
        }
        public static void Update(string stuId)
        {
            SqlConnection con = new SqlConnection(strConnection);
            Console.WriteLine("Enter the Name:");
            List<int> subjlist = new List<int>();
            string name = Console.ReadLine();
            Console.WriteLine("Enter the Address:");
            string address = Console.ReadLine();
            Console.WriteLine("Enter the Class:");
            string Class = Console.ReadLine();
            con.Open();
            SqlCommand c = new SqlCommand("exec spUpdateStudent'" + stuId + "','" + name + "','" + address + "','" + Class + "'", con);
            c.ExecuteNonQuery();
            string querystring = "select subjectId from subjects where studentId = '"+stuId+"'";
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int item = dr.GetInt32(dr.GetOrdinal("subjectId"));
                subjlist.Add(item);
            }
            dr.Close();
            for(int i=0; i<subjlist.Count; i++)
            {
                Console.WriteLine("enter subject name");
                string subName = Console.ReadLine();
                Console.WriteLine("enter subject Marks obtained");
                int Subobt = Convert.ToInt32(Console.ReadLine());
                SqlCommand c1 = new SqlCommand("exec spUpdateSubject'" + subName + "','" + Subobt + "','" + subjlist[i] + "'", con);
                c1.ExecuteNonQuery();
            }
            Console.WriteLine("Updated Successfully");
        }
        public static void Add()
        {
            SqlConnection con = new SqlConnection(strConnection);
            Console.WriteLine("Enter the Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the Address:");
            string address = Console.ReadLine();
            Console.WriteLine("Enter the Class:");
            string Class = Console.ReadLine();
            con.Open();
            SqlCommand c = new SqlCommand("exec spAddStudent'" + name + "','" + address + "','" + Class + "'", con);
            c.ExecuteNonQuery();
            string querystring = "select MAX(studentId) from students";
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                stuId = dr[0].ToString();
            }
            dr.Close();
            Console.WriteLine("Enter Number of subjects");
            int no = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < no; i++)
            {
                Console.WriteLine("enter subject name");
                string subName = Console.ReadLine();
                Console.WriteLine("enter subject Marks obtained");
                int Subobt = Convert.ToInt32(Console.ReadLine());
                SqlCommand c1 = new SqlCommand("exec spAddSubject'" + subName + "','" + Subobt + "','" + stuId + "'", con);
                c1.ExecuteNonQuery();
            }
            Console.WriteLine("Added successfully");
        }
        public static void Delete(int stuId)
        {
            SqlConnection con = new SqlConnection(strConnection);
            con.Open();
            string querystring = "delete from students where studentId= '"+stuId+"'";
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("Deleted successfuly");
        }
    }
}
