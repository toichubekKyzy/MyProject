class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Введите первое большое целое число:");
            string num1 = Console.ReadLine();

            Console.WriteLine("Введите второе большое целое число:");
            string num2 = Console.ReadLine();

            BigInteger bigInt1 = new BigInteger(num1);
            BigInteger bigInt2 = new BigInteger(num2);

            // Примеры использования с перегрузкой операторов
            Console.WriteLine($"Сложение: {bigInt1 + bigInt2}");
            Console.WriteLine($"Вычитание: {bigInt1 - bigInt2}");
            Console.WriteLine($"Умножение: {bigInt1 * bigInt2}");

            // Реализация деления с обработкой исключений
            try
            {
                Console.WriteLine($"Деление: {bigInt1 / bigInt2}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
