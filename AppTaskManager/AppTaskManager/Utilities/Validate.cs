namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Validate for validation input data
    /// </summary>
    public class Validate
    {
        /// <summary>
        /// Validate string
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static public bool ValidateString(string inputString, int length)
        {
            if (String.IsNullOrEmpty(inputString)) return false;
            if(inputString.Length < length) return false;
            return true;
        }

        /// <summary>
        /// Trim input string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public string TrimInputString(string inputString)
        {
            return inputString.Trim();
        }
    }
}
