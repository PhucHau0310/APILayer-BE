using APILayer.Services.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.AccessControl;

namespace APILayer.Services.Implementations
{
    public class FirebaseService : IFirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly ILogger<FirebaseService> _logger;

        public FirebaseService(IConfiguration configuration, ILogger<FirebaseService> logger)
        {
            _logger = logger;

            try
            {
                // Get the Firebase section first
                var firebaseSection = configuration.GetSection("Firebase");
                if (!firebaseSection.Exists())
                {
                    _logger.LogError("Firebase configuration section not found");
                    throw new ArgumentException("Firebase configuration section not found");
                }

                var credentialsPath = firebaseSection.GetValue<string>("Credentials");
                if (string.IsNullOrEmpty(credentialsPath))
                {
                    _logger.LogError("Firebase credentials path is missing in configuration");
                    throw new ArgumentException("Firebase credentials path is missing in configuration.");
                }

                credentialsPath = Path.Combine(AppContext.BaseDirectory, credentialsPath);
                _logger.LogInformation("Loading Firebase credentials from: {Path}", credentialsPath);

                if (!File.Exists(credentialsPath))
                {
                    _logger.LogError("Firebase credentials file not found at: {Path}", credentialsPath);
                    throw new FileNotFoundException($"Firebase credentials file not found at path: {credentialsPath}");
                }

                var credential = GoogleCredential.FromFile(credentialsPath);
                _logger.LogInformation("Successfully loaded Firebase credentials");

                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = credential
                    });
                    _logger.LogInformation("Created new Firebase App instance");
                }

                _storageClient = StorageClient.Create(credential);
                _bucketName = configuration["Firebase:StorageBucket"];

                if (string.IsNullOrEmpty(_bucketName))
                {
                    _logger.LogError("Firebase storage bucket name is missing in configuration");
                    throw new ArgumentException("Firebase storage bucket name is missing in configuration");
                }

                _logger.LogInformation("Firebase service initialized successfully with bucket: {BucketName}", _bucketName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing Firebase service");
                throw;
            }
           
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            try
            {
                // Generate unique filename
                var fileName = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                // Upload to Firebase Storage
                var dataObject = await _storageClient.UploadObjectAsync(
                    _bucketName,
                    fileName,
                    file.ContentType,
                    memoryStream,
                    new UploadObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }); // Public Read

                // Return public URL
                return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception("File upload failed", ex);
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                _logger.LogInformation("Attempting to delete file: {FileUrl}", fileUrl);

                // Extract object name from URL
                var uri = new Uri(fileUrl);
                var objectName = uri.LocalPath.TrimStart('/');
                objectName = objectName.Substring(objectName.IndexOf('/') + 1);

                _logger.LogInformation("Extracted object name: {ObjectName}", objectName);

                // Delete from Firebase Storage  
                await _storageClient.DeleteObjectAsync(_bucketName, objectName);

                _logger.LogInformation("Successfully deleted file");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File deletion failed");
                throw new Exception("File deletion failed", ex);
            }
        }

        public async Task<string> GetFileUrlAsync(string path)
        {
            //var storage = storageclient.create();
            //var bucket = storage.getbucket("e-commerce-c9b1d.appspot.com");

            //tạo signed url có thời hạn
            //var signedurl = await storage.getsignedurlasync(
            //    bucket.name,
            //    path,
            //timespan.fromhours(1), // url có hiệu lực trong 1 giờ
            //    signurloptions.fromcredential(credential)
            //);

            //return signedurl;
            throw new Exception("mm");
        }
    }
}
