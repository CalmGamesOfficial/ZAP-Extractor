using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace ZAP_Converter
{
    class Program
    {
        //Variables Estaticas
        public static string file;
        public static string filePath;
        public static string outputPath;

        public static string fileName;

        public static DateTime startTime;

        static void Main(string[] args)
        {
            Strings strings = new Strings();

            //Titulo
            Console.Title = "ZAP CONVERTER";
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" ZAP CONVERTER                                                                                                          \n");
            Console.ResetColor();
            
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
                Console.WriteLine("No se ha encontrado el archivo especificado, por favor vuelva a introducirla:");
                Console.ForegroundColor = ConsoleColor.Green;
                filePath = Console.ReadLine();
                Console.WriteLine("\n");
            }
            
            //Una vez encontrado se guarda su nombre para su posterior uso
            string[] fileDir = filePath.Split('\\');
            fileName = fileDir[fileDir.Length - 1];
            fileName = fileName.Split('.')[0];

            //Cargar archivo
            try
            {
                file = File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                Console.WriteLine("IO Error: {0}", e.GetType().Name);
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
                Console.WriteLine("No se ha encontrado la ruta especificada, por favor vuelva a introducirla:");
                Console.ForegroundColor = ConsoleColor.Green;
                outputPath = Console.ReadLine();
            }

            //Preguntar campo a mostrar
            Console.ResetColor();
            Console.WriteLine("\nIntroduzca el campo que quiere que se muestre (escriba 'ShowAllFields' para ver todos los campos que hay)");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            string field = Console.ReadLine();
            Console.ResetColor();

            //Segun la respuesta que se de se inicia un bucle o el proceso de conversion
            if (field == "ShowAllFields")
            {
                strings.ShowAllFields(file);
            }
            else
            {
                //Confirmar si el usuario quiere convertir el archivo
                Console.ResetColor();
                Console.WriteLine("\nEstas seguro que deseas continuar? Si/No");
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
                        Console.WriteLine("\ncsv creado correctamente\n");
                        Console.WriteLine("Copiando informacion al csv...\n");

                        byte[] info = new UTF8Encoding(true).GetBytes(data);

                        fileStream.Write(info, 0, info.Length);
                    }
                    Console.WriteLine("Informacion copiada correctamente\n");
                }

             }

            //se inicia un bucle hasta que introduzca
            while (field == "ShowAllFields" || file == string.Empty)
            {
                Console.WriteLine("\nIntroduzca el campo que quiere que se muestre (escriba 'ShowAllFields' para ver todos los campos que hay)");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                field = Console.ReadLine();
                Console.WriteLine("\n");
                Console.ResetColor();

                if (field == "ShowAllFields")
                {
                    strings.ShowAllFields(file);
                }
                else
                {
                    //Confirmar si el usuario quiere convertir el archivo
                    Console.ResetColor();
                    Console.WriteLine("\nEstas seguro que deseas continuar? Si/No");
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
                            Console.WriteLine("\ncsv creado correctamente\n");
                            Console.WriteLine("Copiando informacion al csv...\n");

                            byte[] info = new UTF8Encoding(true).GetBytes(data);

                            fileStream.Write(info, 0, info.Length);
                        }
                        Console.WriteLine("Informacion copiada correctamente\n");
                    }
                }
            }

            TimeSpan time = DateTime.Now.Subtract(startTime);

            Console.Title = "ZAP CONVERTER";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conversion completa en " + time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00") + ", csv guardado en '" + outputPath + "'\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Pulse Intro para salir...");
            Console.ReadKey();
            Console.ResetColor();
        }

    }

    class Strings
    {
        public string RemoveEscapeSequence (string section)
        {
            if (section.StartsWith("\r")) section = section.Remove(0, 2);
            return section;
        }
        public string CopyDataToArray(string file, string field)
        {            
            //Primero se dividen todas las lineas del archivo
            string[] fileLines = file.Split("\n");
            //Se crea la variable en la que se va a guardar la informacion
            string[] data = new string[fileLines.Length];

            //Se quita la secuencia de escape \r
            for (int i = 1; i < fileLines.Length; i++) { fileLines[i] = RemoveEscapeSequence(fileLines[i]); }

            //Se crean las secciones
            for (int i = 0; i < fileLines.Length; i++) { if (fileLines[i] == "!\r") fileLines[i] = "\\section"; }
            string[] fileSections = string.Join("\n", fileLines).Split("\\section");

            int e = 1;
            data[0] = "ZPID," + field + "\n";
            for (int i = 0; i < fileSections.Length; i++, e++)
            {
                //Porcentaje
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
                    string element = string.Empty;
                    if (names.Contains(field))
                    {
                        element = names[names.IndexOf(field)];
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


        public void ShowAllFields(string file)
        {
            //Primero se divide en distintas secciones
            string[] fileSections = file.Split("!");
            for (int i = 1; i < fileSections.Length; i++)
            {
                fileSections[i] = RemoveEscapeSequence(fileSections[i]);
            }
            
            //Luego busca la seccion que mas campos tenga
            int section = 0;
            int theMostLength = 0;
            Console.WriteLine("Cargando todos los campos disponibles...\n");
            for (int i = 0; i < fileSections.Length; i++)
            {
                string[] fileElements = fileSections[i].Split("\n");
                if (fileElements.Length > theMostLength)
                {
                    theMostLength = fileElements.Length;
                    section = i;
                }
            }

            //Sacar los campos a mostrar
            string[] sectionElements = fileSections[section].Split("\n");
            for (int i = 0; i < sectionElements.Length; i++)
            {
                if (sectionElements[i].IndexOf('=') != -1)
                {
                    int count = 0;
                    count = (sectionElements[i].Length - sectionElements[i].IndexOf('='));
                    sectionElements[i] = sectionElements[i].Remove(sectionElements[i].LastIndexOf('='), count);
                    
                }
            }
            
            //Mostrar en pantalla
            for (int i = 0; i < sectionElements.Length / 6; i++)
            {
                if (i != 0) Console.WriteLine(sectionElements[i] + ", " + sectionElements[i + 1] + ", " + sectionElements[i + 2] + ", " + sectionElements[i + 3] + ", " + sectionElements[i + 4] + ", " + sectionElements[i + 5]);
            }

            //Finalizar metodo
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Pulsa intro para continuar\n");
            Console.ResetColor();
            Console.ReadKey();
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
//Creditos: Pau Garcia Chiner
