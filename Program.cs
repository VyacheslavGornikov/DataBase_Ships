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
            try
            {
                Ship.MaxRepair = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message + " Максимальное число ремонтов будет равно 3");
                Ship.MaxRepair = 3;
                Console.ReadKey();
            }
            List<Ship> ships = new List<Ship>(); // Создание пустого массива объектов класса Ship

            bool isRunning = true; // Переменная, отвечающая за работу цикла
            while (isRunning)
            {
                Console.Clear(); // Очистка экрана
                PrintMenu(); // Вывод меню на экран
                int choice;               
                try
                {
                    choice = int.Parse(Console.ReadLine()); // Выбор пункта меню                   
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message + " Необходимо ввести цифру!");
                    choice = -1; // для перезапуска цикла через метку default switch                    
                }
                
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
                            int removeId;
                            try
                            {
                                removeId = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message + " Введите цифру!");
                                removeId = 0;
                            }
                            while (removeId < 1 || removeId > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                try
                                {
                                    removeId = int.Parse(Console.ReadLine());
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine(ex.Message + " Введите цифру!");
                                    removeId = 0;
                                }
                                
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
                            int changeId;
                            try
                            {
                                changeId = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message + " Введите цифру!");
                                changeId = 0;
                            }
                            while (changeId < 1 || changeId > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                try
                                {
                                    changeId = int.Parse(Console.ReadLine());
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine(ex.Message + " Введите цифру!");
                                    changeId = 0;
                                }
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
                            int id;
                            try
                            {
                                id = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message + " Введите цифру!");
                                id = 0;
                            }
                            while (id < 1 || id > ships.Count)
                            {
                                Console.Write("Неверный номер! Повторите ввод: ");
                                try
                                {
                                    id = int.Parse(Console.ReadLine());
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine(ex.Message + " Введите цифру!");
                                    id = 0;
                                }
                            }
                            Console.WriteLine();
                            PrintOneShip(ships, id - 1); // Вывод на экран выбранного корабля
                        }                        
                        Console.ReadKey();
                        break;
                    case 7: // Сериализация данных
                        SerializeData(ships);
                        Console.ReadKey();
                        break;
                    case 8: // Десериализация данных
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
            Console.Write("Введите тип корабля (1 - пассажирский, 2 - военный): ");
            int enteredType;
            try
            {
                enteredType = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message + " Введите цифру!");
                enteredType = 0;
            }
            
            while (enteredType < 1 || enteredType > 2)
            {
                Console.Write("Неверный выбор! Повторите ввод: ");
                try
                {
                    enteredType = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message + " Введите цифру!");
                    enteredType = 0;
                }
            }
            Console.WriteLine("Вы хотите создать:");
            Console.WriteLine("[1] Корабль с параметрами по умолчанию");
            Console.WriteLine("[2] Корабль с параметрами, введенными с клавиатуры");
            Console.Write("Ваш выбор: ");
            int choiceCase1;
            try
            {
                choiceCase1 = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message + " Введите цифру!");
                choiceCase1 = 0;
            }
            
            while (choiceCase1 < 1 || choiceCase1 > 2)
            {
                Console.Write("Неверный выбор! Повторите ввод: ");
                try
                {
                    choiceCase1 = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message + " Введите цифру!");
                    choiceCase1 = 0;
                }
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
                    Console.Write("Введите число членов экипажа: ");
                    int newCrew;
                    try
                    {
                        newCrew = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение будет установлено в 1!");
                        newCrew = 1;
                    }
                    
                    Console.Write("Введите рекомендуемую скорость: ");
                    double newMaxSpeed; 
                    try
                    {
                        newMaxSpeed = double.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение будет установлено в 1!");
                        newMaxSpeed = 1;
                    }
                    Console.Write("Введите год выпуска: ");
                    int newYear = int.Parse(Console.ReadLine());
                    if (enteredType == 1)
                    {
                        Console.Write("Введите число пассажиров: ");
                        int newPassangers; 
                        try
                        {
                            newPassangers = int.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в 1!");
                            newPassangers = 1;
                        }
                        Console.Write("Наличие ресторана (true - есть, false - нет): ");
                        bool newRestaraunt; 
                        try
                        {
                            newRestaraunt = bool.Parse(Console.ReadLine()); ;
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в true!");
                            newRestaraunt = true;
                        }
                        Console.Write("Введите площадь пассажирской каюты (м2): ");
                        double newSquare;
                        try
                        {
                            newSquare = double.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в 10!");
                            newSquare = 10;
                        }
                        try
                        {
                            PassengerShip newPassengerShip2 = new PassengerShip(newName, (ShipType)enteredType, newCrew, newMaxSpeed, newYear, newPassangers, newRestaraunt, newSquare);
                            CreateRepairDates(newPassengerShip2);
                            ships.Add(newPassengerShip2);
                            Console.WriteLine("Пассажирский корабль с заданными параметрами успешно внесен в БД");
                        }
                        catch (ShipException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }
                        
                    }
                    else
                    {
                        Console.Write("Введите количество орудий: ");
                        int newGuns; 
                        try
                        {
                            newGuns = int.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в 1!");
                            newGuns = 1;
                        }
                        Console.Write("Введите калибр орудий: ");
                        int newCaliber; 
                        try
                        {
                            newCaliber = int.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в 76!");
                            newCaliber = 76;
                        }
                        Console.Write("Наличие торпед (true - есть, false - нет): ");
                        bool newTorpedoes;
                        try
                        {
                            newTorpedoes = bool.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + " Значение будет установлено в true!");
                            newTorpedoes = true;
                        }
                        try
                        {
                            WarShip newWarShip2 = new WarShip(newName, (ShipType)enteredType, newCrew, newMaxSpeed, newYear, newGuns, newCaliber, newTorpedoes);
                            CreateRepairDates(newWarShip2);
                            ships.Add(newWarShip2);
                            Console.WriteLine("Военнный корабль с заданными параметрами успешно внесен в БД");
                        }
                        catch (ShipException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Объект не будет создан!!!");
                        }

                    }                                                       
                    Console.ReadKey();
                    break;
            }
        }

        private static void CreateRepairDates(Ship newShip) // Метод добавления дат при создании объекта
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

        public static void SerializeData(List<Ship> ships) // Метод сериализации данных
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("ShipsDB.bin", FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, ships);
                fs.Close();
                Console.WriteLine("Данные успешно сохранены в файле ShipsDB.bin!");
            }
        }

        public static void DeserializeData(ref List<Ship> ships) // Метод десериализации данных
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
