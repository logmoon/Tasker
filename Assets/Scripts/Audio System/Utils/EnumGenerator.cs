 #if UNITY_EDITOR
 using UnityEditor;
 using System.IO;
 using System;
 using System.Collections.Generic;

 public class EnumGenerator
 {
     public static void Generate(string enumName, List<String> enumEntries)
     {
         string filePath = "Assets/Scripts/Enums/";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
 
         using ( StreamWriter streamWriter = new StreamWriter( filePath + enumName + ".cs" ) )
         {
             streamWriter.WriteLine( "public enum " + enumName );
             streamWriter.WriteLine( "{" );
             for( int i = 0; i < enumEntries.Count; i++ )
             {
                 streamWriter.WriteLine( "\t" + enumEntries[i] + "," );
             }
             streamWriter.WriteLine( "}" );
         }
         AssetDatabase.Refresh();
     }
 }
 #endif
