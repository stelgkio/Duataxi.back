using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DuaTaxi.Common.WebApiClient
{
    public class SrvResp<T>
    {

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Content { get; set; }
        public int TotalCount { get; set; }
        public Dictionary<string, string> ErrorData { get; private set; }

        public SrvResp()
        {
            Success = true;
        }

        public SrvResp(T result)
        {
            Success = true;
            Content = result;
        }

        public SrvResp(T result, int count)
        {
            Success = true;
            Content = result;
            TotalCount = count;
        }

        public SrvResp(ValidationException vex)
        {

            if (vex.ErrorEntity == null)
            {
                ErrorMessage = "ST_VALIDATION_ERROR";
                return;
            }


            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(vex.ErrorEntity.Error))
            {
                sb.AppendLine(vex.ErrorEntity.Error);
            }

         

        }

        public SrvResp(Exception ex)
        {

            StringBuilder sb = new StringBuilder();

            Success = false;

            if (ex == null)
            {
                return;
            }

            ErrorMessage = ex.Message;

            if (ex is ValidationException vex)
            {
                if (vex.ErrorEntity != null && vex.ErrorEntity.Error.Length > 0)
                {
                    ErrorMessage = vex.ErrorEntity.Error;
                    return;
                }
            }

           

            string toRemove = "See the inner exception for details.";
            string line;
            string last = "";

            while (ex.InnerException != null)
            {
                line = ex.InnerException.Message;
                if (line.EndsWith(toRemove))
                {
                    line = line.Substring(0, line.IndexOf(toRemove));
                }
                if (!last.Contains(line))
                {
                    sb.AppendLine(line);
                    last = line;
                }
                ex = ex.InnerException;
            }

            ErrorMessage += sb.ToString();
        }
    }


    public class SrvResp
    {

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> ErrorData { get; private set; }

        public SrvResp()
        {
            Success = true;
        }

        public SrvResp(bool result)
        {
            Success = result;
        }



        public SrvResp(ValidationException vex)
        {

            if (vex.ErrorEntity == null)
            {
                ErrorMessage = "ST_VALIDATION_ERROR";
                return;
            }


            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(vex.ErrorEntity.Error))
            {
                sb.AppendLine(vex.ErrorEntity.Error);
            }

         

        }

        public SrvResp(Exception ex)
        {

            StringBuilder sb = new StringBuilder();

            Success = false;

            if (ex == null)
            {
                return;
            }

            ErrorMessage = ex.Message;

          

            string toRemove = "See the inner exception for details.";
            string line;
            string last = "";

            while (ex.InnerException != null)
            {
                line = ex.InnerException.Message;
                if (line.EndsWith(toRemove))
                {
                    line = line.Substring(0, line.IndexOf(toRemove));
                }
                if (!last.Contains(line))
                {
                    sb.AppendLine(line);
                    last = line;
                }
                ex = ex.InnerException;
            }

            ErrorMessage += sb.ToString();
        }
    }

}
