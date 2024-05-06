using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    [Serializable] // сериализуемый
    internal class WarShip : Ship, ICrewChanger // наследник класса Ship
    {
        private int guns;
        private int gunCaliber;
        private readonly bool torpedoes;

        public WarShip() // конструктор без параметров
        {
            Type = ShipType.Военный;
            guns = 2;
            gunCaliber = 76;
            torpedoes = false;
        }

        public WarShip(string name, ShipType type, int crew, double maxSpeed, int year, int guns, int gunCaliber, bool torpedoes) : base(name, type, crew, maxSpeed, year)
        { // конструктор с параметрами
            Guns = guns;
            GunCaliber = gunCaliber;
            this.torpedoes = torpedoes;
        }

        public int Guns // свойство поля guns
        { 
            get => guns; 
            set
            {
                if (value > 0)
                    guns = value;
                else
                    throw new ArgumentOutOfRangeException("Количество орудий должно быть положительным!");
            } 
        }
        public int GunCaliber // свойство поля gunCaliber
        { 
            get => gunCaliber; 
            set
            {
                if (value > 0)
                    gunCaliber = value;
                else
                    throw new ArgumentOutOfRangeException("Калибр орудий должен быть положительным!");
            } 
        }
        public bool Torpedoes => torpedoes; // свойство readonly поля torpedoes

        public void ChangeCrew(int increment) // Метод наследуемый от интерфейса, изменяющий численность экипажа
        {
            Crew += increment;
            if (Crew < 0)
            {
                throw new ArgumentOutOfRangeException("Число членов экипажа не может быть отрицательным!");
            }
        }

        public override void EditData() // Переопределенный метод изменения данных класса Ship
        {
            Console.WriteLine("Параметры для редактирования:");
            Console.WriteLine("[1] Изменить название корабля");
            Console.WriteLine("[2] Изменить количество членов экипажа");
            Console.WriteLine("[3] Изменить рекомендуемую скорость");
            Console.WriteLine("[4] Добавить дату ремонта");
            Console.WriteLine("[5] Изменить год выпуска(для созданных по умолчанию)");
            Console.WriteLine("[6] Изменить количество орудий");
            Console.WriteLine("[7] Изменить калибр орудий");

            Console.Write("Введите пункт меню: ");
            int editChoice;
            try
            {
                editChoice = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message + " Введите цифру!");
                editChoice = 0;
            }
            while (editChoice < 1 || editChoice > 7)
            {
                Console.Write("Неверный выбор! Попробуйте еще раз: ");
                try
                {
                    editChoice = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message + " Введите цифру!");
                    editChoice = 0;
                }
            }
            switch (editChoice)
            {
                case 1:
                    Console.Write("Введите новое название: ");
                    string changeName = Console.ReadLine();
                    this.Name = changeName;
                    break;
                case 2:
                    Console.Write("На сколько изменить количество членов экипажа([+] число увеличивает экипаж, [-] уменьшает: ): ");
                    int crewInc;
                    try
                    {
                        crewInc = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        crewInc = 0;
                    }
                    try
                    {
                        this.ChangeCrew(crewInc);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }
                    break;
                case 3:
                    Console.Write("На сколько изменить рекомендуемую скорость([+] число увеличивает экипаж, [-] уменьшает: ): ");
                    int speedInc;
                    try
                    {
                        speedInc = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        speedInc = 0;
                    }
                    try
                    {
                        this.ChangeRecommendedSpeed(speedInc);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }                    
                    break;
                case 4:
                    Console.Write("Введите дату ремонта в формате дд.мм.гггг: ");
                    string inputStr = Console.ReadLine();
                    string mask = "dd.MM.yyyy";
                    DateTime date;
                    while (!DateTime.TryParseExact(inputStr, mask, null, System.Globalization.DateTimeStyles.None, out date))
                    {
                        Console.Write("Дата введена неверно! Попробуйте еще раз: ");
                        inputStr = Console.ReadLine();
                    }
                    try
                    {
                        this[this.NumberRepair] = date;
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case 5:
                    Console.Write("Введите год выпуска: ");
                    int changeYear;
                    try
                    {
                        changeYear = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        changeYear = 0;
                    }
                    try
                    {
                        this.Year = changeYear;
                    }
                    catch (ShipException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }                    
                    break;
                case 6:
                    Console.Write("Введите количество орудий: ");
                    int changeGuns;
                    try
                    {
                        changeGuns = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        changeGuns = 0;
                    }
                    try
                    {
                        this.Guns = changeGuns;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }
                    break;
                case 7:
                    Console.Write("Введите калибр орудий: ");
                    int changeGunCaliber;
                    try
                    {
                        changeGunCaliber = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        changeGunCaliber = 0;
                    }
                    try
                    {
                        this.GunCaliber = changeGunCaliber;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }                    
                    break;
            }
        }
        public override string ToString() // Переопределенный метод класса Ship для вывода объекта класса через консоль
        {
            return base.ToString() + $"\nКоличество орудий: {guns}\nКалибр орудий: {GunCaliber}\nНаличие торпед: {torpedoes}";
        }

    }
}
