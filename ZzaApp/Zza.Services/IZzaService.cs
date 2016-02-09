using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Zza.Entities;
using Task = Zza.Entities.Task;


namespace Zza.Services
{
    [ServiceContract]
    public interface IZzaService
    {
        [OperationContract]
        List<Client> GetMachineInfo(string hostname);

        [OperationContract]
        List<Task> GetTaskInfo(string task);

        [OperationContract]
        Client GetMachineInfoById(int id);

        [OperationContract]
        Task GetTaskInfoById(int id);


        [OperationContract]
        bool UpdateClientInformation(Client obj);

        [OperationContract]
        bool CreateClient(Client obj);

        [OperationContract]
        void TaskDone(int taskid, string output);

        [OperationContract]
        bool DeleteClient(int id);
        
        [OperationContract]
        List<Task> GetDetailsView(int id);

        [OperationContract]
        List<Schedule> GetScheduleByTaskId(int id);

        [OperationContract]
        bool UpdateTaskInformation(Task obj);

        [OperationContract]
        bool CreateTask(Task obj);

        [OperationContract]
        bool DeleteTask(int id);

        [OperationContract]
        List<Schedule> GetSchedules(string sch);


        [OperationContract]
        string HelloWorld();

        [OperationContract]
        bool CreateSchedule(Schedule obj);
        
        [OperationContract]
        bool DeleteSchedule(int id);

        [OperationContract]
        Schedule GetScheduleInfoById(int id);

          [OperationContract]
        bool UpdateScheduleInformation(Schedule obj);
            
            
    }
}
