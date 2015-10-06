using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Infra.Contracts
{
    public interface IServerResponse
    {
        /// <summary>
        /// Gets or sets the status indicating the response type.
        /// </summary>
        /// <value>
        /// <see cref="ServerResponseStatus"/>
        /// </value>
        ServerResponseStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title of the response
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message of the response.
        /// </value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the data object.
        /// </summary>
        /// <value>
        /// The responce data object for the client.
        /// </value>
        object Data { get; set; }
    }
}
