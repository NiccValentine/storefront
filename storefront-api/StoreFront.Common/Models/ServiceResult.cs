namespace StoreFront.Common.Models
{
    using System.Collections.Generic;

    public class ServiceResult<T>
    {
        #region Constructors
        public ServiceResult()
        {
            this.Messages = new List<Message>();
        }

        #endregion

        #region Public Methods


        public bool IsSuccessful { get; set; }


        public bool IsValid { get; set; }

        public List<Message> Messages { get; set; }

        public T Object { get; set; }

        #endregion
    }
}