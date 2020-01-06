using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxcd.linqToSql.common
{
    /// <summary>
    /// Linq to SQL exception
    /// </summary>
    public class LinqToSqlException : Exception
    {
        /// <summary>
        /// Linq To Sql exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">exception</param>
        /// <param name="logger">ILogger</param>
        public LinqToSqlException(string message, Exception innerException = null, ILogger<Exception> logger = null) : base(message, innerException)
        {
            if (logger != null)
            {
                if (InnerException != null)
                {
                    logger.LogError(innerException, message);
                }
                else
                {
                    logger.LogError(message);
                }
            }
        }

    }
}
