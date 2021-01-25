using Barracuda;
using Xunit;

namespace IntegretationTests
{
    /*
        The core of the ReadBlob method in the ReadBlobService relied on external calls. Therefore it needed it's own
        integration tests to catch anything unit tests couldn't cover.
    */
    public class ReadBlobIntegrationTests
    {
        // Environment variables for these integration tests must be manually set here.
        private const string accountName = "cudacodingexercise";
        private const string containerName = "files";

        [Theory]
        [InlineData("cat.jpg", 97508)]
        [InlineData("dog.jpg", 476689)]
        public async void ReadBlobSucceeds(string fileName, int expectByteSize)
        {
            var readBlobServer = new ReadBlobServer.Services.ReadBlobService(accountName, containerName);

            var readBlobResponse = 
                await readBlobServer.ReadBlob(new ReadBlobRequest() { Key  = fileName }, null);

            var data = readBlobResponse.Data;
            
            // Byte size is unique for each jpg. This ensures data was returned and for the correct picture.
            Assert.Equal(expectByteSize, data.Length);
        }
    }
}