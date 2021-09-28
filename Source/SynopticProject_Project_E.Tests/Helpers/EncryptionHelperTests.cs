using NUnit.Framework;
using SynopticProject_Project_E.Helpers;

namespace SynopticProject_Project_E.Tests
{
    public class EncryptionHelperTests
    {
        private readonly string publicKey = "TEST PUBLIC";
        private readonly string privateKey = "TEST PRIVATE";

        [Test]
        public void EncryptThenDecryptTest()
        {
            // Arrange
            string message = "The Quick Brown Fox Jumped over the Lazy Dog";

            // Act
            string encryptedMessage = EncryptionHelper.Encrypt(message, publicKey, privateKey);
            string unencryptedMessage = EncryptionHelper.Decrypt(encryptedMessage, publicKey, privateKey);

            // Assert
            Assert.AreEqual(unencryptedMessage, message);
        }

        [Test]
        public void DecryptThenEncryptTest()
        {
            // Arrange
            string encryptedMessage = "TL1zLsbqkWqoagzZxSHg25zCQ9YyUqOv0r73hms+vzzCr/Lu4FTUzvIACtbUR36n";

            // Act
            string message = EncryptionHelper.Decrypt(encryptedMessage, publicKey, privateKey);
            string reEncryptedMessage = EncryptionHelper.Encrypt(message, publicKey, privateKey);

            // Assert
            Assert.AreEqual(reEncryptedMessage, encryptedMessage);
        }
    }
}