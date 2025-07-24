using AppTaskManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AppTaskManager.Controllers
{
    /// <summary>
    /// Class TaskController contains methods for working with Task
    /// </summary>
    public class TaskController
    {
        /// <summary>
        /// Private field for path of json file
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Private field holding the application name
        /// </summary>
        private readonly string folderName = "AppTaskManager";

        /// <summary>
        /// Private field holding the json-file name
        /// </summary>
        private readonly string fileName = "tasks.json";

        /// <summary>
        /// Constructor for TaskController. It get path for json-file, and initializes this file.
        /// </summary>
        public TaskController()
        {
            string dataFolder = GetPath();

            //Check if the data folder exists
            if (!Directory.Exists(dataFolder))
            {
                //Create the directory if the folder doesn't exist
                Directory.CreateDirectory(dataFolder);
            }

            //Define the path to the json file
            _filePath = Path.Combine(dataFolder, fileName);

            //Ensure the json file exists
            InitializeFile();
        }

        /// <summary>
        /// Get path: This method get path of json-file.
        /// </summary>
        /// <returns></returns>
        private string GetPath()
        {
            //Get the path to the app data
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //Get the folder of the application in roaming
            string appFolder = Path.Combine(appDataPath, folderName);
            //Get the data folder inside the app
            string dataFolder = Path.Combine(appFolder, "data");
            return dataFolder;
        }

        /// <summary>
        /// Initialize file: This method check json-file
        /// </summary>
        private void InitializeFile()
        {
            //Check if the file exists
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Models.Task>()));
            }
        }

        /// <summary>
        /// Load Tasks: This method load and deserialise tasks from json file
        /// </summary>
        /// <returns></returns>
        public List<Models.Task> LoadTasks()
        {
            //Read and deserialise the JSON file
            string fileContent = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Models.Task>>(fileContent);
        }

        /// <summary>
        /// Save Tasks: This method serialize and save list of tasks to JSON file
        /// </summary>
        /// <param name="tasks"></param>
        public void SaveTasks(List<Models.Task> tasks)
        {
            //Serialize end write the list of tasks to the json file
            string json = JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Generate new task id: This method generate new id for new task
        /// </summary>
        /// <returns>new id</returns>
        public int GenerateNewTaskId()
        {
            var tasks = LoadTasks();
            if (!tasks.Any())
            {
                return 1;
            }
            int maxId = tasks.Max(t => t.Id);
            return maxId + 1;
        }

        /// <summary>
        /// Add task: This method add new task into JSON file
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(Models.Task newTask)
        {
            //Generate new Id for new Task
            newTask.Id = GenerateNewTaskId();
            //Loading all tasks from JSON file into List<Task>
            List<Models.Task> tasks = LoadTasks();
            //Adding new task to List<Task>
            tasks.Add(newTask);
            //Saving List<Task> in JSON file
            SaveTasks(tasks);
        }

        /// <summary>
        /// Update task: This method update task into json-file
        /// </summary>
        /// <param name="updatableTask"></param>
        public void UpdateTask(Models.Task updatableTask)
        {
            //Loading all tasks from JSON file to List<Task>
            List<Models.Task> tasks = LoadTasks();
            //Getting task index in List<Task> by updated task id
            int taskIndex = tasks.FindIndex(t => t.Id == updatableTask.Id);
            //If the task index is valid 
            if (taskIndex != -1)
            {
                //Change task into List<Task>
                tasks[taskIndex] = updatableTask;
                //Save List<Task> into JSON file
                SaveTasks(tasks);
            }
        }

        /// <summary>
        /// Delete task: This method delete task from json-file
        /// </summary>
        /// <param name="removableTask"></param>
        public void DeleteTask(Models.Task removableTask)
        {
            //Loading all tasks from JSON file to List<Task>
            List<Models.Task> tasks = LoadTasks();
            //Getting task index in List<Task> by updated task id
            int taskIndex = tasks.FindIndex(t => t.Id == removableTask.Id);
            //If the task index is valid 
            if (taskIndex != -1)
            {
                //Remove task from List<Task>
                tasks.Remove(removableTask);
                //Save List<Task> into JSON file
                SaveTasks(tasks);
            }
        }
    }
}

