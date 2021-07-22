using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


using Debug = UnityEngine.Debug;

public static class FileManager
{
    #region Directories
    /// <summary>
    /// Clear the folder of all files and folders.
    /// </summary>
    public static void ClearFolder(string sFolderName, params string[] sFilter)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(sFolderName);
        FileInfo[] files = dirInfo.GetFiles();
        bool bDelete = true; // Default if no filter, delete straight away.

        //filter files you want to keep
        int iFileCount = files.Length;
        for (int iFile = 0; iFile < iFileCount; ++iFile)
        {
            //check each file against each string in the filter.
            int iFilterLength = sFilter.Length;
            for (int iFilterCount = 0; iFilterCount < iFilterLength; ++iFilterCount)
            {
                if (false == files[iFile].FullName.Contains(sFilter[iFilterCount]))
                {
                    bDelete = true;
                }
                else
                {
                    bDelete = false;
                    break;
                }
            }

            //if it's true at the end of check, delete file
            if (true == bDelete)
            {
                try
                {
                    files[iFile].Delete(); // sometimes this will fail, why, we should find out why. Assumption: files are active somehow? 'AudioPluginOculusSpatializer.dll' is denied.
                }
                catch
                {
                    UnityEngine.Debug.Log("Force delete + " + files[iFile].FullName);
                    DeleteViaCmd(files[iFile].FullName);
                }
            }
        }

