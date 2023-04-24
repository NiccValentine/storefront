namespace StoreFront.Common.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Service result class
    /// </summary>
    /// <typeparam name="T">A instance of the <see cref="ModelBase{T}"/> class</typeparam>
    public class ServiceResult<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class
        /// </summary>
        public ServiceResult()
        {
            this.Messages = new List<Message>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets or sets a value indicating whether or nor the action was succeeded
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or nor the data is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets a list of instances of the <see cref="Message"/> class
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets an instance of T
        /// </summary>
        public T Object { get; set; }

        #endregion
    }
}