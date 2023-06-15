using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Internal;

namespace Zadanie8_
{
    class Program
    {
        // Класс "pdd"
        class Driver
        {
            public int driverKey { get; set; } // Ключ
            public string fullName { get; set; } // Имя
            public int experience { get; set; } // Опыт в годах
            public string number { get; set; } // номер
            public string document { get; set; } // удостоверение
            public DateOnly date { get; set; } // дата
            public int actNumber { get; set; } // номер акта
            public Driver(int k, string fn, int exp, string n, string doc, DateOnly dat, int act) // Конструктор
            { driverKey = k; fullName = fn; experience = exp; number = n; document = doc; date = dat; actNumber = act; }
        }
        class Car
        {
            public int carKey { get; set; } // Ключ
            public string company { get; set; } // фирма
            public string brand { get; set; } // марка
            public string type { get; set; } // тип кузова
            public string number { get; set; } // номер
            public Car(int k, string c, string b, string t, string n) // Конструктор
            { carKey = k; company = c; brand = b; type = t; number = n; }
        }
        class Gibdd
        {
            public int gibddKey { get; set; } // Ключ
            public string name_of_accident { get; set; } // название отдела ГБДД
            public int actNumber { get; set; } // номер акта
            public string driver { get; set; } // водитель
            public string number { get; set; } // номер
            public DateOnly date { get; set; } // дата
            public string location { get; set; } // место
            public int victims { get; set; }
            public string type { get; set; }
            public string reason { get; set; }
            public Gibdd(int k, string label, int act, string driv, string car_num, DateOnly d, string locat, int vict, string typ, string reas) // Конструктор
            { gibddKey = k; name_of_accident = label; actNumber = act; driver = driv; number = car_num; date = d; location = locat; victims = vict; type = typ; reason = reas; }
        }
        static void Main(string[] args)
        {
            Console.Write("Введите путь к папке с файлами для чтения:");
            string path = Console.ReadLine();

            StreamReader FileIn1 = new StreamReader(path + @"\Drivers.txt",
            Encoding.Default); //Заполняем таблицу "Водитель"
            string line1; //int k1; int d1; string[] n1;
            string[] ms1;
            List<Driver> drivers_list = new List<Driver>();
            while ((line1 = FileIn1.ReadLine()) != null) // Пока не конец файла
            {
                ms1 = line1.Split(';'); // Расщепление на массив строк
                                        // Добавление строки через конструктор с параметрами
                int key = Int32.Parse(ms1[0]);
                string f_n = ms1[1];
                int exp = Int32.Parse(ms1[2]);
                string c_n = ms1[3];
                string doc = ms1[4];
                var dat = DateOnly.Parse(ms1[5]);
                int act = Int32.Parse(ms1[6]);


                drivers_list.Add(new Driver(key, f_n, exp, c_n, doc, dat, act));
            }

            StreamReader FileIn2 = new StreamReader(path + @"\Cars.txt",
            Encoding.Default); //Заполняем таблицу "Cars"
            string line2; //int k1; int d1; string[] n1;
            string[] ms2;
            List<Car> cars_list = new List<Car>();
            while ((line2 = FileIn2.ReadLine()) != null) // Пока не конец файла
            {
                ms2 = line2.Split(';'); // Расщепление на массив строк
                                        // Добавление строки через конструктор с параметрами
                int keyCar = Int32.Parse(ms2[0]);
                string comp = ms2[1];
                string br = ms2[2];
                string t_k = ms2[3];
                string num = ms2[4];

                cars_list.Add(new Car(keyCar, comp, br, t_k, num));
            }

            StreamReader FileIn3 = new StreamReader(path + @"\gbdd.txt",
            Encoding.Default); //Заполняем таблицу "gbdd"
            string line3; //int k1; int d1; string[] n1;
            string[] ms3;
            List<Gibdd> gbdd_list = new List<Gibdd>();
            while ((line3 = FileIn3.ReadLine()) != null) // Пока не конец файла
            {
                ms3 = line3.Split(';'); // Расщепление на массив строк
                                        // Добавление строки через конструктор с параметрами
                int gibddKey = Int32.Parse(ms3[0]);
                string accident_name = ms3[1];
                int act_num = Int32.Parse(ms3[2]);
                string driver = ms3[3];
                string num = ms3[4];
                DateOnly date = DateOnly.Parse(ms3[5]);
                string loc = ms3[6];
                int victims = Int32.Parse(ms3[7]);
                string type = ms3[8];
                string reason = ms3[9];

                gbdd_list.Add(new Gibdd(gibddKey, accident_name, act_num, driver, num, date, loc, victims, type, reason));
            }

            /////////////////////////                           ФУНКЦИИ                                                         ////////////////////////////////////

            void print_all()
            {
                foreach (Driver d in drivers_list)
                {
                    Console.WriteLine("{0,-2}\t{1,-12}\t{2,-3}\t{3,-3}\t{4,-3}", d.driverKey, d.fullName, d.experience, d.number, d.document);
                }
                Console.WriteLine("");

                foreach (Car c in cars_list)
                {
                    Console.WriteLine($"{c.carKey}, {c.company}, {c.brand}, {c.type}, {c.number}");
                }
                Console.WriteLine("");
                foreach (Gibdd g in gbdd_list)
                {
                    Console.WriteLine($"{g.gibddKey}, {g.name_of_accident}, {g.actNumber}, {g.driver}, {g.number}, {g.date}, {g.location}, {g.victims}, {g.type}, {g.reason}");
                }
                Console.WriteLine("");
            }

            void find_driv_crim_sort()
            {
                foreach (Driver driv in drivers_list)
                {
                    int number = (from gibd in gbdd_list where gibd.driver.Equals(driv.fullName) select gibd).Count();
                    if (number > 1) Console.WriteLine(driv.fullName + ", число преступлений: " + number);
                }

            }

            void find_for_location(string locat)
            {
                var selected = from gibd in gbdd_list where gibd.location.Contains(locat) select gibd.driver;
                selected = selected.Distinct();

                foreach (var driver in selected)
                {
                    Console.WriteLine(driver);
                }

            }

            void find_for_date(DateTime dat)
            {
                var selected = from gibd in gbdd_list where gibd.date.Equals(dat) select gibd.driver;
                selected = selected.Distinct();

                foreach (var driver in selected)
                {
                    Console.WriteLine(driver);
                }

            }

            void find_accident_with_max_victims()
            {
                foreach (var g in gbdd_list)
                {
                    var max_num_victims = (from gibd in gbdd_list where gibd.victims > 0 select gibd.victims).Max();
                    if (g.victims == max_num_victims) Console.WriteLine("Номер акта " + g.actNumber + ", количество жертв: " + max_num_victims);
                }

            }

            void give_list_of_drivers_which_runover_a_pedestrian()
            {
                var selected = (from gibd in gbdd_list where gibd.type.Contains("наезд на пешехода") select gibd.driver).Distinct();

                foreach (var driver in selected)
                {
                    Console.WriteLine(driver);
                }
            }

            void give_reason_ByDescending()
            {
                var reasons = (from gibd in gbdd_list group gibd.reason by gibd.reason into value select new { value = value.Key, count = value.Count() }).ToArray();

                foreach (var item in reasons.OrderByDescending(m => m.count))
                {
                    Console.WriteLine(item.value + ": " + item.count);
                }
            }

            void delete_gibdd_for_id(int id)
            {
                foreach (Gibdd item in gbdd_list.ToList())
                {
                    if (item.gibddKey == id)
                    {
                        gbdd_list.Remove(item);
                        Console.WriteLine("Запись с указанным номером успешно удалена!");
                        return;
                    }
                }
                Console.WriteLine("Записи с указанным номером не найдено!");
            }

            void delete_driver_for_id(int id)
            {
                foreach (Driver item in drivers_list.ToList())
                {
                    if (item.driverKey == id)
                    {
                        drivers_list.Remove(item);
                        Console.WriteLine("Запись с указанным номером успешно удалена!");
                        return;
                    }
                }
                Console.WriteLine("Записи с указанным номером не найдено!");
            }

            void delete_car_for_id(int id)
            {
                foreach (Car item in cars_list.ToList())
                {
                    if (item.carKey == id)
                    {
                        cars_list.Remove(item);
                        Console.WriteLine("Запись с указанным номером успешно удалена!");
                        return;
                    }
                }
                Console.WriteLine("Записи с указанным номером не найдено!");
            }

            void add_gibdd(string strr)
            {
                try
                {
                    ms3 = strr.Split(';'); // Расщепление на массив строк
                                           // Добавление строки через конструктор с параметрами
                    string accident_name = ms3[0];
                    int act_num = Int32.Parse(ms3[1]);
                    string driver = ms3[2];
                    string num = ms3[3];
                    DateOnly date = DateOnly.Parse(ms3[4]);
                    string loc = ms3[5];
                    int victims = Int32.Parse(ms3[6]);
                    string type = ms3[7];
                    string reason = ms3[8];

                    while (true)
                    {
                        Console.Write("Введите айди:");
                        int idd = Int32.Parse(Console.ReadLine());
                        int ch = (from gibd in gbdd_list where gibd.gibddKey == idd select gibd).Count();
                        if (ch == 0)
                        {
                            gbdd_list.Add(new Gibdd(idd, accident_name, act_num, driver, num, date, loc, victims, type, reason));
                            return;
                        }
                        else { Console.WriteLine("Такой айди уже занят!"); continue; }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный формат!");
                    return;
                }
            }

            void add_driver(string strr)
            {
                try
                {
                    ms1 = strr.Split(';'); // Расщепление на массив строк
                                           // Добавление строки через конструктор с параметрами

                    string f_n = ms1[0];
                    int exp = Int32.Parse(ms1[1]);
                    string c_n = ms1[2];
                    string doc = ms1[3];
                    var dat = DateOnly.Parse(ms1[4]);
                    int act = Int32.Parse(ms1[5]);

                    while (true)
                    {
                        Console.Write("Введите айди:");
                        int idd = Int32.Parse(Console.ReadLine());
                        int ch = (from driv in drivers_list where driv.driverKey == idd select driv).Count();
                        if (ch == 0)
                        {
                            drivers_list.Add(new Driver(idd, f_n, exp, c_n, doc, dat, act));
                            return;
                        }
                        else { Console.WriteLine("Такой айди уже занят!"); continue; }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный формат!");
                    return;
                }
            }

            void add_car(string strr)
            {
                try
                {
                    ms2 = strr.Split(';'); // Расщепление на массив строк
                                           // Добавление строки через конструктор с параметрами

                    string comp = ms2[0];
                    string br = ms2[1];
                    string t_k = ms2[2];
                    string num = ms2[3];

                    while (true)
                    {
                        Console.Write("Введите айди:");
                        int idd = Int32.Parse(Console.ReadLine());
                        int ch = (from car in cars_list where car.carKey == idd select car).Count();
                        if (ch == 0)
                        {
                            cars_list.Add(new Car(idd, comp, br, t_k, num));
                            return;
                        }
                        else { Console.WriteLine("Такой айди уже занят!"); continue; }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный формат!");
                    return;

                }
            }



            ///////////////////////////               ИНТЕРФЕЙС          /////////////////////////////////////////////////////////////////

            while (true)
            {
                Console.Write("\nСписко комманд: \n0 - лица с числом преступлений >1\n1 - водители с ДТП в указанном месте" +
                    "\n2 - водители в указанную дату\n3 - ДТП с макс. числом жертв\n4 - Водители с наездом на пешехода" +
                    "\n5 - Причины ДТП от частых до редких\n6 - Вывести базы\n7 - Работа с водителями\n8 - работа с машинами\n9 - работа с ГИБДД\n\nВведите команду:");
                //"\n8 - Добавить запись ГИБДД\n9 - Добавить запись Driver\n10 - Добавить запись Car\n\nВведите команду:");
                switch (Console.ReadLine())
                {
                    case "0":
                        find_driv_crim_sort();
                        break;
                    case "1":
                        Console.Write("Введите адрес:");
                        find_for_location(Console.ReadLine());
                        break;
                    case "2":
                        Console.Write("Введите дату:");
                        find_for_date(DateTime.Parse(Console.ReadLine()));
                        break;
                    case "3":
                        find_accident_with_max_victims();
                        break;
                    case "4":
                        give_list_of_drivers_which_runover_a_pedestrian();
                        break;
                    case "5":
                        give_reason_ByDescending();
                        break;
                    case "6":
                        print_all();
                        break;
                    case "7":
                        Console.Write("0 - удалить запись, 1 - добавить запись\n\nВведите команду:");
                        switch (Console.ReadLine())
                        {
                            case "0":
                                Console.Write("Введите id:");
                                delete_driver_for_id(Int32.Parse(Console.ReadLine()));
                                break;
                            case "1":
                                Console.Write("Введите данные в формате БД(без id):");
                                add_driver(Console.ReadLine());
                                break;
                            default:
                                break;
                        }
                        break;
                    case "8":
                        Console.Write("0 - удалить запись, 1 - добавить запись\n\nВведите команду:");
                        switch (Console.ReadLine())
                        {
                            case "0":
                                Console.Write("Введите id:");
                                delete_car_for_id(Int32.Parse(Console.ReadLine()));
                                break;
                            case "1":
                                Console.Write("Введите данные в формате БД(без id):");
                                add_car(Console.ReadLine());
                                break;
                            default:
                                break;
                        }
                        break;
                    case "9":
                        Console.Write("0 - удалить запись, 1 - добавить запись\n\nВведите команду:");
                        switch (Console.ReadLine())
                        {
                            case "0":
                                Console.Write("Введите id:");
                                delete_gibdd_for_id(Int32.Parse(Console.ReadLine()));
                                break;
                            case "1":
                                Console.Write("Введите данные в формате БД(без id):");
                                add_gibdd(Console.ReadLine());
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}


