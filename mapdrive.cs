using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Text.RegularExpressions;
namespace mapdrive
{
    class MainClass
    {      
		public static void Main(string[] args)
		{
			String user = "DOMAIN\\username";
            String path = "\\\\server.com\\folder";
            String passwd = "123456";
            String driveletter = "T:";
			System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\mapdrive\mapdrivers.txt", false);
			bool retorno = mapDrive(file, user, path, passwd, driveletter);
			checkEnvironment(file);
			file.Close();
			
		}
		public static bool mapDrive(System.IO.StreamWriter file, String user, String path, String passwd, String driveletter)
        {
			bool drivemapped = false;
			Thread.Sleep(5000);
			try
			{
				String[] unidades = Environment.GetLogicalDrives();
                foreach (string unidade in unidades)
                {
					if (Regex.Split(unidade,":")[0].ToLower() == Regex.Split(driveletter,":")[0].ToLower())
					{
						Console.WriteLine("Driver " + driveletter + " found.");
						file.WriteLine("Driver " + driveletter + " found.");
						drivemapped = true;
					}
					else
					{
						Console.WriteLine("Driver " + driveletter + " Not found. What I found was " + unidade);
						file.WriteLine("Driver " + driveletter + " Not found. What I found was " + unidade);
					}
                }
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not determine if the drive is mapped.");
				file.WriteLine("Could not determine if the drive is mapped.");
				Console.WriteLine(e);
				file.WriteLine("Erro " + e);
			}
			if (drivemapped == false)
			{
				try
				{
					DoProcess("net", "use /D " + driveletter);
					DoProcess("net", @"use " + driveletter + " " + path + " /u:" + user + " " + passwd);
				}
				catch (Exception e)
				{
					Console.WriteLine("Could not map driver");
					file.WriteLine("Could not map driver");
					Console.WriteLine(e);
					file.WriteLine("Erro " + e);
				}
			}
			else
			{
				Console.WriteLine("Driver is allready mapped.");
				file.WriteLine("Driver is allready mapped.");
			}
			return drivemapped;
        }
		public static string DoProcess(string cmd, string argv)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = cmd;
            p.StartInfo.Arguments = $" {argv}";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();         
            return output;         
        }
		public static void checkEnvironment(System.IO.StreamWriter file)
        {
			foreach (var drive in System.IO.DriveInfo.GetDrives())
            {

                try
                {
                    double freeSpace = drive.TotalFreeSpace;
                    double totalSpace = drive.TotalSize;
                    double percentFree = (freeSpace / totalSpace) * 100;
                    float num = (float)percentFree;

                    file.WriteLine(DateTime.Now);
                    file.WriteLine(drive.Name);
                    file.WriteLine(Environment.UserName);
                    file.WriteLine("Drive:{0} With {1} % free", drive.Name, num);
                    file.WriteLine("Space Remaining:{0}", drive.AvailableFreeSpace);
                    file.WriteLine("Percent Free Space:{0}", percentFree);
                    file.WriteLine("Space used:{0}", drive.TotalSize);
                    file.WriteLine("Type: {0}", drive.DriveType);

                    Console.WriteLine(Environment.UserName);
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine(drive.Name);
                    Console.WriteLine("Drive:{0} With {1} % free", drive.Name, num);
                    Console.WriteLine("Space Remaining:{0}", drive.AvailableFreeSpace);
                    Console.WriteLine("Percent Free Space:{0}", percentFree);
                    Console.WriteLine("Space used:{0}", drive.TotalSize);
                    Console.WriteLine("Type: {0}", drive.DriveType);
                    try
                    {
                        string[] filePaths = System.IO.Directory.GetFiles(@drive.Name);
                        foreach (string arquivo in filePaths)
                        {
							Console.WriteLine("Arquivo: " + arquivo);
							file.WriteLine("Arquivo: " + arquivo);
                        }
                        string[] dirpaths = System.IO.Directory.GetDirectories(@drive.Name);
                        foreach (string diretorio in dirpaths)
                        {
							Console.WriteLine("Diretório: " + diretorio);
							file.WriteLine("Diretório: " + diretorio);
                        }
                    }
                    catch (System.ArgumentException e)
                    {
						Console.WriteLine("erro " + e);
						file.WriteLine("erro " + e);
                    }


                }
                catch (System.IO.IOException e)
                {
					Console.WriteLine("Erro " + e);
					file.WriteLine("Erro " + e);
                }


            }      
        }
    }
}
