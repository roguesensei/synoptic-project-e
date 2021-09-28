using NUnit.Framework;
using SynopticProject_Project_E.Extensions;
using System;
using System.Text;

namespace SynopticProject_Project_E.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Test]
        public void ConvertToBase64AndBackTest()
        {
            // Arrange
            string message = "The Quick Brown Fox jumped over the Lazy Dog";

            // Act
            string messageToBase64 = message.ToBase64();
            string messageFromBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(messageToBase64));

            // Assert
            Assert.AreEqual(messageFromBase64, message);
        }
    }
}
