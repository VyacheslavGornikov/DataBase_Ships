using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите название порта: ");
            Ship.Port = Console.ReadLine();
            Console.Write("Введите максимально возможное число ремонтов корабля: ");
            Ship.MaxRepair = int.Parse(Console.ReadLine());
            List<Ship> ships = new List<Ship>(); // Создание пустого массива объектов класса Ship

            bool isRunning = true; // Переменная, отвечающая за работу цикла
            while (isRunning)
            {
                Console.Clear(); // Очистка экрана
                PrintMenu(); // Вывод меню на экран
                int choice = int.Parse(Console.ReadLine()); // Выбор пункта меню
                switch (choice)
                {
                    case 1: // Добавление в БД
                        AddShip(ships);
                        break;
                    case 2: // Удаление из БД
                        if (ships.Count == 0)
                            Console.WriteLine("В порту {0} нет кораблей", Ship.Port);
                        else
                        {   // Вводится номер удаляемого корабля
                            Console.Write("Введите номер корабля для удаления 1 - {0}: ", ships.Count);
                            int removeId = int.Parse(Console.ReadLine());
                            while (removeId < 1 || removeId > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                removeId = int.Parse(Console.ReadLine());
                            }
                            ships.RemoveAt(removeId - 1); // Удаление объекта из массива по номеру
                            Console.WriteLine("Корабль #{0} успешно удален из БД", removeId);
                        }
                        Console.ReadKey();
                        break;
                    case 3: // Изменение данных корабля в БД
                        if (ships.Count == 0)
                            Console.WriteLine("В порту {0} нет кораблей", Ship.Port);
                        else
                        {   // Вводится номер изменяемого корабля
                            Console.Write("Введите номер корабля для изменения данных 1 - {0}: ", ships.Count);
                            int changeId = int.Parse(Console.ReadLine());
                            while (changeId < 1 || changeId > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                changeId = int.Parse(Console.ReadLine());
                            }
                            Console.WriteLine();
                            Console.WriteLine("***Корабль до изменения данных***");
                            PrintOneShip(ships, changeId - 1); // Вывод на экран выбранного корабля до изменения
                            //EditShipData(ships, changeId); // Изменение данных
                            ships[changeId - 1].EditData();
                            Console.WriteLine();
                            Console.WriteLine("***Корабль после изменения данных***");
                            PrintOneShip(ships, changeId - 1); // Вывод на экран выбранного корабля после изменения
                        }
                        Console.ReadKey();
                        break;
                    case 4: // Поиск кораблей в БД
                        if (ships.Count == 0)
                            Console.WriteLine("В порту {0} нет кораблей", Ship.Port);
                        else
                            SearchShipData(ships); // Поиск кораблей по заданным параметрам
                        Console.ReadKey();
                        break;
                    case 5: // Вывод на экран всех кораблей, содержащихся в БД
                        PrintAllShips(ships);
                        break;
                    case 6: // Вывод на экран выбранного корабля
                        if (ships.Count == 0)
                            Console.WriteLine("В порту {0} нет кораблей", Ship.Port);
                        else
                        {   // Вводится номер выводимого на экран корабля
                            Console.Write("Введите номер корабля для отображения 1 - {0}: ", ships.Count);
                            int id = int.Parse(Console.ReadLine());
                            while (id < 1 || id > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                id = int.Parse(Console.ReadLine());
                            }
                            Console.WriteLine();
                            PrintOneShip(ships, id - 1); // Вывод на экран выбранного корабля
                        }                        
                        Console.ReadKey();
                        break;
                    case 7:
                        SerializeData(ships);
                        Console.ReadKey();
                        break;
                    case 8:
                        DeserializeData(ref ships); 
                        Console.ReadKey();
                        break;
                    case 0: // Завершение работы программы
                        isRunning = false; 
                        break;
                    default: // Неверно введенный пункт меню
                        Console.WriteLine("Неверный выбор! Нажмите любую клавишу и попробуйте еще раз");
                        Console.ReadKey();
                        break;

                }
            }
            Console.WriteLine("Завершение работы программы...");
        }

        private static void SearchShipData(List<Ship> ships) // Метод для поиска кораблей по заданным параметрам
        {
            Console.WriteLine("Параметры для поиска:");
            Console.WriteLine("[1] По названию корабля");
            Console.WriteLine("[2] По типу корабля");
            Console.WriteLine("[3] По диапазону годов выпуска");
            Console.Write("Введите пункт меню: ");
            int searchChoice = int.Parse(Console.ReadLine());
            while (searchChoice < 1 || searchChoice > 3)
            {
                Console.Write("Неверный выбор! Попробуйте еще раз: ");
                searchChoice = int.Parse(Console.ReadLine());
            }
            int count;
            switch (searchChoice)
            {
                case 1:
                    Console.Write("Введите название корабля: ");
                    string keyName = Console.ReadLine();
                    Console.WriteLine("\n***Результаты поиска***");
                    Console.WriteLine();
                    count = 0;
                    for (int i = 0; i < ships.Count; i++)
                    {
                        if (ships[i].Name == keyName)
                        {
                            PrintOneShip(ships, i);
                            count++;
                        }
                    }
                    if (count == 0)
                        Console.WriteLine("Совпадения не найдены!");
                    break;
                case 2:
                    Console.Write("Введите тип корабля (1 - пассажирский, 2 - военный): ");
                    int keyType = int.Parse(Console.ReadLine());
                    Console.WriteLine("\n***Результаты поиска***");
                    Console.WriteLine();
                    count = 0;
                    for (int i = 0; i < ships.Count; i++)
                    {
                        if (ships[i].Type == (ShipType)keyType)
                        {
                            PrintOneShip(ships, i);
                            count++;
                        }
                    }
                    if (count == 0)
                        Console.WriteLine("Совпадения не найдены!");
                    break;
                case 3:
                    Console.Write("Введите нижнюю границу: ");
                    int lowerLim = int.Parse(Console.ReadLine());
                    Console.Write("Введите верхнюю границу: ");
                    int upperLim = int.Parse(Console.ReadLine());
                    while (lowerLim > upperLim)
                    {
                        Console.Write("Верхняя граница должна быть больше нижней! Повторите ввод: ");
                        upperLim = int.Parse(Console.ReadLine());
                    }
                    Console.WriteLine("\n***Результаты поиска***");
                    Console.WriteLine();
                    count = 0;
                    for (int i = 0; i < ships.Count; i++)
                    {
                        if (lowerLim <= ships[i].Year && ships[i].Year <= upperLim)
                        {
                            PrintOneShip(ships, i);
                            count++;
                        }
                    }
                    if (count == 0)
                        Console.WriteLine("Совпадения не найдены!");
                    break;
            }
        }

        //private static void EditShipData(List<Ship> ships, int changeId) // Метод для изменения данных выбранного корабля
        //{
        //    Console.WriteLine("Параметры для редактирования:");
        //    Console.WriteLine("[1] Изменить название корабля");
        //    Console.WriteLine("[2] Изменить тип корабля");
        //    Console.WriteLine("[3] Изменить количество членов экипажа");
        //    Console.WriteLine("[4] Изменить рекомендуемую скорость");
        //    Console.WriteLine("[5] Добавить дату ремонта");
        //    Console.WriteLine("[6] Изменить год выпуска(для созданных по умолчанию)");
        //    Console.Write("Введите пункт меню: ");
        //    int editChoice = int.Parse(Console.ReadLine());
        //    while (editChoice < 1 || editChoice > 6)
        //    {
        //        Console.WriteLine("Неверный выбор! Попробуйте еще раз: ");
        //        editChoice = int.Parse(Console.ReadLine());
        //    }
        //    switch (editChoice)
        //    {
        //        case 1:
        //            Console.Write("Введите новое название: ");
        //            string changeName = Console.ReadLine();
        //            ships[changeId - 1].Name = changeName;
        //            break;
        //        case 2:
        //            Console.Write("Введите новый тип корабля: ");
        //            string changeType = Console.ReadLine();
        //            ships[changeId - 1].Type = changeType;
        //            break;
        //        case 3:
        //            Console.Write("На сколько изменить количество членов экипажа([+] число увеличивает экипаж, [-] уменьшает: ): ");
        //            int crewInc = int.Parse(Console.ReadLine());
        //            ships[changeId - 1].ChangeCrew(crewInc);
        //            break;
        //        case 4:
        //            Console.Write("На сколько изменить рекомендуемую скорость([+] число увеличивает экипаж, [-] уменьшает: ): ");
        //            int speedInc = int.Parse(Console.ReadLine());
        //            ships[changeId - 1].ChangeRecommendedSpeed(speedInc);
        //            break;
        //        case 5:
        //            Console.Write("Введите дату ремонта в формате дд.мм.гггг: ");
        //            string inputStr = Console.ReadLine();
        //            string mask = "dd.MM.yyyy";
        //            DateTime date;
        //            while (!DateTime.TryParseExact(inputStr, mask, null, System.Globalization.DateTimeStyles.None, out date))
        //            {
        //                Console.Write("Дата введена неверно! Попробуйте еще раз: ");
        //                inputStr = Console.ReadLine();
        //            }
        //            ships[changeId - 1][ships[changeId - 1].NumberRepair] = date;
        //            break;
        //        case 6:
        //            Console.Write("Введите год выпуска: ");
        //            int changeYear = int.Parse(Console.ReadLine());
        //            ships[changeId - 1].Year = changeYear;
        //            break;
        //    }
        //}

        private static void PrintAllShips(List<Ship> ships) // Выводит на экран все корабли в БД
        {
            Console.WriteLine("\n***Данные всех кораблей***\n");
            if (ships.Count == 0)
                Console.WriteLine("В порту {0} нет кораблей", Ship.Port);
            else
            {
                for (int i = 0; i < ships.Count; i++)
                {
                    PrintOneShip(ships, i);
                }
            }
            Console.ReadKey();
        }

        private static void PrintOneShip(List<Ship> ships, int index) // Выводит на экран корабль по индексу
        {
            Console.WriteLine("Корабль #{0}", index + 1);
            Console.WriteLine(ships[index]);
            Console.WriteLine();
        }

        private static void AddShip(List<Ship> ships) // Добавляет корабль в БД
        {
            Console.WriteLine("Введите тип корабля (1 - пассажирский, 2 - военный): ");
            int enteredType = int.Parse(Console.ReadLine());
            while (enteredType < 1 || enteredType > 2)
            {
                Console.Write("Неверный выбор! Повторите ввод: ");
                enteredType = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Вы хотите создать:");
            Console.WriteLine("[1] Корабль с параметрами по умолчанию");
            Console.WriteLine("[2] Корабль с параметрами, введенными с клавиатуры");
            Console.Write("Ваш выбор: ");
            int choiceCase1 = int.Parse(Console.ReadLine());
            while (choiceCase1 < 1 || choiceCase1 > 2)
            {
                Console.Write("Неверный выбор! Повторите ввод: ");
                choiceCase1 = int.Parse(Console.ReadLine());
            }
            switch (choiceCase1)
            {
                case 1:
                    if (enteredType == 1)
                    {
                        PassengerShip newPassengerShip1 = new PassengerShip();
                        ships.Add(newPassengerShip1);
                        Console.WriteLine("Пассажирский корабль с параметрами по умолчнию успешно внесен в БД");                        
                    }
                    else
                    {
                        WarShip newWarShip1 = new WarShip();
                        ships.Add(newWarShip1);
                        Console.WriteLine("Военный корабль с параметрами по умолчнию успешно внесен в БД");
                    }
                    Console.ReadKey();
                    break;
                case 2:
                    Console.Write("Введите название корабля: ");
                    string newName = Console.ReadLine();
                    //Console.Write("Введите тип корабля: ");
                    //string newType = Console.ReadLine();
                    Console.Write("Введите число членов экипажа: ");
                    int newCrew = int.Parse(Console.ReadLine());
                    Console.Write("Введите рекомендуемую скорость: ");
                    double newMaxSpeed = double.Parse(Console.ReadLine());
                    Console.Write("Введите год выпуска: ");
                    int newYear = int.Parse(Console.ReadLine());
                    if (enteredType == 1)
                    {
                        Console.Write("Введите число пассажиров: ");
                        int newPassangers = int.Parse(Console.ReadLine());
                        Console.Write("Наличие ресторана (true - есть, false - нет): ");
                        bool newRestaraunt = bool.Parse(Console.ReadLine());
                        Console.Write("Введите площадь пассажирской каюты (м2): ");
                        double newSquare = double.Parse(Console.ReadLine());
                        PassengerShip newPassengerShip2 = new PassengerShip(newName, (ShipType)enteredType, newCrew, newMaxSpeed, newYear, newPassangers, newRestaraunt, newSquare);
                        CreateRepairDates(newPassengerShip2);
                        ships.Add(newPassengerShip2);
                        Console.WriteLine("Пассажирский корабль с заданными параметрами успешно внесен в БД");
                    }
                    else
                    {
                        Console.Write("Введите количество орудий: ");
                        int newGuns = int.Parse(Console.ReadLine());
                        Console.Write("Введите калибр орудий: ");
                        int newCaliber = int.Parse(Console.ReadLine());
                        Console.Write("Наличие торпед (true - есть, false - нет): ");
                        bool newTorpedoes = bool.Parse(Console.ReadLine());
                        WarShip newWarShip2 = new WarShip(newName, (ShipType)enteredType, newCrew, newMaxSpeed, newYear, newGuns, newCaliber, newTorpedoes);
                        CreateRepairDates(newWarShip2);
                        ships.Add(newWarShip2);
                        Console.WriteLine("Военнный корабль с заданными параметрами успешно внесен в БД");
                    }                                                       
                    Console.ReadKey();
                    break;
            }
        }

        private static void CreateRepairDates(Ship newShip)
        {
            Console.Write("Хотите внести даты ремонта?(Y/N): ");
            char ch = Console.ReadLine()[0];
            while (ch != 'Y' && ch != 'y' && ch != 'N' && ch != 'n')
            {
                Console.Write("Неверный символ! Повторите ввод: ");
                ch = Console.ReadLine()[0];
            }
            if (ch == 'Y' || ch == 'y')
            {
                Console.Write("Сколько дат ремонта вы хотите внести?: ");
                int repairLim = int.Parse(Console.ReadLine());
                for (int i = 0; i < repairLim && i < Ship.MaxRepair; i++)
                {
                    Console.Write("Введите дату {0}-го ремонта в формате дд.мм.гггг: ", i + 1);
                    string inputStr = Console.ReadLine();
                    string mask = "dd.MM.yyyy";
                    DateTime date;
                    while (!DateTime.TryParseExact(inputStr, mask, null, System.Globalization.DateTimeStyles.None, out date))
                    {
                        Console.Write("Дата введена неверно! Попробуйте еще раз: ");
                        inputStr = Console.ReadLine();
                    }
                    newShip[i] = date;
                    //Console.WriteLine("Entered data: {0}", date);
                }
            }
            else
                Console.WriteLine("Данные о датах ремонта не были внесены");
        }

        public static void SerializeData(List<Ship> ships)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("ShipsDB.bin", FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, ships);
                fs.Close();
                Console.WriteLine("Данные успешно сохранены в файле ShipsDB.bin!");
            }
        }

        public static void DeserializeData(ref List<Ship> ships)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("ShipsDB.bin", FileMode.Open, FileAccess.Read))
            {
                ships = (List<Ship>)bf.Deserialize(fs);
                fs.Close();
                Console.WriteLine("Данные успешно получены из файла ShipsDB.bin!");
            }
        }
        public static void PrintMenu() // Вывод пунктов меню на экран
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("[1] Добавить корабль в БД");
            Console.WriteLine("[2] Удалить корабль из БД");
            Console.WriteLine("[3] Изменить данные корабля");
            Console.WriteLine("[4] Поиск кораблей по заданному параметру");
            Console.WriteLine("[5] Вывод информации о всех кораблях на экран");
            Console.WriteLine("[6] Вывод информации об одном корабле на экран");
            Console.WriteLine("[7] Сериализация БД кораблей");
            Console.WriteLine("[8] Десериализация БД кораблей");
            Console.WriteLine("[0] Выход из программы");
            Console.Write("Введите пункт меню: ");
        }
    }    
}
