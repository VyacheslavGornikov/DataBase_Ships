using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    enum ShipType // перечисление с типами корабля
    {
        Неизвестный,
        Пассажирский,
        Военный
    }
    [Serializable] // сериализуемый
    internal abstract class Ship
    {
        // Поля класса Ship
        static string port; // статическое поле "название порта"
        static int maxRepair; // статическое поле "количество максимально возможных ремонтов"
        private string name; // название корабля
        private ShipType type; // тип корабля
        private int crew; // количество членов экипажа
        private double recommendedSpeed; // рекомендуемая скорость хода корабля
        private int year; // год выпуска
        private DateTime[] RepairDate; // массив с датами ремонта
        private int numberRepair; // параметр, отслеживающий текущее количество ремонтов

        // Свойства полей класса
        public string Name { get => name; set => name = value; } // свойство поля name
        public double RecommendedSpeed // свойство поля recommendedSpeed
        { 
            get => recommendedSpeed; 
            set
            {
                if (value > 0)
                    recommendedSpeed = value;
                else
                    throw new ArgumentOutOfRangeException("Рекомендуемая скорость должна быть положительной!");

            } 
        }
        public DateTime this[int index] // индексатор для поля-массива RepairDate
        { 
            get
            {
                if (index >= 0 && index < numberRepair)
                    return RepairDate[index];
                else                
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за границы массива");                                          
            } 
            set
            {
                if (index >= 0 && index < RepairDate.Length)
                    RepairDate[numberRepair++] = value;
                else
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за границы массива");
            } 
        }
        public int NumberRepair { get => numberRepair; } // свойство поля numberRepair
        public int Year // свойство поля year
        { 
            get => year; 
            set
            {
                if (value >= 2000)
                    year = value;
                else
                    throw new ShipException($"Год выпуска {value} не может быть меньше 2000");
            } 
        }

        
        public int Crew // свойство поля crew
        { 
            get => crew; 
            set
            {
                if(value > 0)
                    crew = value;
                else                
                    throw new ArgumentOutOfRangeException("Число членов экипажа не может быть отрицательным!");                                    
            } 
        }
        public static string Port { get => port; set => port = value; } // статическое свойство поля port
        public static int MaxRepair { get => maxRepair; set => maxRepair = value; } // статическое свойство поля maxRepair
        internal ShipType Type { get => type; set => type = value; }

        // Конструкторы
        public Ship() // Конструктор без параметров
        {
            name = "Без Названия";
            Type = ShipType.Неизвестный;
            crew = 0;
            recommendedSpeed = 1;
            year = 2000;
            RepairDate = new DateTime[maxRepair];
        }

        public Ship(string name, ShipType type, int crew, double maxSpeed, int year) // Конструктор с параметрами
        {
            Name = name;
            Type = type;
            Crew = crew;
            RecommendedSpeed = maxSpeed;
            Year = year;
            RepairDate = new DateTime[maxRepair];
        }

        

        public void ChangeRecommendedSpeed(int increment) // Метод класса, изменяющий рекомендуемую скорость
        {
            recommendedSpeed += increment;
            if (recommendedSpeed <= 0)
            {
                throw new ArgumentOutOfRangeException("Рекомендуемая скорость должна быть положительной!");
            }
        }

        public override string ToString() // Переопределенный метод класса Object для вывода объекта класса через консоль
        {
            string str = $"Порт {port}\nТип корабля: {type}\nНазвание: {name}\nЭкипаж: {crew} человек\nРекомендуемая скорость(в узлах): {recommendedSpeed}\nГод выпуска: {year}";
            StringBuilder finStr = new StringBuilder(str);
            if (numberRepair > 0)
            {
                finStr.Append("\nДаты ремонта:\n");
                for (int i = 0;  i < numberRepair; i++)
                {
                    finStr.Append(RepairDate[i].ToShortDateString()).Append("   ");
                }
            }
            return finStr.ToString();
        }

        public abstract void EditData(); // абстрактный метод класса             
    }
}
