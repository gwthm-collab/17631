using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System.Data.SqlClient;
using System.Timers;
using System.Xml;
using System.Collections;


namespace SIHProducerWindowsService
{
    public partial class SIFMDBProducer : ServiceBase
    {
        SqlConnection conn;
        SqlDataReader dataReader;
        SqlCommand selectCommand;
        SqlCommand updtCommand;
        SqlCommand insrtCommand;


        //static string payload = "Sample message";
        static string payload;
        //static string topic = "sih2018";
        static string topic = "gosped1";
        static Uri uri = new Uri("http://192.168.1.100:9092");
        Message msg;
        private Timer timer;

        public SIFMDBProducer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SIFMLogger.WriteEntry("Starting Service");
            conn = new SqlConnection("Server=RAM-LAPTOP;Database=sifms;User Id=sa;Password =sql;MultipleActiveResultSets=true;");
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception e)
            {
                SIFMLogger.WriteEntry(e.Message, EventLogEntryType.Error);
            }
            SIFMLogger.WriteEntry("Connection opened for sql server");

            selectCommand = new SqlCommand("select * from emp_sal where UPPER(flag)='U'", conn);
            updtCommand = new SqlCommand("update emp_sal set flag = 'S' where emp_id = @id", conn);
            insrtCommand = new SqlCommand("insert into tbl_logs (emp_no) values (@empid)", conn);
            SIFMLogger.WriteEntry("DB Connection is configured");
            timer = new Timer(30000);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        protected override void OnStop()   
        {

            SIFMLogger.WriteEntry("Stopping Service");
            timer.Stop();
            timer = null;
            try
            {
                selectCommand = null;
                updtCommand = null;
                insrtCommand = null;
                if (conn != null)
                {
                    conn.Close();
                }
                conn = null;
            }
            catch (Exception e)
            {
                SIFMLogger.WriteEntry(e.Message, EventLogEntryType.Error);
            }

        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {

            int empId;
            var client = new Producer(new BrokerRouter(new KafkaOptions(uri)));
            SIFMLogger.WriteEntry("Starting to send new Message");
            //open connection to sql server
            dataReader = selectCommand.ExecuteReader();
            SIFMLogger.WriteEntry("Receiving message from SQL Server");
            while (dataReader.Read())
            {
                SIFMLogger.WriteEntry("Yet to send string");
                payload = dataReader[0].ToString()+",'"+dataReader[1]+"','" + dataReader[2] + "','" + dataReader[3] + "','" + dataReader[4] + "','" + dataReader[5] + "','" + dataReader[6] + "',"+dataReader[7]+",'"+dataReader[8]+"','"+dataReader[9]+"'";
                empId = Int32.Parse(dataReader[0].ToString());
                msg = new Message(payload);
                try
                {
                    client.SendMessageAsync(topic, new List<Message> { msg });
                    SIFMLogger.WriteEntry("String sent");
                }
                catch (Exception ex)
                {
                    SIFMLogger.WriteEntry(ex.Message, EventLogEntryType.Error);
                }

                updtCommand.Parameters.Clear();
                updtCommand.Parameters.AddWithValue("@id", empId);
                updtCommand.CommandType = CommandType.Text;
                SIFMLogger.WriteEntry("Update employee table flag with query: " + "update emp_sal set flag='S' where emp_id = " + empId);
                try
                {
                    updtCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    SIFMLogger.WriteEntry("Exception: " + e.Message, EventLogEntryType.Error);
                }

                SIFMLogger.WriteEntry("sent update query");
                insrtCommand.Parameters.Clear();
                insrtCommand.Parameters.AddWithValue("@empid", empId);
                insrtCommand.CommandType = CommandType.Text;
                SIFMLogger.WriteEntry("Insert log table value");
                //command.Connection = conn;
                try
                {
                    insrtCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    SIFMLogger.WriteEntry("Exception: " + e.Message, EventLogEntryType.Error);
                }
                //transaction.Commit();
                SIFMLogger.WriteEntry("Sent the log table");
            }

            try
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader = null;
                }
            }
            catch (Exception e)
            {
                SIFMLogger.WriteEntry("Exception: "+e.Message, EventLogEntryType.Error);
            }

            SIFMLogger.WriteEntry("New Message Sent");
        }
    }
}
