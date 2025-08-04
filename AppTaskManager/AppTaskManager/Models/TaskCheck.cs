using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaskManager.Models
{
    public class TaskCheck
    {
        public int Id { get; set; }
        public TaskModel TaskModel { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
