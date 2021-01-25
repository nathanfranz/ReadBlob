using System;
using Barracuda;
using Grpc.Net.Client;
using Xunit;

namespace FunctionalTest
{
    public class ClientFunctionalTest
    {
        private const string Address = "http://localhost:5001";

        private const string FileName = "cat.jpg";
        
        private const int ExpectByteSize = 97508;
        
        [Fact]
        public void ClientReadsFileSucceeds()
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2Support", true);
            
            var channel = GrpcChannel.ForAddress(Address);
            var client =  new BlobProxy.BlobProxyClient(channel);

            var response = client.ReadBlob(new ReadBlobRequest() { Key = FileName });
            
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            
            // Byte size is unique for each jpg. This ensures data was returned and for the correct file.
            Assert.Equal(ExpectByteSize, response.Data.Length);
        }
    }
}