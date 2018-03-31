using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using KafkaNet;
using KafkaNet.Protocol;
using KafkaNet.Model;
using System.Data.SqlClient;


namespace SIHconsumerservice
{
    public partial class SIHconsumerservice : ServiceBase
    {

        SqlConnection connection;
        SqlCommand insrtCmd;
        SqlCommand logInsrtCmd;
        System.Timers.Timer timer;

        static string topic = "gosped1";
        static Uri uri = new Uri("http://localhost:9092");
        public SIHconsumerservice()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            gospedLog.WriteEntry("Starting Service");
            
            connection = new SqlConnection("SERVER=DESKTOP-HV1C8E0;Database=gosped;User Id=sa;Password=sql;MultipleActiveResultSets=true;");
            try
            {
                connection.Open();
                gospedLog.WriteEntry("DB Connection Open");
            }
            catch (Exception ex)
            {
                gospedLog.WriteEntry("DB Connection Open Erorr:" + ex.Message, EventLogEntryType.Error);

            }
            insrtCmd = new SqlCommand("MERGE emp_sal AS [Target] " +
                                    "USING (SELECT @emp_id AS empid) AS [Source] " +
                                    "ON [Target].emp_id = [Source].empid " +
                                    "WHEN MATCHED THEN " +
                                    "		UPDATE SET [Target].emp_id=@emp_id, " +
                                    "		[Target].emp_name=@emp_name, " +
                                    "		[Target].desig=@desig, " +
                                    "		[Target].dept=@dept, " +
                                    "		[Target].position=@position, " +
                                    "		[Target].off_name=@off_name, " +
                                    "		[Target].dist=@dist, " +
                                    "		[Target].salary=@salary, " +
                                    "		[Target].dob=@dob, " +
                                    "		[Target].doj=@doj " +
                                    "WHEN NOT MATCHED THEN " +
                                    "		INSERT (emp_id, emp_name,desig,dept, " +
                                    "			position,off_name,dist,salary,dob,doj) " +
                                    "		VALUES (@emp_id, @emp_name,@desig,@dept, " +
                                    "			@position,@off_name,@dist,@salary,@dob,@doj); ", connection);
            insrtCmd.CommandType = System.Data.CommandType.Text;
            logInsrtCmd = new SqlCommand("insert into tbl_logs values (@empid)", connection);
            logInsrtCmd.CommandType = System.Data.CommandType.Text;
            gospedLog.WriteEntry("DB Connection is configured");

            // Set up a timer to trigger every minute.  
            timer = new System.Timers.Timer();
            timer.Interval = 15000; // 15 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            gospedLog.WriteEntry("starting to receive new msg");
            var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(new KafkaOptions(uri))));
            gospedLog.WriteEntry("Received data from kafka");
            foreach(var message in consumer.Consume())
            {
                string msg = Encoding.UTF8.GetString(message.Value);
                gospedLog.WriteEntry(Encoding.UTF8.GetString(message.Value) + ", Starting to update DB");
                string[] data = msg.Split(',');
                int indx = 0;
                int empid;
                DateTime dt;

                empid = Int32.Parse(data[indx++]);
                insrtCmd.Parameters.Clear();
                insrtCmd.Parameters.AddWithValue("@emp_id", empid);
                insrtCmd.Parameters.AddWithValue("@emp_name", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@desig", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@dept", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@position", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@off_name", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@dist", data[indx++].Replace("'", ""));
                insrtCmd.Parameters.AddWithValue("@salary", data[indx++].Replace("'", ""));
                try
                {
                    dt = DateTime.ParseExact(data[indx++].Replace("'", ""), "dd-MM-yyyy HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                    insrtCmd.Parameters.AddWithValue("@dob", dt);
                    dt = DateTime.ParseExact(data[indx++].Replace("'", ""), "dd-MM-yyyy HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                    insrtCmd.Parameters.AddWithValue("@doj", dt);
                    insrtCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    gospedLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }

                logInsrtCmd.Parameters.Clear();
                logInsrtCmd.Parameters.AddWithValue("@empid", empid);
                try
                {
                    logInsrtCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    gospedLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
            }

        }

        protected override void OnStop()
        {
            gospedLog.WriteEntry("Stopping Service");
            timer.Stop();
            timer = null;
            try
            {
                insrtCmd = null;
                logInsrtCmd = null;
                if (connection != null)
                {
                    connection.Close();
                }
                connection = null;
            }
            catch (Exception ex)
            {
                gospedLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }
    }

}
