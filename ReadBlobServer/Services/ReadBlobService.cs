using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Specialized;
using Barracuda;
using Google.Protobuf;
using Grpc.Core;

namespace ReadBlobServer.Services
{
    public class ReadBlobService : BlobProxy.BlobProxyBase
    {
        private readonly string accountName;
        private readonly string containerName;
        
        /*
            There was an issue with using environment variables in integration tests. Hacked this constructor so
            environment variables can be passed as parameters in the integration tests. This could be revisited, but for 
            now it only affects integration tests, so I didn't consider it a big issue. 
        */
        public ReadBlobService(string accEnv = null, string conEnv = null)
        {
            accountName = Environment.GetEnvironmentVariable("BLOB_ACCOUNT");
            containerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER");

            accountName ??= accEnv;
            containerName ??= conEnv;
        }

        public override Task<ReadBlobResponse> ReadBlob(ReadBlobRequest request, ServerCallContext context)
        {
            var blobClient = new BlockBlobClient
                (new Uri($"https://{accountName}.blob.core.windows.net/{containerName}/{request.Key}"));

            var blockStream = GetBlockStream(blobClient);

            var byteArr = CreateByteArray(blockStream);

            var byteStr = ByteString.CopyFrom(byteArr);

            return Task.FromResult(new ReadBlobResponse() { Data = byteStr });
        }
        
        public static Stream GetBlockStream(BlockBlobClient blockClient)
        {
            if (blockClient == null)
            {
                throw new Exception("There was an error creating the blob client.");
            }
            
            Stream blockStream;
            try
            {
                blockStream = blockClient.OpenRead();
            }
            catch
            {
                throw new Exception("There was an error retrieving requested blob.");
            }

            return blockStream;
        }

        public static byte[] CreateByteArray(Stream blockStream)
        {
            if (blockStream == null || blockStream.Length == 0)
            {
                throw new Exception("No blob data was found.");
            }
            
            using var memoryStream = new MemoryStream();
            blockStream.CopyTo(memoryStream);
            var byteArr = memoryStream.ToArray();

            return byteArr;
        }
    }
}