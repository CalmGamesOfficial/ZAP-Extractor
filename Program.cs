//ZAP Extractor (Version 1.2) by Calm Games for more information https://github.com/CalmGamesOfficial/ZAP-Extractor
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace ZAP_Converter
{
    class Program
    {
        //SO Info
        public static bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
         
        //Variables Estaticas
        public static string file;
        public static string filePath;
        public static string outputPath;

        public static string fileName;

        public static DateTime startTime;

        static void Main(string[] args)
        {
            Strings strings = new Strings();

            strings.ClearConsole();
            
            //Preguntar archivo de entrada
            Console.WriteLine("Introduzca la ruta de el archivo a convertir (Ejemplo: C:\\Users\\...\\Archivo.ZAP):");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            filePath = Console.ReadLine();
            Console.WriteLine("\n");
            Console.ResetColor();
            
            //Si no existe se inicia un bucle hasta que encuentre un archivo
            while (!File.Exists(filePath))
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No se ha encontrado el archivo especificado, por favor vuelva a introducirla:");
                Console.ForegroundColor = ConsoleColor.Green;
                filePath = Console.ReadLine();
                Console.WriteLine("\n");
            }

            //Una vez encontrado se guarda su nombre para su posterior uso
            string[] fileDir;
            if (isWindows)
            {
                fileDir = filePath.Split('\\');
                fileName = fileDir[fileDir.Length - 1];
                fileName = fileName.Split('.')[0];
            }
            else{
                fileDir = filePath.Split('/');
                fileName = fileDir[fileDir.Length - 1];
                fileName = fileName.Split('.')[0];
            }
            
            //Cargar archivo
            try
            {
                file = File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("IO Error: {0}", e.GetType().Name);
                Console.ResetColor();
                Console.WriteLine("Pulse Intro para salir...");
                Console.ReadKey();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Archivo cargado correctamente\n\n");
            Console.Title = "ZAP CONVERTER - " + fileName;
            Console.ResetColor();

            //Preguntar ruta de salida
            Console.ResetColor();
            Console.WriteLine("Introduzca la ruta de el archivo a exportar (Ejemplo: C:\\Users\\...\\):");
            Console.ForegroundColor = ConsoleColor.Green;
            outputPath = Console.ReadLine();

            //Si no existe see inicia un bucle hasta que encuentre una directorio de salida
            while (!Directory.Exists(outputPath))
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No se ha encontrado la ruta especificada, por favor vuelva a introducirla:");
                Console.ForegroundColor = ConsoleColor.Green;
                outputPath = Console.ReadLine();
            }

            //Preguntar campo a mostrar
            Console.ResetColor();
            Console.WriteLine("\nIntroduzca el campo que quiere que se muestre (usa , para introducir varios): ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            string field = Console.ReadLine();
            Console.ResetColor();

            //se inicia un bucle hasta que introduzca
            while (file == string.Empty)
            {
                Console.ResetColor();
                Console.WriteLine("\nIntroduzca el campo que quiere que se muestre (usa , para introducir varios): ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                field = Console.ReadLine();
                Console.WriteLine("\n");
                Console.ResetColor();
            }

            //Confirmar si el usuario quiere convertir el archivo
            Console.ResetColor();
            Console.Write("\nEstas seguro que deseas continuar? ");
            
            //Mostrar colores en Si/No
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Si");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("/");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("No\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            string userKey = Console.ReadLine();
            Console.ResetColor();

            if (userKey == "N" || userKey == "n" || userKey == "No" || userKey == "NO")
            {
                //En caso de lo contrario se cancela la conversion y se cierra el programa
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("la conversion ha sido cancelada");
                Console.ResetColor();
                return;
            }
            if (userKey == "S" || userKey == "s" || userKey == "Si" || userKey == "SI")
            {
                //Guardar en memoria la hora en la que empieza a convertir el archivo
                startTime = DateTime.Now;

                string data = strings.CopyDataToArray(file, field);

                //Crear csv
                using (FileStream fileStream = File.Create(outputPath + fileName + ".csv"))
                {
                    Console.WriteLine("\ncsv creado correctamente");

                    strings.ClearCurrentConsoleLine();
                    Console.WriteLine("Copiando informacion al csv...");

                    byte[] info = new UTF8Encoding(true).GetBytes(data);

                    fileStream.Write(info, 0, info.Length);
                }
                strings.ClearCurrentConsoleLine();
                Console.WriteLine("Informacion copiada correctamente\n");
            }

            TimeSpan time = DateTime.Now.Subtract(startTime);

            strings.ClearConsole();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nConversion completa en " + time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00") + ", csv guardado en '" + outputPath + "'\n");
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("DAT Extractor (Version 1.2) by Calm Games for more information: ");
            Console.ForegroundColor =ConsoleColor.Cyan;
            Console.Write("https://github.com/CalmGamesOfficial\n\n");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Pulse Intro para salir...");
            Console.ReadKey();
            Console.ResetColor();
        }

    }

    class Strings
    {
        //Console methods
        public void ClearConsole() {Console.Clear(); Title();}
        public void Title() {
            Console.Title = "ZAP EXTRACTOR";
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" ZAP EXTRACTOR ".PadRight(Console.BufferWidth));
            Console.ResetColor();
        }
        public void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        //String methods
        public string RemoveEscapeSequence (string section)
        {
            if (section.StartsWith("\r")) section = section.Remove(0, 2);
            return section;
        }
        public string CopyDataToArray(string file, string field)
        {                     
            //Comprobamos si ha introduccido mas de un campo
            List<string> fields = new List<string>();
            if(field.Contains(','))
            {
                 string[] array = field.Split(',');
                 
                 for (int i = 0; i < array.Length; i++)
                 { fields.Add(array[i]); }
            }
            
            Console.WriteLine("\nEjecutando operaciones menores... (1/3)");

            //Primero se dividen todas las lineas del archivo
            string[] fileLines = file.Split("\n");
            //Se crea la variable en la que se va a guardar la informacion
            string[] data = new string[fileLines.Length];

            ClearCurrentConsoleLine();
            Console.WriteLine("Ejecutando operaciones menores... (2/3)");

            //Se quita la secuencia de escape \r
            for (int i = 1; i < fileLines.Length; i++) { fileLines[i] = RemoveEscapeSequence(fileLines[i]); }

            ClearCurrentConsoleLine();
            Console.WriteLine("Ejecutando operaciones menores... (3/3)");

            //Se crean las secciones
            for (int i = 0; i < fileLines.Length; i++) { if (fileLines[i] == "!\r") fileLines[i] = "\\section"; }
            string[] fileSections = string.Join("\n", fileLines).Split("\\section");

            Console.WriteLine("Completadas\n\n");

            if(fields.Count == 0) return OneFieldOperation(data, fileSections, field);
            else return MultipleFieldOperation(data, fileSections, fields);
        }
        
        public string OneFieldOperation(string[] data, string[] fileSections, string field){
            int e = 1;
            data[0] = "ZPID," + field + "\n";
            for (int i = 0; i < fileSections.Length; i++, e++)
            {
                //Porcentaje
                ClearCurrentConsoleLine();
                Console.WriteLine("Dando formato a la informacion: " + (i * 100 / fileSections.Length) + "%");

                //Cargar las secciones correspondientes
                List<string> names = GetFieldNames(fileSections, i);
                List<string> values = GetFieldValues(fileSections, i);
                
                //Borrar los valores vacios
                if (names[0] == "") names.Remove("");
                if (values[0] == "") values.Remove("");

                //Comprueba que los pids son correctos
                if (CheckPids(values[0]))
                {
                    int pos = 0;
                    if (names.Contains(field))
                    {
                        pos = names.IndexOf(field);
                        if (values[pos] != "###BLANKS###") data[i] = values[0] + "," + values[pos] + "\n";
                        else data[i] =  values[0] + ",\n";
                    }
                    else
                    {
                        data[i] = values[0] + ",\n";
                    }
                }

                //Dejar en null las  variables
                names = new List<string>();
                values = new List<string>();
            }
            return string.Join("", data);
        } 
        
        public string MultipleFieldOperation(string[] data, string[] fileSections, List<string> fields){
            int e = 1;
            
            //Header
            data[0] = "ZPID,";

            for(int i = 0; i < fields.Count; i++)
            { 
                if(i != fields.Count) data[0] += fields[i] + ","; 
                else data[0] += fields[i]; 
            }

            data[0] += "\n";

            //Body
            for (int i = 0; i < fileSections.Length; i++, e++)
            {
                //Porcentaje
                ClearCurrentConsoleLine();
                Console.WriteLine("Dando formato a la informacion: " + (i * 100 / fileSections.Length) + "%");

                //Cargar las secciones correspondientes
                List<string> names = GetFieldNames(fileSections, i);
                List<string> values = GetFieldValues(fileSections, i);
                
                //Borrar los valores vacios
                if (names[0] == "") names.Remove("");
                if (values[0] == "") values.Remove("");

                //Comprueba que los pids son correctos
                if (CheckPids(values[0]))
                    {
                        data[i] = values[0] + ",";
                        
                        int pos = 0;
                        for (int x = 0; x < fields.Count; x++)
                        {
                            if (names.Contains(fields[x]))
                            {
                                pos = names.IndexOf(fields[x]);
                                if(pos != - 1){
                                    if (values[pos] != "###BLANKS###") data[i] += values[pos] + ",";
                                    else data[i] += ",";
                                }    
                            }
                            else data[i] += ",";
                        }
                        data[i] += "\n";
                    }        
                //Dejar en null las  variables
                names = new List<string>();
                values = new List<string>();
            }
            return string.Join("", data);
        } 

        public List<string> GetFieldNames(string[] fileSections, int section)
        {
            List<string> names = new List<string>();
            //Se dividen las secciones por lineas para sacar los nombres de los campos
            string[] fileSection = fileSections[section].Split("\n");
            for (int i = 0; i < fileSection.Length - 1; i++)
            {
                //Obtener el index del =
                int index = fileSection[i].IndexOf('=');
                //Si el index no es nulo se hace un substring para obtener el nombre de los campos
                if (index != -1 ) names.Insert(i,fileSection[i].Substring(0, index));
                else if (index == -1 || index == 0) names.Insert(i, string.Empty);
            }
            return names;
        }
        public List<string> GetFieldValues(string[] fileSections, int section)
        {
            List<string> values = new List<string>();
            //Se dividen las secciones por lineas para sacar los nombres de los campos
            string[] fileSection = fileSections[section].Split("\n");
            for (int i = 0; i < fileSection.Length; i++)
            {
               //Obtener el index del =
                int index = 0; 
                index = fileSection[i].IndexOf('=') + 1;
                //Si el index no es nulo se hace un substring para obtener el valor de los campos
                if (index != 0 && index != -1) values.Insert(i, fileSection[i].Substring(index, (fileSection[i].Length - 1 - (index) ) ) ); 
                else if ((index == 0 || index == -1)) values.Insert(i, string.Empty);

            }
            return values;
        }
        public bool CheckPids (string pid)
        {
            if (pid.Length > 1)
            {
                //Comprueba si tiene 4 o 5 caracteres 
                if (pid.Length == 4)
                {
                    if (Char.IsNumber(pid, 0) && Char.IsNumber(pid, 1) && Char.IsNumber(pid, 2) && Char.IsNumber(pid, 3))
                    {
                        return true;
                    }
                }
                if (pid.Length == 5)
                {
                   //Comprueba si contiene A o C 
                    if (pid.StartsWith('A') && Char.IsNumber(pid, 1) && Char.IsNumber(pid, 2) && Char.IsNumber(pid, 3) && Char.IsNumber(pid, 4))
                    {
                        return true;
                    }
                    else if (pid.StartsWith('C') && Char.IsNumber(pid, 1) && Char.IsNumber(pid, 2) && Char.IsNumber(pid, 3) && Char.IsNumber(pid, 4))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
