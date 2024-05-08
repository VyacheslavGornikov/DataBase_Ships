using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    [Serializable] // Сериализуемый класс
    internal class PassengerShip : Ship, ICrewChanger // наследник класса Ship
    {
        private int passengers; // число пассажиров
        private readonly bool restaurant; // наличие ресторана
        private double passengerCabinArea; // площадь каюты пассажиров

        public PassengerShip() // конструктор без параметров
        {
            Type = ShipType.Пассажирский;
            passengers = 10;
            restaurant = true;
            passengerCabinArea = 30;
        }

        public PassengerShip(string name, ShipType type, int crew, double maxSpeed, int year, int passangers, bool restaurant, double passengerCabinArea) : base(name, type, crew, maxSpeed, year)
        { // конструктор с параметрами
            Passengers = passangers;
            this.restaurant = restaurant;
            PassengerCabinArea = passengerCabinArea;
        }

        public int Passengers // свойство поля passengers
        { 
            get => passengers; 
            set
            {
                if (value > 0)
                    passengers = value;
                else
                    throw new ArgumentOutOfRangeException("Число пассажиров должно быть положительным!");
            } 
        }
        public bool Restaurant => restaurant; // свойство readonly поля restaurant
        public double PassengerCabinArea // свойство поля passengerCabinArea 
        { 
            get => passengerCabinArea; 
            set
            {
                if (value > 0)
                    passengerCabinArea = value;
                else
                    throw new ArgumentOutOfRangeException("Площадь пассажирской каюты должна быть положительной!");
            } 
        }


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
            Console.WriteLine("[6] Изменить число пассажиров");
            Console.WriteLine("[7] Изменить площадь пассажирской каюты");

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
                    Console.Write("Введите число пассажиров: ");
                    int changePassangers;
                    try
                    {
                        changePassangers = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        changePassangers = 0;
                    }
                    try
                    {
                        this.Passengers = changePassangers;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message + " Значение не будет изменено!");
                    }                    
                    break;
                case 7:
                    Console.Write("Введите площадь пассажирскорй каюты: ");
                    double changeSquare;
                    try
                    {
                        changeSquare = double.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message + " Переменной присвоится значение 0!");
                        changeSquare = 0;
                    }
                    try
                    {
                        this.PassengerCabinArea = changeSquare;
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
            return base.ToString() + $"\nЧисло пассажиров: {passengers}\nРесторан: {restaurant}\nПлощадь пассажирской каюты (м2): {passengerCabinArea}";
        }
    }
}
