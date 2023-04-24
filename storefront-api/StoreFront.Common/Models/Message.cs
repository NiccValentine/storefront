

namespace StoreFront.Common.Models
{
    /// <summary>
    /// Allows for message collection within Service validators
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the field name of a message
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the message text
        /// </summary>
        public string MessageText { get; set; }
    }
}
