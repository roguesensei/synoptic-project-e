using Microsoft.AspNetCore.Mvc;

namespace SynopticProject_Project_E.Helpers
{
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
        HttpOk = 200,
        HttpBadRequest = 400,
        HttpNotFound = 404,
        HttpInternalServerError = 500
    }
}
