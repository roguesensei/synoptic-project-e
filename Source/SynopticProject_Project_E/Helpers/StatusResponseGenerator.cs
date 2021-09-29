using Microsoft.AspNetCore.Mvc;

namespace SynopticProject_Project_E.Helpers
{
    /// <summary>
    /// Class used to generate custom status responses
    /// </summary>
    public static class StatusResponseGenerator
    {
        /// <summary>
        /// Method used to generate custom status server responses
        /// </summary>
        /// <param name="statusCode">HTTP Status Code</param>
        /// <param name="message">Optional Message to return to the user</param>
        /// <returns>Json Response to be sent back to the client</returns>
        public static JsonResult Generate(HttpStatusResponse statusCode, string message = null)
        {
            // Use the standard StatusCodeResult if no message is provided
            if (string.IsNullOrEmpty(message))
            {
                return new JsonResult(new StatusCodeResult((int)statusCode));
            }

            return new JsonResult(new
            {
                statusCode = statusCode,
                message = message
            });
        }
    }

    /// <summary>
    /// Common Http Status Codes used in app
    /// </summary>
    public enum HttpStatusResponse
    {
        /// <summary>
        /// HTTP 200 - OK
        /// </summary>
        HttpOk = 200,
        /// <summary>
        /// HTTP 400 - Bad Request
        /// </summary>
        HttpBadRequest = 400,
        /// <summary>
        /// HTTP 401 - Unauthorized
        /// </summary>
        HttpUnauthorized = 401,
        /// <summary>
        /// HTTP 403 - Forbidden
        /// </summary>
        HttpForbidden = 403,
        /// <summary>
        /// HTTP 404 - Not Found
        /// </summary>
        HttpNotFound = 404,
        /// <summary>
        /// HTTP 419 - Authentication Timeout
        /// </summary>
        HttpAuthenticationTimeout = 419,
        /// <summary>
        /// Http 500 - Internal Server Error
        /// </summary>
        HttpInternalServerError = 500
    }
}
