using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Plant_Test.UWP;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;

namespace Plant_Test.UWP
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string fileName)
        {
            //Localfolder should be a place where the app can write files
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string path = localFolder.Path;
            string dbPath = Path.Combine(path, fileName);

            //If this is the first run, then copy the database out of the app so we have something to work with.
            //If the database already exists, then this function does nothing
            //I should pass in the localFolder, but that is causing an error so going to manually keep these methods in sync
            CopyDatabaseIfNotExistsAsync(fileName);

            return dbPath;
        }

        //should this be static?
        private static async void CopyDatabaseIfNotExistsAsync(string fileName)
        {
            StorageFolder targetFolder = ApplicationData.Current.LocalFolder;

            //We have a couple checks to make to ensure that the file needs to be copied, so track that state in a bool
            bool needToCopy = false;

            //TEMP CODE -- Always delete the file if it's there so we can confirm that the initial copying works
            File.Delete(Path.Combine(targetFolder.Path, fileName));

            //check if the file exists, Null means it does 
            if (await targetFolder.TryGetItemAsync(fileName) != null)
            {
                System.Diagnostics.Debug.WriteLine("File does exist");
                
                //check the size to make sure it's non-zero
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(targetFolder.Path, fileName));
                Windows.Storage.FileProperties.BasicProperties props = await file.GetBasicPropertiesAsync();

                System.Diagnostics.Debug.WriteLine("File size is:{0}", props.Size);

                //If file is zero bytes, then an error probably occurred in the past, so we should stomp it
                if (props.Size == 0)
                {
                    needToCopy = true;
                }
             }
            else
            {
                System.Diagnostics.Debug.WriteLine("File does NOT exist");

                needToCopy = true;
            }

            if (needToCopy)
            {
                //commented out the try/catch to better see the exception
                //try
                //{
                System.Diagnostics.Debug.WriteLine("Trying to copy");

                //We are here, so need to copy the file
                
                //First, get the file out of the app package
                //ISSUE: For some reason, this is causing the targetFile to be created!!!
                Uri sourceURI = new Uri("ms-appx:///" + fileName, UriKind.Absolute);
                StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(sourceURI);

                    //Test code to confirm that we can do something with this
                    System.Diagnostics.Debug.WriteLine(sourceFile.DisplayName);
                    Windows.Storage.FileProperties.BasicProperties props = await sourceFile.GetBasicPropertiesAsync();
                    System.Diagnostics.Debug.WriteLine(props.Size);

                //Second, create the file we want to write to
                //ISSUE: This is going to fail because the file is already created and locked for write access
                StorageFile targetFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                //There are several ways I've found to do the copying. Another one is commented out below if this doesn't work
                using (Stream sourceStream = await sourceFile.OpenStreamForReadAsync())
                {
                    System.Diagnostics.Debug.WriteLine("File opened");

                    //Open the target file for writing
                    using (Stream targetStream = await targetFile.OpenStreamForWriteAsync())
                    {
                        //do the copying
                        System.Diagnostics.Debug.WriteLine("About to copy");
                        await sourceStream.CopyToAsync(targetStream);
                    }
                }


                    /*
                    byte[] buffer = new byte[1024];
                    using (BinaryWriter fileWriter = new BinaryWriter(await targetFile.OpenStreamForWriteAsync()))
                    {
                        using (BinaryReader fileReader = new BinaryReader(await sourceFile.OpenStreamForReadAsync()))
                        {
                            long readCount = 0;
                            while (readCount < fileReader.BaseStream.Length)
                            {
                                int read = fileReader.Read(buffer, 0, buffer.Length);
                                readCount += read;
                                fileWriter.Write(buffer, 0, read);
                            }
                        }
                    }


                    //First, get the original database
                    Uri sourceURI = new Uri(@"ms-appx:///" + fileName, UriKind.Absolute);
                        System.Diagnostics.Debug.WriteLine(sourceURI.ToString());

                        StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(sourceURI);

                        //Test code to confirm that we can do something with this
                        System.Diagnostics.Debug.WriteLine(sourceFile.DisplayName);
                        Windows.Storage.FileProperties.BasicProperties props = await sourceFile.GetBasicPropertiesAsync();
                        System.Diagnostics.Debug.WriteLine(props.Size);

                        //having to reset this because somehow it's getting broken with the Async stuff
                        targetFolder = ApplicationData.Current.LocalFolder;
                        //Try to create the destination file.
                        if (targetFolder != ApplicationData.Current.LocalFolder)
                        {
                            System.Diagnostics.Debug.WriteLine("Error, somehow target is NOT the same as the localfolder");
                        }

                        StorageFile targetFile = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                        System.Diagnostics.Debug.WriteLine("File name:{0}", targetFile.DisplayName);
                        */
                    /*//Open it for reading
                    using (Stream sourceStream = await sourceFile.OpenStreamForReadAsync())
                        {
                            System.Diagnostics.Debug.WriteLine("File opened");

                            //Open the target file for writing
                            using (Stream targetStream = await targetFile.OpenStreamForWriteAsync())
                            {
                                //do the copying
                                System.Diagnostics.Debug.WriteLine("About to copy");
                                await sourceStream.CopyToAsync(targetStream);
                            }*/
                //}



                /*
                catch
                {
                    //Something went wrong, need to put in some error handling eventually
                    ;
                }*/
            }

        }

    }

}