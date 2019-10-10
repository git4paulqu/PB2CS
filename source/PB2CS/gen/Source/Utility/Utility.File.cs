using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace gen.Source
{
    public partial class Utility
    {
        public class File
        {
            public static string FormatPath(string path)
            {
                return path.Replace("\\", "/");
            }

            public static bool FileExists(string path)
            {
                return System.IO.File.Exists(path);
            }

            public static bool DirectoryExists(string path)
            {
                return System.IO.Directory.Exists(path);
            }

            public static void CreateFile(string path)
            {
                if (!FileExists(path))
                {
                    FileStream fs = System.IO.File.Create(path);
                    fs.Close();
                }
            }

            public static void DeleteFile(string path)
            {
                if (FileExists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    Console.WriteLine("File:[" + path + "] dont exists");
                }
            }

            public static void CreatFolder(string dir)
            {
                if (!DirectoryExists(dir))
                    Directory.CreateDirectory(dir);
            }

            public static void TryClearPath(string path)
            {
                DeleteDirectories(path, true);
                CreatFolder(path);
            }

            public static void DeleteDirectories(string path, bool rmPath = false)
            {
                if (!DirectoryExists(path))
                    return;

                if (rmPath)
                {
                    Directory.Delete(path, true);
                    return;
                }

                string[] directorys = Directory.GetDirectories(path);

                //删掉所有子目录
                for (int i = 0; i < directorys.Length; i++)
                {
                    string pathTmp = directorys[i];

                    if (DirectoryExists(pathTmp))
                    {
                        Directory.Delete(pathTmp, true);
                    }
                }

                //删掉所有子文件
                string[] files = Directory.GetFiles(path);

                for (int i = 0; i < files.Length; i++)
                {
                    string pathTmp = files[i];
                    if (FileExists(pathTmp))
                    {
                        DeleteFile(pathTmp);
                    }
                }
            }

            public static void CopyDirectories(string sourcePath, string destinationPath)
            {
                DirectoryInfo info = new DirectoryInfo(sourcePath);
                Directory.CreateDirectory(destinationPath);

                foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
                {
                    string destName = Path.Combine(destinationPath, fsi.Name);

                    if (fsi is FileInfo)
                    {
                        CopyFile(fsi.FullName, destName);
                    }
                    else
                    {
                        Directory.CreateDirectory(destName);
                        CopyDirectories(fsi.FullName, destName);
                    }
                }
            }

            public static void CopyFile(string src, string dest)
            {
                System.IO.File.Copy(src, dest, true);
            }

            public static string RemoveExpandName(string name)
            {
                int dirIndex = name.LastIndexOf(".");

                if (dirIndex != -1)
                {
                    return name.Remove(dirIndex);
                }
                else
                {
                    return name;
                }
            }

            public static string GetExpandName(string name)
            {
                return name.Substring(name.LastIndexOf(".") + 1, (name.Length - name.LastIndexOf(".") - 1));
            }

            public static string GetFileNameByPath(string path)
            {
                FileInfo fi = new FileInfo(path);
                return fi.Name; // text.txt
            }

            public static string GetFilePath(string path)
            {
                FileInfo fi = new FileInfo(path);
                return fi.DirectoryName;
            }

            public static string GetFileNameByString(string path)
            {
                path = FormatPath(path);
                string[] paths = path.Split('/');
                return paths[paths.Length - 1];
            }

            public static void RecursionFileExecute(string path, string expandName, FileExecuteHandle handle)
            {
                string[] allUIPrefabName = Directory.GetFiles(path);
                foreach (var item in allUIPrefabName)
                {
                    try
                    {
                        if (expandName != null)
                        {
                            if (item.EndsWith("." + expandName))
                            {
                                handle(item);
                            }
                        }
                        else
                        {
                            handle(item);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(("RecursionFileExecute Error :" + item + " Exception:" + e.ToString()));
                        throw e;
                    }
                }

                string[] dires = Directory.GetDirectories(path);
                for (int i = 0; i < dires.Length; i++)
                {
                    RecursionFileExecute(dires[i], expandName, handle);
                }
            }

            public static List<string> GetAllFileNamesByPath(string path, string[] expandNames = null)
            {
                List<string> list = new List<string>();

                RecursionFind(list, path, expandNames);

                return list;
            }

            static void RecursionFind(List<string> list, string path, string[] expandNames)
            {
                string[] allUIPrefabName = Directory.GetFiles(path);
                foreach (var item in allUIPrefabName)
                {
                    if (ExpandFilter(item, expandNames))
                    {
                        list.Add(item);
                    }
                }

                string[] dires = Directory.GetDirectories(path);
                for (int i = 0; i < dires.Length; i++)
                {
                    RecursionFind(list, dires[i], expandNames);
                }
            }

            static bool ExpandFilter(string name, string[] expandNames)
            {
                if (expandNames == null)
                {
                    return true;
                }

                else if (expandNames.Length == 0)
                {
                    return !name.Contains(".");
                }

                else
                {
                    for (int i = 0; i < expandNames.Length; i++)
                    {
                        if (name.EndsWith("." + expandNames[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            public static void ConvertFileEncoding(string sourceFile, string destFile, System.Text.Encoding targetEncoding)
            {
                destFile = string.IsNullOrEmpty(destFile) ? sourceFile : destFile;
                Encoding sourEncoding = GetEncodingType(sourceFile);

                System.IO.File.WriteAllText(destFile, System.IO.File.ReadAllText(sourceFile, sourEncoding), targetEncoding);
            }

            public static Encoding GetEncodingType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetEncodingType(fs);
                fs.Close();
                return r;
            }

            public static Encoding GetEncodingType(FileStream fs)
            {
                //byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                //byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                //byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1;
                //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX......1111110X　 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }

            public static string ReadStringByFile(string path, Encoding encoding = null)
            {
                encoding = encoding == null ? Encoding.UTF8 : encoding;
                StringBuilder line = new StringBuilder();
                try
                {
                    if (!FileExists(path))
                    {
                        Console.WriteLine("path dont exists ! : " + path);
                        return "";
                    }

                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, encoding);
                    line.Append(sr.ReadToEnd());

                    sr.Close();
                    sr.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Load text fail ! message:" + e.Message);
                }

                return line.ToString();
            }

            public static byte[] ReadBytesByFile(string path, Encoding encoding = null)
            {
                encoding = encoding == null ? Encoding.UTF8 : encoding;
                byte[] result = null;
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    BinaryReader br = new BinaryReader(fs, encoding);
                    result = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                }
                return result;
            }

            public static void WriteBytes2File(string fileName, byte[] bs)
            {
                if (!FileExists(fileName))
                    CreateFile(fileName);
                System.IO.File.WriteAllBytes(fileName, bs);
            }

            public static void WriteString2File(string fileName, string content)
            {
                WriteLock.EnterWriteLock();
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                WriteLock.ExitWriteLock();
            }

            static ReaderWriterLockSlim WriteLock = new ReaderWriterLockSlim();
        }
    }

    public delegate void FileExecuteHandle(string filePath);
}
