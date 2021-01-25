using System;
using System.IO;
using System.Text;
using Azure.Storage.Blobs.Specialized;
using Moq;
using ReadBlobServer.Services;
using Xunit;

namespace UnitTests
{
    public class ReadBlobUnitTests
    {
        [Fact]
        public void GetBlockStreamNullBlockClientThrows()
        {
            Assert.Throws<Exception>(() => ReadBlobService.GetBlockStream(null));
        }

        [Fact]
        public void GetBlockStreamSucceeds()
        {
            var moqBlockClient = new Mock<BlockBlobClient>();

            ReadBlobService.GetBlockStream(moqBlockClient.Object);
        }
        
        [Fact]
        public void CreateByteArrayNullStreamThrows()
        {
            Assert.Throws<Exception>(() => ReadBlobService.CreateByteArray(null));
        }
        
        [Fact]
        public void CreateByteArrayLength0StreamThrows()
        {
            var testStream = new MemoryStream();

            Assert.Throws<Exception>(() => ReadBlobService.CreateByteArray(testStream));
        }
        
        [Fact]
        public void CreateByteArraySucceeds()
        {
            var testStream = new MemoryStream(Encoding.UTF8.GetBytes("test"));

            var testByteArr = ReadBlobService.CreateByteArray(testStream);
            
            Assert.Equal(4, testByteArr.Length);
        }
    }
}