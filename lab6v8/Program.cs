using System;

namespace Lab6v8
{
    public class Device
    {
        private string _brand;
        private string _model;

        public string Brand
        {
            get => _brand;
            set => _brand = value;
        }

        public string Model
        {
            get => _model;
            set => _model = value;
        }

        public Device(string brand, string model)
        {
            _brand = brand;
            _model = model;
        }

        public virtual void PowerOn()
        {
            Console.WriteLine($"[Device] Пристрій {Brand} {Model} вмикається...");
        }

        public string GetDeviceType()
        {
            return "Загальний пристрій (Device)";
        }
    }

    public class Smartphone : Device
    {
        public double ScreenSize { get; set; }

        public Smartphone(string brand, string model, double screenSize)
            : base(brand, model)
        {
            ScreenSize = screenSize;
        }

        public override void PowerOn()
        {
            Console.WriteLine($"[Smartphone] Смартфон {Brand} {Model} ({ScreenSize}\") завантажує ОС Android/iOS...");
        }

        public void MakeCall(string number)
        {
            Console.WriteLine($"[Smartphone] Телефонуємо на номер {number} з пристрою {Brand} {Model}...");
        }

        public new string GetDeviceType()
        {
            return "Смартфон (Smartphone)";
        }
    }

    public class Laptop : Device
    {
        public string Processor { get; set; }

        public Laptop(string brand, string model, string processor)
            : base(brand, model)
        {
            Processor = processor;
        }

        public override void PowerOn()
        {
            Console.WriteLine($"[Laptop] Ноутбук {Brand} {Model} (Процесор: {Processor}) запускає системне самотестування та вмикає екран...");
        }

        public void RunProgram(string programName)
        {
            Console.WriteLine($"[Laptop] Запуск програми {programName} на процесорі {Processor}...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ (override) ===\n");

            Device[] devices = new Device[]
            {
                new Device("Generic", "BaseModel"),
                new Smartphone("Apple", "iPhone 15", 6.1),
                new Laptop("ASUS", "ROG Zephyrus", "Intel Core i9")
            };

            foreach (var device in devices)
            {
                device.PowerOn();
            }

            Console.WriteLine("\n=== 2. ВИКЛИК УНІКАЛЬНИХ МЕТОДІВ ПІДКЛАСІВ ===\n");

            Smartphone myPhone = new Smartphone("Samsung", "Galaxy S24", 6.8);
            Laptop myLaptop = new Laptop("Lenovo", "ThinkPad", "AMD Ryzen 7");

            myPhone.MakeCall("+380970000000");
            myLaptop.RunProgram("Visual Studio Code");

            Console.WriteLine("\n=== 3. ДЕМОНСТРАЦІЯ РІЗНИЦІ МІЖ override ТА new ===\n");

            Smartphone phoneObj = new Smartphone("Google", "Pixel 8", 6.2);
            Device deviceRef = phoneObj;

            Console.WriteLine("A. Виклики через посилання типу Smartphone:");
            Console.WriteLine($"   phoneObj.GetDeviceType() [new]:      {phoneObj.GetDeviceType()}");
            Console.Write("   phoneObj.PowerOn() [override]:       ");
            phoneObj.PowerOn();

            Console.WriteLine("\nB. Виклики того ж об'єкта через посилання типу Device:");
            Console.WriteLine($"   deviceRef.GetDeviceType() [new]:     {deviceRef.GetDeviceType()}");
            Console.Write("   deviceRef.PowerOn() [override]:      ");
            deviceRef.PowerOn();
        }
    }
}