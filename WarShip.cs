using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    [Serializable]
    internal class WarShip : Ship, ICrewChanger
    {
        private int guns;
        private int gunCaliber;
        private readonly bool torpedoes;

        public WarShip()
        {
            Type = ShipType.Военный;
            guns = 2;
            gunCaliber = 76;
            torpedoes = false;
        }

        public WarShip(string name, ShipType type, int crew, double maxSpeed, int year, int guns, int gunCaliber, bool torpedoes) : base(name, type, crew, maxSpeed, year)
        {
            Guns = guns;
            GunCaliber = gunCaliber;
            this.torpedoes = torpedoes;
        }

        public int Guns { get => guns; set => guns = value; }
        public int GunCaliber { get => gunCaliber; set => gunCaliber = value; }
        public bool Torpedoes => torpedoes;

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
            Console.WriteLine("[6] Изменить количество орудий");
            Console.WriteLine("[7] Изменить калибр орудий");

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
                    Console.Write("Введите количество орудий: ");
                    int changeGuns = int.Parse(Console.ReadLine());
                    this.Guns = changeGuns;
                    break;
                case 7:
                    Console.Write("Введите калибр орудий: ");
                    int changeGunCaliber = int.Parse(Console.ReadLine());
                    this.GunCaliber = changeGunCaliber;
                    break;
            }
        }
        public override string ToString()
        {
            return base.ToString() + $"\nКоличество орудий: {guns}\nКалибр орудий: {GunCaliber}\nНаличие торпед: {torpedoes}";
        }

    }
}
