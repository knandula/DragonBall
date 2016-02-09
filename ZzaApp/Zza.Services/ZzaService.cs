using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading;
using Zza.Data;
using Zza.Entities;

namespace Zza.Services
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class ZzaService : IZzaService, IDisposable
    {
        readonly ZzaDbContext _Context = new ZzaDbContext();

        
        public List<Client> GetMachineInfo(string hostname)
        {
            var principal = Thread.CurrentPrincipal;
            using (var ctx = new ZzaDbContext())
            {
                return ctx.Clients.Where(m => m.HostName.StartsWith(hostname) || hostname == null).ToList();
            }
        }

        public List<Task> GetDetailsView(int id)
        {
            var principal = Thread.CurrentPrincipal;

            using (var ctx = new ZzaDbContext())
            {
                return  ctx.Tasks.Where(x => x.ClientID == id).ToList();
            }
           
        }
     

        public void Dispose()
        {
            _Context.Dispose();
        }


        public string HelloWorld()
        {
            return "h world test 2 reflecting";
        }


        public bool DeleteClient(int id)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var tasks = ctx.Tasks.Where(x => x.ClientID == id).ToList();

                    if (tasks.Any())
                    {
                        foreach (var tsk in tasks)
                        {
                            ctx.Tasks.Remove(tsk);
                            ctx.SaveChanges();
                        }
                    }

                    var client = ctx.Clients.First(m => m.ID == id);
                    ctx.Clients.Remove(client);
                    ctx.SaveChanges();
                    return true;
                } 
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        public Client GetMachineInfoById(int id)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    return ctx.Clients.First(m => m.ID == id);
                }
            }
            catch (Exception)
            {
                return null;
            }
           
        }

        public bool UpdateClientInformation(Client obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var q = ctx.Clients.First(m => m.ID == obj.ID);
                    if (q != null)
                    {
                        q.HostName = obj.HostName;
                        q.IpAddress = obj.IpAddress;
                        q.Username = obj.Username;
                        q.Password = obj.Password;
                        ctx.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool CreateClient(Client obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var q = ctx.Clients.Add(obj);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool UpdateTaskInformation(Task obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var q = ctx.Tasks.First(m => m.taskID == obj.taskID);
                    if (q != null)
                    {
                        q.Command = obj.Command;
                        q.Path = obj.Path;
                        q.TaskName = obj.TaskName;
                        q.TaskType = obj.TaskType;
                        q.Status = "Queued";
                        q.OutputLog = string.Empty;
                        ctx.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateTask(Task obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var q = ctx.Tasks.Add(obj);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTask(int taskid)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var task = ctx.Tasks.FirstOrDefault(x => x.taskID == taskid);
                    if (task != null)
                    {
                        ctx.Tasks.Remove(task);
                        ctx.SaveChanges();
                    }
                    
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }


        public Task GetTaskInfoById(int id)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    return ctx.Tasks.First(m => m.taskID == id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        

        public List<Task> GetTaskInfo(string task)
        {
            var principal = Thread.CurrentPrincipal;
            using (var ctx = new ZzaDbContext())
            {
                return ctx.Tasks.ToList();
            }
        }


        public List<Schedule> GetSchedules(string sch)
        {
            var principal = Thread.CurrentPrincipal;
            using (var ctx = new ZzaDbContext())
            {
                return ctx.Schedules.ToList();
            }
        }



        public void TaskDone(int taskid, string output)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var q = ctx.Tasks.First(m => m.taskID == taskid);
                    q.Status = "Done";
                    q.OutputLog = output;
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                
            }
        }


        public List<Schedule> GetScheduleByTaskId(int id)
        {
            var principal = Thread.CurrentPrincipal;

            using (var ctx = new ZzaDbContext())
            {
                return ctx.Schedules.Where(x => x.taskID == id).ToList();
            }
        }


        public bool DeleteSchedule(int id)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var task = ctx.Schedules.FirstOrDefault(x => x.sID == id);
                    if (task != null)
                    {
                        ctx.Schedules.Remove(task);
                        ctx.SaveChanges();
                    }
                    
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Schedule GetScheduleInfoById(int id)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    return ctx.Schedules.First(m => m.sID == id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdateScheduleInformation(Schedule obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {
                    var query = ctx.Tasks.FirstOrDefault(x => x.taskID == obj.taskID);

                    if (query != null)
                    {
                        query.Status = "Queued";
                        query.OutputLog = string.Empty;

                        ctx.SaveChanges();
                    }

                    var q = ctx.Schedules.First(m => m.sID == obj.sID);
                    if (q != null)
                    {
                        q.endAt = obj.endAt;
                        q.startAt = obj.startAt;
                        q.intHours = obj.intHours;
                        q.intMin = obj.intMin;
                        q.intSec = obj.intSec;
                        q.repeatforever = obj.repeatforever;
                        ctx.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool CreateSchedule(Schedule obj)
        {
            try
            {
                using (var ctx = new ZzaDbContext())
                {

                    var query = ctx.Tasks.FirstOrDefault(x => x.taskID == obj.taskID);

                    if (query != null)
                    {
                        query.Status = "Queued";
                        query.OutputLog = string.Empty;

                        ctx.SaveChanges();
                    }
                    var q = ctx.Schedules.Add(obj);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


     
    }
}
