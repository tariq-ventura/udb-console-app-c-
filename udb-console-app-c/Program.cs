using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace udb_console_app_c
{
    internal class Program
    {
        public static int userType;

        public struct admin
        {
            public string username;
            public string password;
        }

        public struct seller
        {
            public string username;
            public string password;
        }

        public static List<admin> adminList = new List<admin>();
        public static List<seller> sellerList = new List<seller>();

        public static bool createUser(int userType)
        {
            Console.Write("\n" + "ingrese el nombre de usuario: ");
            string username = Console.ReadLine();

            Console.Write("\n" + "ingrese la contraseña: ");
            string password = Console.ReadLine();

            if (!dataValidation(password))
            {
                Console.WriteLine("La contraseña debe contener almenos 8 caracteres");
                return false;
            }

            bool confirm = false;

            switch (userType)
            {
                case 1:
                    adminList.Add(new admin { username = username, password = password });
                    confirm = writer("admin", username, password);
                    break;
                case 2:
                    sellerList.Add(new seller { username = username, password = password });
                    confirm = writer("seller", username, password);
                    break;
            }

            if (confirm)
            {
                Console.Clear();
                Console.WriteLine("Usuario Ingresado Correctamente" + "\n");
                adminMenu();
            }

            return true;
        }

        public static bool dataValidation(string password)
        {
            bool ok = true;

            if (password.Trim().Length < 8)
            {
                ok = false;
            }

            return ok;
        }

        public static bool writer(string fileName, string username, string password)
        {
            string rutaEjecutable = Directory.GetCurrentDirectory();

            string rutaPadre = Directory.GetParent(rutaEjecutable).Parent?.Parent?.FullName;

            StreamWriter sw = new StreamWriter(rutaPadre + "/" + fileName + ".txt", true);

            sw.WriteLine(username + " , " + password);

            sw.Close();

            return true;
        }

        public static void reader(string fileName)
        {
            string rutaEjecutable = Directory.GetCurrentDirectory();

            string rutaPadre = Directory.GetParent(rutaEjecutable).Parent?.Parent?.FullName;

            string line;

            try
            {
                StreamReader sr = new StreamReader(rutaPadre + "/" + fileName + ".txt");

                while ((line = sr.ReadLine()) != null)
                {
                    string[] partes = line.Split(',');
                    if (partes.Length == 2)
                    {
                        if (fileName.Equals("admin"))
                        {
                            adminList.Add(new admin { username = partes[0].Trim(), password = partes[1].Trim() });
                        }
                        else
                        {
                            sellerList.Add(new seller { username = partes[0].Trim(), password = partes[1].Trim() });
                        }
                    }
                }
                sr.Close();
            }
            catch
            {

            }
        }

        public static void mainMenu()
        {
            bool runner = true;
            do
            {
                Console.WriteLine("\n" + "Seleccione con que tipo de usuario desea entrar: ");
                Console.WriteLine("\n" + "1) administrador: ");
                Console.WriteLine("\n" + "2) vendedor: ");
                Console.WriteLine("\n" + "3) cliente: " + "\n");

                bool validation = int.TryParse(Console.ReadLine(), out userType);

                if (!validation || userType > 3)
                {
                    Console.Clear();
                    Console.WriteLine("Opcion no valida");
                    continue;
                }

                runner = false;
            } while (runner);
        }

        public static void login(int userType)
        {
            Console.Clear();
            bool runner = true;

            do
            {
                Console.Write("\n" + "ingrese su nombre de usuario: ");
                string username = Console.ReadLine();

                Console.Write("\n" + "ingrese su contraseña: ");
                string password = Console.ReadLine();

                switch (userType)
                {
                    case 1:
                        admin findAdmin = adminList.Find(u => u.username == username.Trim() && u.password == password.Trim());

                        if (findAdmin.username == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Usuario o contraseña incorrecta");
                            continue;
                        }

                        break;
                    case 2:
                        seller findSeller = sellerList.Find(u => u.username == username.Trim() && u.password == password.Trim());

                        if (findSeller.username == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Usuario o contraseña incorrecta");
                            continue;
                        }

                        break;
                }

                runner = false;
            } while (runner);
        }

        public static void adminMenu()
        {
            int option;
            bool runner = true;
            do
            {
                Console.WriteLine("\n" + "Seleccione que desea hacer: ");
                Console.WriteLine("\n" + "1) Crear usuario administrador ");
                Console.WriteLine("\n" + "2) Crear usuario vendedor ");
                Console.WriteLine("\n" + "3) Ver ventas ");
                Console.WriteLine("\n" + "4) Volver " + "\n");

                bool validation = int.TryParse(Console.ReadLine(), out option);

                if (!validation || option > 4)
                {
                    Console.Clear();
                    Console.WriteLine("Opcion no valida");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        createUser(option);
                        break;
                    case 2:
                        createUser(option);
                        break;
                    case 3:
                        // menu de ventas
                        break;
                    case 4:
                        mainMenu();
                        break;
                }

                runner = false;
            } while (runner);
        }

        static void Main(string[] args)
        {
            reader("admin");
            reader("seller");

            if (adminList.Count() < 1)
            {
                bool runner = false;

                do
                {
                    runner = createUser(1);
                } while (!runner);

            }

            mainMenu();

            if (userType != 3)
            {
                login(userType);
            }

            switch (userType)
            {
                case 1:
                    Console.Clear();
                    adminMenu();
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            Console.ReadKey();
        }
    }
}
