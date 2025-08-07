using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaskManager.Controllers
{
    public interface ITaskController
    {
        IEnumerable<TaskModel> GetAllTasks();
        void SaveTasks(IEnumerable<TaskModel> tasks);
        void AddTask(TaskModel newTask);

        void UpdateTask(TaskModel task);

        void DeleteTask(TaskModel task);
        public int GenerateNewTaskId();
    }
}