        DirectoryInfo[] directories = dirInfo.GetDirectories();
        int iDirLength = directories.Length;
        for (int iDir = 0; iDir < iDirLength; ++iDir)
        {
            ClearFolder(directories[iDir].FullName, sFilter);

            //find if their are any files left in the directory
            FileInfo[] emptyfiles = directories[iDir].GetFiles();

            //if not delete directory
            if (0 == emptyfiles.Length)
            {
                try
                {
                    directories[iDir].Delete(); // sometimes this will fail 
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log("Force delete + " + directories[iDir].FullName);
                    DeleteViaCmd(directories[iDir].FullName);
                }
            }
        }
    }

    private static void DeleteViaCmd(string sFolderFileName)
    {
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.StandardInput.WriteLine("del /f \"" + sFolderFileName + "\"   ");
        process.StandardInput.Flush();
        process.StandardInput.Close();
    }

    /// <summary>
    /// Copy directory.
    /// </summary>
    public static void CopyDirectory(string sSource, string sDestination)
    {
        if (false == Directory.Exists(sDestination))
        {
            Directory.CreateDirectory(sDestination);
        }

        DirectoryInfo dirInfo = new DirectoryInfo(sSource);
        FileInfo[] files = dirInfo.GetFiles();

        int iFilesLenght = files.Length;
        for (int iFileIndex = 0; iFileIndex < iFilesLenght; ++iFileIndex)
        {
            files[iFileIndex].CopyTo(Path.Combine(sDestination, files[iFileIndex].Name));
        }

        DirectoryInfo[] directorys = dirInfo.GetDirectories();
        int iDirLenght = directorys.Length;
        for (int iDirIndex = 0; iDirIndex < iDirLenght; ++iDirIndex)
        {
            CopyDirectory(Path.Combine(sSource, directorys[iDirIndex].Name), Path.Combine(sDestination, directorys[iDirIndex].Name));
        }
    }

    public static IEnumerable<string> GetFiles(string root, string searchPattern)
    {
        Stack<string> pending = new Stack<string>();
        pending.Push(root);
        int i = 0;
        while (pending.Count != 0)
        {
            string path = pending.Pop();
            string[] next = null;

            try
            {
                // get files in current directory
                next = Directory.GetFiles(path, searchPattern);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                i++;
            }

            // return each file found
            if (next != null && next.Length != 0)
            {
                foreach (string file in next)
                {
                    yield return file;
                }
            }

            try
            {
                // get sub-directories and add to pending
                next = Directory.GetDirectories(path);
                foreach (string subdir in next)
                {
                    pending.Push(subdir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public static bool CheckFolderLocation(string sDir)
    {
        if (true == Directory.Exists(sDir))
        {
            return true;
        }
        else
        {
            Directory.CreateDirectory(sDir);
            return CheckFolderLocation(sDir);
        }
    }

    public static bool CheckFileExist(string sFullPath)
    {
        string sDrive = Path.GetPathRoot(sFullPath);
        string sFolder = Path.GetDirectoryName(sFullPath);
        return (true == Directory.Exists(sDrive)) &&
                (true == Directory.Exists(sFolder)) &&
                (true == File.Exists(sFullPath));
    }

    public static bool DeleteFile(string sFilename)
    {
        if (true == CheckFileExist(sFilename))
        {
            File.Delete(sFilename);
            return true;
        }
        return false;
    }
    #endregion




    #region XML Loading and Saving of classes
    /// <summary>
    /// Loading a class from XML
    /// </summary>
    public static bool LoadClassFromXML<T>(ref T data, string sFullFilePath) where T : class
    {
        try
        {
            if (false == CheckFileExist(sFullFilePath))
            {
                return false;
            }

            //Deserialise file
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(sFullFilePath, FileMode.Open);
            data = serializer.Deserialize(stream) as T;
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + data.ToString() + e.Message);
            return false;
        }

        Debug.Log("Deserialised Data as type " + data.ToString());
        return true;
    }

    /// <summary>
    /// Saving a class to XML
    /// </summary>
    public static bool SaveClassToXML<T>(T data, string sDirectory) where T : class
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(sDirectory, FileMode.Create);
            TextWriter s = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(s, data);
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Saving Error - " + e.ToString());
            return false;
        }

        Debug.Log("XML Data saved!");
        return true;
    }
    #endregion




    #region JSON Save to file

    /// <summary>
    /// Load a class from JSON
    /// </summary>
    public static bool LoadClassFromJSON<T>(ref T type, string sFullFilePath) where T : class
    {
        try
        {
            if (false == CheckFileExist(sFullFilePath))
            {
                return false;
            }

            //Deserialise file
            string sJSON = string.Empty;
            using (StreamReader reader = new StreamReader(sFullFilePath))
            {
                sJSON = reader.ReadToEnd();
            }

            // step 2: parse the JSON data
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(sJSON);

            // step 3: deserialize the data
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccess();

            //convert to type
            type = (T)deserialized;
        }
        catch (Exception e)
        {
            if (null != type)
            {
                Debug.LogError("Failed to load : " + type.ToString() + e.Message);
            }
            else
            {
                Debug.LogError("Failed to load  (type is null): " + e.Message);
            }
            return false;
        }

        //Debug.Log("Deserialised Data as type " + type.ToString());
        return true;
    }

    /// <summary>
    /// Load a struct from JSON
    /// </summary>
    public static bool LoadStructFromJSON<T>(ref T type, string sFullFilePath) where T : struct
    {
        try
        {
            if (false == CheckFileExist(sFullFilePath))
            {
                return false;
            }

            //Deserialise file
            string sJSON = string.Empty;
            using (StreamReader reader = new StreamReader(sFullFilePath))
            {
                sJSON = reader.ReadToEnd();
            }

            // step 2: parse the JSON data
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(sJSON);

            // step 3: deserialize the data
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

            //convert to type
            type = (T)deserialized;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Failed to load : " + type.ToString() + e.Message);
            return false;
        }

        //Debug.Log("Deserialised Data as type " + type.ToString());
        return true;
    }


    /// <summary>
    /// Loads the JSON files from resources.
    /// </summary>
    public static bool LoadJSONFromResourcesToClass<T>(ref T dataType, string sFile) where T : class
    {
        try
        {
            TextAsset textAsset = null;
            if (true == LocalUtils.LoadResource<TextAsset>(ref textAsset, sFile))
            {
                // step 2: parse the JSON data
                fsSerializer serializer = new fsSerializer();
                fsData data = fsJsonParser.Parse(textAsset.text);

                // step 3: deserialize the data
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

                //convert to type
                dataType = (T)deserialized;
            }
            else
            {
                Debug.LogError("Failed to load : " + dataType.ToString());
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + dataType.ToString() + e.Message);
            return false;
        }

        Debug.Log("Deserialised Data as type " + dataType.ToString());
        return true;
    }

    /// <summary>
    /// Loads the JSON files from resources.
    /// </summary>
    public static bool LoadJSONFromResourcesToStruct<T>(ref T dataType, string sFile) where T : struct
    {
        try
        {
            TextAsset textAsset = null;
            if (true == LocalUtils.LoadResource<TextAsset>(ref textAsset, sFile))
            {
                // step 2: parse the JSON data
                fsSerializer serializer = new fsSerializer();
                fsData data = fsJsonParser.Parse(textAsset.text);

                // step 3: deserialize the data
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

                //convert to type
                dataType = (T)deserialized;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + dataType.ToString() + e.Message);
            return false;
        }

        Debug.Log("Deserialised Data as type " + dataType.ToString());
        return true;
    }

    /// <summary>
    /// Saving a class to JSON
    /// </summary>
    public static bool SaveClassToJSON<T>(ref T value, string sPath) where T : class
    {
        try
        {
            //Check if folder exists, if not create
            CheckFolderLocation(Path.GetDirectoryName(sPath));

            // serialize the data
            fsData data;
            fsSerializer serializer = new fsSerializer();

            serializer.TrySerialize(typeof(T), value, out data).AssertSuccessWithoutWarnings();

            using (StreamWriter writer = new StreamWriter(sPath))
            {
                // emit the data via JSON
                writer.Write(fsJsonPrinter.PrettyJson(data));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + value.ToString() + e.Message);
            return false;
        }

        //Debug.Log("Data saved!");
        return true;
    }

    /// <summary>
    /// Saving a struct to JSON
    /// </summary>
    public static bool SaveStructToJSON<T>(ref T value, string sPath) where T : struct
    {
        try
        {
            //Check if folder exists, if not create
            CheckFolderLocation(Path.GetDirectoryName(sPath));

            // serialize the data
            fsData data;
            fsSerializer serializer = new fsSerializer();

            serializer.TrySerialize(typeof(T), value, out data).AssertSuccessWithoutWarnings();

            using (StreamWriter writer = new StreamWriter(sPath))
            {
                // emit the data via JSON
                writer.Write(fsJsonPrinter.CompressedJson(data));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + value.ToString() + e.Message);
            return false;
        }

        Debug.Log("Data saved!");
        return true;
    }
    #endregion



    #region JSON String Operations
    public static bool LoadJsonStringFromFile<T>(ref string sJsonString, string sFullFilePath) where T : class
    {
        try
        {
            if (false == CheckFileExist(sFullFilePath))
            {
                return false;
            }

            //Deserialise file
            using (StreamReader reader = new StreamReader(sFullFilePath))
            {
                sJsonString = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + sFullFilePath + e.Message);
            return false;
        }

        Debug.Log("Deserialised Data as type " + sFullFilePath);
        return true;
    }

    public static bool LoadJsonStringFromString<T>(ref T type, string sJSON) where T : class
    {
        try
        {
            // step 2: parse the JSON data
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(sJSON);

            // step 3: deserialize the data
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

            //convert to type
            type = (T)deserialized;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + e.Message);
            return false;
        }

        return true;
    }

    public static bool SaveClassToJSONString<T>(ref T value, out string sJson) where T : class
    {
        sJson = "";

        try
        {
            // serialize the data
            fsData data;
            fsSerializer serializer = new fsSerializer();

            serializer.TrySerialize(typeof(T), value, out data).AssertSuccessWithoutWarnings();

            // emit the data via JSON
            sJson = fsJsonPrinter.PrettyJson(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load : " + value.ToString() + e.Message);
            return false;
        }

        return true;
    }
    #endregion



}
