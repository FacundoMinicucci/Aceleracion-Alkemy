using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Amazon;
using OngProject.Core.Helper;
using System;
using OngProject.Core.Interfaces;
using System.Collections.Generic;

namespace OngProject.Core.Services
{
    public class AWSHandlerFilesService : IAWSService
    {
        private  IAmazonS3 _IAmazonS3;
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly RegionEndpoint region;

        public AWSHandlerFilesService(IConfiguration configuration)
        {
             region = RegionEndpoint.SAEast1;
             accessKey = configuration["AWS:AccessKey"];
             secretKey = configuration["AWS:SecretKey"];

            //_IAmazonS3 = new AmazonS3Client(accessKey, secretKey, region);
        }

        public async Task<AWSManagerResponse> UploadImage(IFormFile file)
        {
            try
            {
                using (_IAmazonS3 = new AmazonS3Client(accessKey, secretKey, region))
                {
                    var putRequest = new PutObjectRequest()
                    {
                        BucketName = "alkemy-ong",
                        Key = file.FileName,
                        InputStream = file.OpenReadStream(),
                        ContentType = file.ContentType
                    };

                    var result = await _IAmazonS3.PutObjectAsync(putRequest);

                    var response = new AWSManagerResponse
                    {
                        Message = "Image successfully uploaded",
                        Code = (int?)result.HttpStatusCode,
                        NameImage = file.FileName,
                        URL = $"https://alkemy-ong.s3.sa-east-1.amazonaws.com/{file.FileName}"
                    };

                    return response;
                }
            }

            catch (AmazonS3Exception e)
            {
                return new AWSManagerResponse
                {
                    Message = "Oops! an error was found",
                    Code = (int?)e.StatusCode,
                    Errors = e.Message
                };
            }

            catch (Exception e)
            {
                return new AWSManagerResponse
                {
                    Message = "Oops! an error has been encountered on the server.",
                    Code = 500,
                    Errors = e.Message
                };
            }
        }

        public async Task<AWSManagerResponse> DeleteImage(string urlKey)
        {
            try
            {
               
                using (_IAmazonS3 = new AmazonS3Client(accessKey, secretKey, region))
                {
                    var deleteObjectRequest = new DeleteObjectRequest()
                    {
                        BucketName = "alkemy-ong",
                        Key = urlKey
                    };

                    var result = await _IAmazonS3.DeleteObjectAsync(deleteObjectRequest);
                                        
                   


                    var response = new AWSManagerResponse
                    {
                        Message = "Image successfully deleted",
                        Code = (int?)result.HttpStatusCode,
                        NameImage = urlKey
                    };

                    return response;
                }
            }
            catch (AmazonS3Exception e)
            {
                return new AWSManagerResponse
                {
                    Message = "Oops! an error was found",
                    Code = (int?)e.StatusCode,
                    Errors = e.Message
                };
            }
            catch (Exception e)
            {
                return new AWSManagerResponse
                {
                    Message = "Oops! an error has been encountered on the server.",
                    Code = 500,
                    Errors = e.Message
                };
            }
        }

        public void Dispose()
        {
            if (_IAmazonS3 != null)
            {
                _IAmazonS3.Dispose();
            }
        }
    }
}
