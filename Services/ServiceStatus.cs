using Inveasy.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Inveasy.Services
{
    public class ServiceStatus 
    {
        public const string Error = "ERROR";
        public const string Warning = "WARNING";
        public const string ExceptionRaised = "EXCEPTION RAISED";
        public const string Success = "SUCCESS";

        protected string _entity = "";       

        public ServiceStatus(string entity) 
        {
            this._entity = entity;
        }

        #region GET status templates
        public string SuccessGetStatus(string entityName) 
        {
            return $"{Success} - Succesfully fetched {entityName} from database";
        }

        public string WarningGetStatus(string message)
        {
            return WarningStatus(message);
        }

        public string ErrorGetStatus(string message)
        {
            return FailedGetStatus(Error, message);
        }

        public string ExceptionGetStatus(string message)
        {
            return FailedGetStatus(ExceptionRaised, message);
        }
        #endregion

        #region ADD status templates
        public string SuccessAddStatus(string entityName)
        {
            return $"{Success} - {_entity} {entityName} successfully added to database";
        }

        public string ErrorAddStatus(string message, string entityName)
        {
            return FailedAddStatus(Error, message, entityName);
        }

        public string ErrorAddStatus(string message)
        {
            return FailedAddStatus(Error, message, null);
        }

        public string WarningAddStatus(string message)
        {
            return WarningStatus(message) ;
        }

        public string ExceptionAddStatus(string message, string entityName)
        {
            return FailedAddStatus(ExceptionRaised, message, entityName);
        }

        public string ExceptionAddStatus(string message)
        {
            return FailedAddStatus(ExceptionRaised, message, null);
        }
        #endregion

        #region UPDATE status templates
        public string SuccessUpdateStatus(string entityName, List<string> updatedFields)
        {           
            return $"{Success} - Succesfully updated {_entity} {entityName} fields {String.Join(",", updatedFields)}";
        }

        public string SuccessUpdateStatus(string entityName)
        {
            return $"{Success} - Succesfully updated {_entity} {entityName}";
        }

        public string WarningUpdateStatus(string message)
        {
            return WarningStatus(message);
        }

        public string ErrorUpdateStatus(string message, string entityName)
        {
            return FailedUpdateStatus(Error, message, entityName);
        }

        public string ErrorUpdateStatus(string message)
        {
            return FailedUpdateStatus(Error, message, null);
        }

        public string ExceptionUpdateStatus(string message, string entityName)
        {
            return FailedUpdateStatus(ExceptionRaised, message, entityName);
        }
        #endregion

        #region DELETE status templates
        public string SuccessDeleteStatus(string entityName)
        {
            return $"{Success} - Succesfully deleted {_entity} from database";
        }

        public string WarningDeleteStatus(string message)
        {
            return WarningStatus(message);
        }

        public string ErrorDeleteStatus(string message, string entityName)
        {
            return FailedDeleteStatus(Error, message, entityName);
        }

        public string ErrorDeleteStatus(string message)
        {
            return FailedDeleteStatus(Error, message, null);
        }

        public string ExceptionDeleteStatus(string message, string entityName)
        {
            return FailedDeleteStatus(ExceptionRaised, message, entityName);
        }
        #endregion

        #region Core status templates
        private string WarningStatus(string message)
        {
            return $"{Warning} - " + $"{(message != null ? $": {message}" : "")}";
        }
       
        private string FailedGetStatus(string status, string message)
        {
            return $"{status} - Unable to fetch {_entity}s{(message != null ? $": {message}" : "")}";
        }

        private string FailedAddStatus(string status, string message, string entityName)
        {
            return $"{status} - Unable to add {_entity}" +
                $"{(entityName != null ? $" {entityName}" : "")} to database" +
                $"{(message != null ? $": {message}" : "")}";
        }

        private string FailedUpdateStatus(string status, string message, string entityName)
        {
            return $"{status} - Unable to update {_entity} {entityName}";
        }

        private string FailedDeleteStatus(string status, string message, string entityName)
        {
            return $"{status} - Unable to delete {_entity} {entityName}";
        }
        #endregion       

        #region Services subclasses
        public class UserStatus : ServiceStatus
        {           
            public UserStatus(): base(nameof(User)) { }                        
        }

        public class ProjectStatus : ServiceStatus
        {
            public ProjectStatus() : base(nameof(Project)) { }
        }

        public class DonationStatus : ServiceStatus
        {            
            public DonationStatus() : base(nameof(Donation)) { }
        }

        public class ViewStatus : ServiceStatus
        {            
            public ViewStatus() : base(nameof(View)) { }
        }

        public class CommentStatus : ServiceStatus
        {
            public CommentStatus() : base(nameof(Comment)) { }
        }

        public class RoleStatus : ServiceStatus
        {            
            public RoleStatus() : base(nameof(Role)) { }
        }

        public class CategoryStatus : ServiceStatus
        {            
            public CategoryStatus() : base(nameof(Category)) { }
        }
        #endregion
    }
}
