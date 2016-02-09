using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Quartz;
using Quartz.Impl;
using Zza.ExecutionEngine.ZzaService;
using Task = Zza.ExecutionEngine.ZzaService.Task;

namespace Zza.ExecutionEngine
{
    public class Program
    {
        public static ISchedulerFactory SchedFact;
        public static IScheduler Sched;
        public static ISchedulerFactory SchedFactSchedulerFactory;
        public static IScheduler Scheduler;
        public static List<Task> Jobs { get; set; }
        public static List<Client> Machines { get; set; }
        public static List<Schedule> Schedules { get; set; }
        
        
        
        

        private static void Main(string[] args)
        {
            Console.WriteLine(" Scheduler Initializing . Please wait.");
            Init();
            InitJobsWithSchedule();
            GetJobs();
            GetMachines();
            GetSchedules();
            Console.WriteLine(" Scheduler Running Queued Jobs.");
            RunJobs();
            ScheduleJobs();
            Console.WriteLine(" Scheduler Listening ...");
          
            Timer watch = new Timer();
            watch.Interval = 1000 * 30;
            watch.Elapsed += watch_Elapsed;
            watch.Start();
        }

        private static void GetSchedules()
        {
            ZzaService.ZzaServiceClient client = new ZzaServiceClient();
            Schedules = client.GetSchedules(null).ToList();
        }

        private static void ScheduleJobs()
        {
            int priority = Jobs.Count;
            foreach (var tsk in Jobs.Where(x => x.TaskType == "S" && x.Status == "Queued")) // O - On Demand ; S- Scheduled.
            {
                var schedule = Schedules.FirstOrDefault(x => x.taskID == tsk.taskID);
                var client = Machines.FirstOrDefault(x => x.ID == tsk.ClientID);

                IJobDetail job = JobBuilder.Create<Job>().WithIdentity(tsk.TaskName, tsk.ClientID.ToString()).Build();
                job.JobDataMap["command"] = tsk.Command;
                job.JobDataMap["taskname"] = tsk.TaskName;
                job.JobDataMap["TaskId"] = tsk.taskID;
                if (client != null)
                {

                    job.JobDataMap["remotepc"] = "\\\\" + client.HostName;
                    job.JobDataMap["username"] = client.Username;
                    job.JobDataMap["password"] = client.Password;
                }


                if (schedule != null)
                {
                    TriggerBuilder tb = TriggerBuilder.Create();
                    tb.WithIdentity(tsk.TaskName);

                    if (schedule.repeatforever <= 0)
                    {
                        tb.WithSimpleSchedule(a => a.WithIntervalInHours(schedule.intHours));
                        tb.WithSimpleSchedule(a => a.WithIntervalInSeconds(schedule.intSec));
                        tb.WithSimpleSchedule(a => a.WithIntervalInMinutes(schedule.intMin));
                    }
                    else
                    {
                       if(schedule.intHours > 0)  tb.WithSimpleSchedule(a => a.WithIntervalInHours(schedule.intHours).RepeatForever());
                       if (schedule.intHours > 0) tb.WithSimpleSchedule(a => a.WithIntervalInSeconds(schedule.intSec).RepeatForever());
                       if (schedule.intHours > 0) tb.WithSimpleSchedule(a => a.WithIntervalInMinutes(schedule.intMin).RepeatForever());
                    }

                    if (schedule.startAt != null) tb.StartAt(new DateTimeOffset((DateTime) schedule.startAt));
                    if(schedule.endAt != null) tb.EndAt(new DateTimeOffset((DateTime) schedule.endAt));

                  

                    tb.ForJob(job);
                    ITrigger trigger = tb.Build();
                    Scheduler.ScheduleJob(job, trigger);
                }

                
            }
        }
        private static void InitJobsWithSchedule()
        {
            SchedFactSchedulerFactory = new StdSchedulerFactory();
            Scheduler = SchedFactSchedulerFactory.GetScheduler();
            Scheduler.Start();
        }

        static void watch_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetJobs();
            RunJobs();
            GetSchedules();
            ScheduleJobs();
        }

        private static void RunJobs()
        {
            int priority = Jobs.Count;
            foreach (var tsk in Jobs.Where(x=> x.TaskType == "O" && x.Status == "Queued")) // O - On Demand ; S- Scheduled.
            {
                var client = Machines.FirstOrDefault(x => x.ID == tsk.ClientID);

                IJobDetail job = JobBuilder.Create<Job>().WithIdentity(tsk.TaskName, tsk.ClientID.ToString()).Build();
                job.JobDataMap["command"] = tsk.Command;
                job.JobDataMap["taskname"] = tsk.TaskName;
                job.JobDataMap["TaskId"] = tsk.taskID;

                if (client != null)
                {

                    job.JobDataMap["remotepc"] = "\\\\" + client.HostName;
                    job.JobDataMap["username"] = client.Username;
                    job.JobDataMap["password"] = client.Password;
                }

                ITrigger trigger = TriggerBuilder.Create().WithIdentity(tsk.TaskName, tsk.ClientID.ToString()).StartNow().WithPriority(priority--).Build();
                Sched.ScheduleJob(job, trigger);
            }
        }  

        private static void GetMachines()
        {
            ZzaService.ZzaServiceClient client = new ZzaServiceClient();
            Machines = client.GetMachineInfo(null).ToList();
        }
        private static void GetJobs()
        {
            ZzaService.ZzaServiceClient client = new ZzaServiceClient();
            Jobs = client.GetTaskInfo(null).ToList();
        }
        private static void Init()
        {
            SchedFact = new StdSchedulerFactory();
            Sched = SchedFact.GetScheduler();
            Sched.Start();
        }}

    public class Job : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string output;
            ZzaService.ZzaServiceClient client = new ZzaServiceClient();
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Console.WriteLine(string.Format("{0} is executing.", dataMap.GetString("taskname")));
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.Arguments = dataMap.GetString("remotepc") + " -u " + dataMap.GetString("username") + " -p " +
                                  dataMap.GetString("password") + " -d -i " + dataMap.GetString("command");
            startInfo.FileName = "PsExec ";
            startInfo.WorkingDirectory = @"c:\windows\system32\";
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            
            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader1 = process.StandardOutput)
                {
                    output= reader1.ReadToEnd();
                }
                using (StreamReader reader2 = process.StandardError)
                {
                    output += reader2.ReadToEnd(); 
                }
            }

            if (string.IsNullOrEmpty(output))
                output = "Job Ran Successfully";

            dataMap.Add("Output",output);
            client.TaskDone(dataMap.GetIntValue("TaskId"),output);
        }
    }
}
