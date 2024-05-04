using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    [Serializable]
    internal class PassengerShip : Ship, ICrewChanger
    {
        private int passengers;
        private readonly bool restaurant;
        private double passengerCabinArea;

        public PassengerShip()
        {
            Type = ShipType.Пассажирский;
            passengers = 10;
            restaurant = true;
            passengerCabinArea = 30;
        }

        public PassengerShip(string name, ShipType type, int crew, double maxSpeed, int year, int passangers, bool restaurant, double passengerCabinArea) : base(name, type, crew, maxSpeed, year)
        {
            Passengers = passangers;
            this.restaurant = restaurant;
            PassengerCabinArea = passengerCabinArea;
        }

        public int Passengers 
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
        public bool Restaurant => restaurant;
        public double PassengerCabinArea 
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


        public void ChangeCrew(int increment) // Метод класса, изменяющий численность экипажа
        {
            Crew += increment;
            if (Crew < 0)
            {
                throw new ArgumentOutOfRangeException("Число членов экипажа не может быть отрицательным!");
            }
        }       

        public override void EditData()
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
            int editChoice = int.Parse(Console.ReadLine());
            while (editChoice < 1 || editChoice > 7)
            {
                Console.WriteLine("Неверный выбор! Попробуйте еще раз: ");
                editChoice = int.Parse(Console.ReadLine());
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
                    int crewInc = int.Parse(Console.ReadLine());
                    this.ChangeCrew(crewInc);
                    break;
                case 3:
                    Console.Write("На сколько изменить рекомендуемую скорость([+] число увеличивает экипаж, [-] уменьшает: ): ");
                    int speedInc = int.Parse(Console.ReadLine());
                    this.ChangeRecommendedSpeed(speedInc);
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
                    this[this.NumberRepair] = date;
                    break;
                case 5:
                    Console.Write("Введите год выпуска: ");
                    int changeYear = int.Parse(Console.ReadLine());
                    this.Year = changeYear;
                    break;
                case 6:
                    Console.Write("Введите число пассажиров: ");
                    int changePassangers = int.Parse(Console.ReadLine());
                    this.Passengers = changePassangers;
                    break;
                case 7:
                    Console.Write("Введите площадь пассажирскорй каюты: ");
                    double changeSquare = double.Parse(Console.ReadLine());
                    this.PassengerCabinArea = changeSquare;
                    break;
            }
        }

        //public override void ShowTypeShip()
        //{
        //    Console.WriteLine("Тип корабля: Пассажирский");            
        //}

        public override string ToString()
        {
            return base.ToString() + $"\nЧисло пассажиров: {passengers}\nРесторан: {restaurant}\nПлощадь пассажирской каюты (м2): {passengerCabinArea}";
        }
    }
}
