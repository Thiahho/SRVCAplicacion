﻿//using System;
//using System.Security;
//using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;
//using CredentialManagement;

//public static class CredentialManager
//{
//    // Guardar la credencial en el Administrador de Credenciales de Windows
//    public static void SaveCredential(string target, string username, string password)
//    {
//        var credential = new Credential()
//        {
//            Target = target,
//            Username = username,
//            Password = password,
//            Type = CredentialType.Generic
//        };

//        credential.Save();
//    }

//    // Obtener la credencial del Administrador de Credenciales de Windows
//    public static Credential GetCredential(string target)
//    {
//        return Credential.Load(target, CredentialType.Generic);
//    }
//}

//// Uso del código
//class Program
//{
//    static void Main(string[] args)
//    {
//        string target = "MyConnectionString"; // Identificador de la credencial
//        string username = "NombreUsuario"; // Usuario de la credencial (si es necesario)
//        string password = "TuPasswordSegura"; // La contraseña o cadena de conexión

//        // Guardar la credencial en el Administrador de Credenciales
//        CredentialManager.SaveCredential(target, username, password);

//        // Recuperar la credencial
//        var credential = CredentialManager.GetCredential(target);
//        Console.WriteLine($"Usuario: {credential.Username}");
//        Console.WriteLine($"Clave: {credential.Password}");
//    }
//}
