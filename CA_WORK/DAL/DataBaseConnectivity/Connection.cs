using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Connection
{
    private static string dataSource;
    private static string userName;
    private static string password;

    public static string DataSource
    {
        get
        {
            return Connection.dataSource;
        }
        set
        {
            Connection.dataSource = value;
        }
    }

    public static string UserName
    {
        get
        {
            return Connection.userName;
        }
        set
        {
            Connection.userName = value;
        }
    }

    public static string Password
    {
        get
        {
            return Connection.password;
        }
        set
        {
            Connection.password = value;
        }
    }

    public static String getConnectionString()
    {
        return string.Format("DATA SOURCE={0};USER ID={1};PASSWORD={2}", DataSource, UserName, Password);
    }

    public static void loadCredentials()
    {
        string[] arrCredentials;

        var objFileStream = new FileStream("C:/EXE" + "/exe.bat", FileMode.Open);
        var objStrReader = new StreamReader(objFileStream);

        var strWord = string.Empty;
        while (!objStrReader.EndOfStream)
        {
            strWord = objStrReader.ReadLine();
        }
        objFileStream.Close();
        objStrReader.Close();

        arrCredentials = strWord.Split('/');

        Connection.UserName = arrCredentials[0];
        Connection.Password = arrCredentials[1];
        Connection.DataSource = arrCredentials[2];

        //Connection.UserName = "0002014";
        //Connection.Password = "0002014";
        //Connection.DataSource = "prod19c";
    }
}

